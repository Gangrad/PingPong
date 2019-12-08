using UnityEngine;

namespace Game.SinglePlayer {
    public class Model {
        private readonly GameData _data;
        public ControlType ControlType { get { return _data.ControlType; } }
        public Vector2 BallSizeRange { get { return _data.Settings.BallSizeRange; } }
        public bool RandomBallColorOnStart { get { return _data.RandomColorOnStart; } }
        public Color BallColor { get { return _data.BallColor; } }
        public int CurrentScore { get; private set; }
        public int BestScore { get { return _data.BestScore; } }
        public float PlatformSpeed { get { return _data.Settings.PlatformSpeed; } }

        public Model(GameData data) {
            _data = data;
        }

        public void ResetScore() {
            CurrentScore = 0;
        }

        public void IncScore() {
            CurrentScore++;
        }

        public void CheckBestScore() {
            if (CurrentScore > _data.BestScore)
                _data.BestScore = CurrentScore;
        }

        public Vector2 GetStartForce() {
            return _data.Settings.GetStartForce();
        }
    }
}