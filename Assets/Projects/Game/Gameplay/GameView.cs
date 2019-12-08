using CsExtensions;
using Game.SinglePlayer;
using TMPro;
using Unity.Utils;
using UnityEngine;

namespace Game.Gameplay {
    public class GameView : DisposableBehaviour {
        [SerializeField] private Ball _ball;
        [SerializeField] private Platform _platformTop;
        [SerializeField] private Platform _platformBottom;
        [SerializeField] private PressButton _leftPanel;
        [SerializeField] private PressButton _rightPanel;
        [SerializeField] private TextMeshProUGUI _scoreLabel;
        
        public readonly Signal<Side> OnBallLeaveField = new Signal<Side>();
        public readonly Signal OnBallBounced = new Signal();
        
        public float Width { get { return ((RectTransform) transform).rect.width; } }
        public float BottomPlatformPos { get { return _platformBottom.Position; } }
        public Vector2 BallPosition { get { return _ball.Position; } }
        
        public IPressable LeftSide { get { return _leftPanel; } }
        public IPressable RightSide { get { return _rightPanel; } }

        private void Start() {
            _ball.OnBounced.Subscribe(OnBallBounced.Dispatch).DisposeBy(this);
            _ball.OnLeave.Subscribe(OnBallLeaveField.Dispatch).DisposeBy(this);
        }

        public void SetupControlPanels(bool state) {
            _leftPanel.gameObject.SetActive(state);
            _rightPanel.gameObject.SetActive(state);
        }

        public void SetupBall(GameParams gameParams) {
            _ball.Reset();
            _ball.SetSize(gameParams.BallSize);
            _ball.SetColor(gameParams.BallColor);
            _ball.Push(gameParams.StartForce);
        }
        
        public void MovePlatforms(float xPos) {
            MovePlatform(_platformTop, xPos);
            MovePlatform(_platformBottom, xPos);
        }

        public void MovePlatform(Side side, float xPos) {
            var platform = side == Side.Top ? _platformTop : _platformBottom;
            MovePlatform(platform, xPos);
        }

        private void MovePlatform(Platform platform, float xPos) {
            platform.UpdatePosition(Mathf.Clamp(xPos, platform.Width / 2, Width - platform.Width / 2));
        }

        public void MoveBall(Vector2 pos) {
            _ball.SetPosition(pos);
        }

        public void UpdateScore(int current, int best) {
            _scoreLabel.text = string.Format("Score: {0} (best: {1})", current, best);
        }
    }
}