using CsExtensions;
using Game.MultiPlayer;
using Unity.Utils;
using UnityEngine;

namespace Game.UI {
    public class Menu : DisposableBehaviour {
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private MultiplayerMenu _multiplayerMenu;
        
        private void Start() {
            _mainMenu.OnMultiplayerGameClicked.Subscribe(() => {
                _mainMenu.DeactivateGameObject();
                _multiplayerMenu.ActivateGameObject();
            }).DisposeBy(this);
            _multiplayerMenu.OnBackButtonClicked.Subscribe(() => {
                _mainMenu.ActivateGameObject();
                _multiplayerMenu.DeactivateGameObject();
            }).DisposeBy(this);
            
            _mainMenu.ActivateGameObject();
            _multiplayerMenu.DeactivateGameObject();
        }

        public void Bind(GameData gameData, Signal startSingleGame, AppData appData, MultiPlayerNetService netService) {
            _mainMenu.Init(gameData);

            appData.OnStartGame.Subscribe(() => {
                _mainMenu.SetObjectActive(false);
                _multiplayerMenu.SetObjectActive(false);
            }).DisposeBy(this);
            appData.OnGameOver.Subscribe(() => {
                _mainMenu.gameObject.SetActive(true);
                _multiplayerMenu.SetInteractable(true);
            }).DisposeBy(this);
            _mainMenu.OnStartGameClicked.Subscribe(startSingleGame.Dispatch).DisposeBy(this);
            _mainMenu.OnToggleRandomColorOnStart.Subscribe(value => gameData.RandomColorOnStart = value).DisposeBy(this);
            _mainMenu.OnBallColorChanged.Subscribe(value => gameData.BallColor = value).DisposeBy(this);
            _mainMenu.OnControlTypeChanged.Subscribe(value => gameData.ControlType = value).DisposeBy(this);

            _multiplayerMenu.OnHostButtonClicked.Subscribe(netService.StartHost).DisposeBy(this);
            _multiplayerMenu.OnConnectButtonClicked.Subscribe(netService.StartConnected).DisposeBy(this);
            _multiplayerMenu.OnBackButtonClicked.Subscribe(netService.Close).DisposeBy(this);
        }
    }
}