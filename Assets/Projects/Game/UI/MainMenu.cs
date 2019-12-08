using CsExtensions;
using TextMeshPro.Utils;
using TMPro;
using Unity.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _startMultiplayerButton;
        [SerializeField] private Toggle _randomColorOnStart;
        [SerializeField] private FlexibleColorPicker _colorPicker;
        [SerializeField] private TMP_Dropdown _dropdown;
        
        public readonly Signal OnStartGameClicked = new Signal();
        public readonly Signal OnMultiplayerGameClicked = new Signal();
        public readonly Signal<bool> OnToggleRandomColorOnStart = new Signal<bool>();
        public readonly Signal<Color> OnBallColorChanged = new Signal<Color>();
        public readonly Signal<ControlType> OnControlTypeChanged = new Signal<ControlType>();
        private Color _ballColor;
        
        private void Start() {
            _startGameButton.SetOnClickAction(OnStartGameClicked.Dispatch);
            _randomColorOnStart.SetOnChangedAction(value => {
                OnToggleRandomColorOnStart.Dispatch(value);
                UpdateColorPickerState();
            });
            _dropdown.SetOnValueChangedAction(OnControlTypeIndexChanged);
            _startMultiplayerButton.SetOnClickAction(OnMultiplayerGameClicked.Dispatch);
        }

        private void Update() {
            var pickedColor = _colorPicker.color;
            if (pickedColor != _ballColor) {
                _ballColor = pickedColor;
                OnBallColorChanged.Dispatch(_ballColor);
            }
        }

        public void Init(GameData data) {
            _randomColorOnStart.isOn = data.RandomColorOnStart;
            _colorPicker.color = data.BallColor;
            _dropdown.value = EnumUtils.GetIndex(data.ControlType);
            UpdateColorPickerState();
        }

        private void UpdateColorPickerState() {
            _colorPicker.gameObject.SetActive(!_randomColorOnStart.isOn);
        }

        private void OnControlTypeIndexChanged(int id) {
            var value = EnumUtils.GetValues<ControlType>()[id];
            OnControlTypeChanged.Dispatch(value);
        }
    }
}