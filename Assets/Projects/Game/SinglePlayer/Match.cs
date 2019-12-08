using CsExtensions;
using Game.Gameplay;
using Unity.Utils;

namespace Game.SinglePlayer {
    public class Match : IUpdatable {
        private readonly GameService _service;
        private readonly GameView _view;
        private IInputListener _inputListener;

        private Model Model { get { return _service.Model; } }

        public Match(GameService gameService, GameView view) {
            _service = gameService;
            _view = view;
            _service.OnStartGame.Subscribe(StartGame).DisposeBy(_view);
            _view.OnBallLeaveField.Subscribe(EndGame).DisposeBy(_view);
            _view.OnBallBounced.Subscribe(_service.OnBallBounced).DisposeBy(_view);
            _service.OnScoreChanged.Subscribe(UpdateScore).DisposeBy(_view);
            UpdateScore();
        }

        private void StartGame() {
            _view.gameObject.SetActive(true);
            var gameParams = new GameParams {
                BallSize = RandomUtils.GetRandomInRange(Model.BallSizeRange),
                BallColor = Model.RandomBallColorOnStart ? RandomUtils.GetRandomColor() : Model.BallColor,
                StartForce = Model.GetStartForce()
            };
            _view.SetupBall(gameParams);
            var startPos = _view.Width / 2;
            _inputListener = Model.ControlType == ControlType.ControlPanels
                ? new ControlSideListener(_view.LeftSide, _view.RightSide, Model.PlatformSpeed)
                : (IInputListener) new PointerPositionListener();
            _view.MovePlatforms(startPos);
        }

        private void EndGame(Side winner) {
            _service.EndGame(winner);
            _view.gameObject.SetActive(false);
        }

        private void UpdateScore() {
            _view.UpdateScore(Model.CurrentScore, Model.BestScore);
        }

        public void Update() {
            _inputListener.Update();
            _view.MovePlatforms(_inputListener.XPosition);
        }
    }
}