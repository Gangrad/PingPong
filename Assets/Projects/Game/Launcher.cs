using CsExtensions;
using Game.Gameplay;
using Game.MultiPlayer;
using Game.SinglePlayer;
using Game.UI;
using Game.Utils;
using Unity.Utils;
using UnityEngine;

namespace Game {
    public class Launcher : DisposableBehaviour {
        [SerializeField] private GameSettings _settings;
        [SerializeField] private GameView _gameView;
        [SerializeField] private Menu _menu;
        private IUpdatable _updatable;
        private AppData _appData;
        private GameData _gameData;
        private MultiPlayerNetService _netService;
        private readonly Signal _startSingleGame = new Signal();
        private readonly ActionsQueue _mainThreadActions = new ActionsQueue();

        private void Awake() {
            _appData = new AppData();
            _gameData = new GameData(_settings);
            _startSingleGame.Subscribe(StartSinglePlayerGame).DisposeBy(this);
        }

        private void Start() {
            _netService = new MultiPlayerNetService(_mainThreadActions);
            _netService.OnStartGame.Subscribe(StartMultiplayerGame).DisposeBy(this);
            _appData.OnAppClose.Subscribe(_netService.Close).DisposeBy(this);
            _menu.Bind(_gameData, _startSingleGame, _appData, _netService);
            _gameView.gameObject.SetActive(false);
        }

        private void Update() {
            _mainThreadActions.Invoke();
            if (_updatable != null)
                _updatable.Update();
        }

        private void StartSinglePlayerGame() {
            var gameService = new GameService(_gameData);
            gameService.OnStartGame.Subscribe(_appData.OnStartGame.Dispatch).DisposeBy(this);
            gameService.OnGameOver.Subscribe(side => _appData.OnGameOver.Dispatch()).DisposeBy(this);
            _updatable = new Match(gameService, _gameView);
            gameService.StartGame();
        }

        private void StartMultiplayerGame(bool isHost) {
            Debug.LogFormat("launcher start multiplayer, {0}", isHost);
            var gameService = new MultiPlayerGameService(isHost, _gameData, _netService);
            gameService.OnStartGame.Subscribe(_appData.OnStartGame.Dispatch).DisposeBy(this);
            gameService.OnGameOver.Subscribe(side => _appData.OnGameOver.Dispatch()).DisposeBy(this);
            _updatable = new NetworkMatch(gameService, _gameView);
            _appData.OnGameOver.SubscribeOnce(() => { _netService.Close(); }).DisposeBy(this);
            _mainThreadActions.Enqueue(gameService.StartGame);
        }

        private void OnApplicationQuit() {
            _appData.OnAppClose.Dispatch();
        }
    }
}