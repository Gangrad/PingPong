using CsExtensions;
using Game.Gameplay;
using Unity.Utils;
using UnityEngine;

namespace Game.MultiPlayer {
    public class NetworkMatch : IUpdatable {
         private readonly MultiPlayerGameService _service;
        private readonly GameView _view;
        private IInputListener _inputListener;
        private bool _active;
       
        private MultiplayerMatchModel Model { get { return _service.Model; } }

        public NetworkMatch(MultiPlayerGameService gameService, GameView view) {
            _service = gameService;
            _view = view;
            _service.OnStartGame.Subscribe(StartGame).DisposeBy(_view);
            if (Model.IsHost)
                _view.OnBallLeaveField.Subscribe(EndGame).DisposeBy(_view);
            else
                _service.OnReceiveGameOver.Subscribe(EndGame).DisposeBy(_view);

            _view.OnBallBounced.Subscribe(_service.OnBallBounced).DisposeBy(_view);
            _service.OnReceiveGameParams.Subscribe(_view.SetupBall).DisposeBy(_view);
            _service.OnScoreChanged.Subscribe(UpdateScore).DisposeBy(_view);
            UpdateScore();
        }

        private void StartGame() {
            Debug.LogFormat("start network match, is host {0}", Model.IsHost);
            _view.gameObject.SetActive(true);
            if (Model.IsHost) {
                var gameParams = new GameParams {
                    BallSize = RandomUtils.GetRandomInRange(Model.BallSizeRange),
                    BallColor = Model.RandomBallColorOnStart ? RandomUtils.GetRandomColor() : Model.BallColor,
                    StartForce = Model.GetStartForce()
                };
                _view.SetupBall(gameParams);
                _service.SendGameParams(gameParams);
            }
            var startPos = _view.Width / 2;
            _inputListener = Model.ControlType == ControlType.ControlPanels
                ? new ControlSideListener(_view.LeftSide, _view.RightSide, Model.PlatformSpeed)
                : (IInputListener)new PointerPositionListener();
            _view.MovePlatforms(startPos);
            _active = true;
        }

        private void EndGame(Side winner) {
            _service.EndGame(winner);
            _view.gameObject.SetActive(false);
            _active = false;
        }

       private void UpdateScore() {
           _view.UpdateScore(Model.CurrentScore, Model.BestScore);
       }

        public void Update() {
            if (!_active)
                return;
            ApplyNetworkData();
            _inputListener.Update();
            _view.MovePlatform(Side.Bottom, _inputListener.XPosition);
            _service.UpdateFrameData(_view.BottomPlatformPos, _view.BallPosition);
        }

        private void ApplyNetworkData() {
            ApplyOtherPlayerPosition();
            if (!Model.IsHost)
                ApplyBallPosition();
        }

        private void ApplyOtherPlayerPosition() {
            _view.MovePlatform(Side.Top, Model.TopPlatformPos);
        }

        private void ApplyBallPosition() {
            _view.MoveBall(Model.BallPosition);
        }
    }
}