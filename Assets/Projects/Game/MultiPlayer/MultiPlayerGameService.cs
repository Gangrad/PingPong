using CsExtensions;
using Game.Gameplay;
using Unity.Utils;
using UnityEngine;

namespace Game.MultiPlayer {
    public class MultiPlayerGameService {
        private const float SendDelay = .01f;
        private readonly MultiPlayerNetService _netService;
        public readonly MultiplayerMatchModel Model;
        private readonly DisposableCollection _disposableCollection = new DisposableCollection();

        public readonly Signal OnStartGame = new Signal();
        public readonly Signal<GameParams> OnReceiveGameParams = new Signal<GameParams>();
        public readonly Signal OnScoreChanged = new Signal();
        public readonly Signal<Side> OnReceiveGameOver = new Signal<Side>();
        public readonly Signal<Side> OnGameOver = new Signal<Side>();

        private float _lastSentTime;

        public MultiPlayerGameService(bool isHost, GameData gameData, MultiPlayerNetService netService) {
            _netService = netService;
            Model = new MultiplayerMatchModel(isHost, gameData);
            _netService.OnGameParamsReceived.Subscribe(OnReceiveGameParams.Dispatch).DisposeBy(_disposableCollection);
            _netService.OnPlatformMoved.Subscribe(pos => Model.TopPlatformPos = pos).DisposeBy(_disposableCollection);
            _netService.OnBallMoved.Subscribe(pos => Model.BallPosition = pos.InverseY()).DisposeBy(_disposableCollection);
            _netService.OnGameOverReceived.Subscribe(side => OnReceiveGameOver.Dispatch(side.Opposite()))
                .DisposeBy(_disposableCollection);
        }

        public void StartGame() {
            Debug.Log("Start multiplayer game");
            OnStartGame.Dispatch();
            Model.ResetScore();
        }

        public void OnBallBounced() {
            Model.IncScore();
            OnScoreChanged.Dispatch();
        }

        public void EndGame(Side winner) {
            Debug.LogFormat("End multiplayer game, winner: {0}", winner);
            if (winner == Side.Bottom)
                Model.CheckBestScore();
            OnGameOver.Dispatch(winner);
            if (Model.IsHost) {
                _netService.SendGameOver(winner);
            }
            _disposableCollection.Dispose();
        }
        
        public void SendGameParams(GameParams gameParams) {
            _netService.SendGameParams(gameParams);
        }

        public void UpdateFrameData(float platformPos, Vector2 ballPos) {
            var t = Time.time;
            if (t - _lastSentTime < SendDelay)
                return;
            _netService.SendPlatformPosition(platformPos);
            if (Model.IsHost)
                _netService.SendBallPositions(ballPos);
            _lastSentTime = t;
        }
    }
}