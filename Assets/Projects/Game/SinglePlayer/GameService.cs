using CsExtensions;
using Game.Gameplay;
using UnityEngine;

namespace Game.SinglePlayer {
    public class GameService {
        public readonly Signal OnStartGame = new Signal();
        public readonly Signal OnScoreChanged = new Signal();
        public readonly Signal<Side> OnGameOver = new Signal<Side>();
        
        public readonly Model Model;

        public GameService(GameData gameData) {
            Model = new Model(gameData);
        }

        public void StartGame() {
            Debug.Log("Start game");
            OnStartGame.Dispatch();
            Model.ResetScore();
        }

        public void OnBallBounced() {
            Model.IncScore();
            OnScoreChanged.Dispatch();
        }

        public void EndGame(Side winner) {
            Debug.LogFormat("End game, winner: {0}", winner);
            if (winner == Side.Bottom)
                Model.CheckBestScore();
            OnGameOver.Dispatch(winner);
        }
    }
}