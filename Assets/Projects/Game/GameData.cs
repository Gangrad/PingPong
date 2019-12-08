using Unity.Utils;
using UnityEngine;

namespace Game {
    public enum ControlType {
        ControlPanels,
        InputPosition
    }
    
    public class GameData {
        private const string RandomColorKey = "RandomColor";
        private const string ColorKey = "Color";
        private const string BestScoreKey = "BestScore";
        private const string ControlTypeKey = "Control";
        
        public readonly GameSettings Settings;
        private bool _randomColorOnStart;
        private Color _ballColor;
        private int _bestScore;
        private ControlType _controlType;

        public bool RandomColorOnStart {
            get { return _randomColorOnStart; }
            set {
                if (value == _randomColorOnStart)
                    return;
                _randomColorOnStart = value;
                SaveData();
            }
        }

        public Color BallColor {
            get {
                return _ballColor;
            }
            set {
                if (value == _ballColor)
                    return;
                _ballColor = value;
                SaveData();
            }
        }
        
        public int BestScore {
            get {
                return _bestScore;
            }
            set {
                if (value == _bestScore)
                    return;
                _bestScore = value;
                SaveData();
            }
        }

        public ControlType ControlType {
            get {
                return _controlType;
            }
            set {
                if (value == _controlType)
                    return;
                _controlType = value;
                SaveData();
            }
        }

        public GameData(GameSettings settings) {
            Settings = settings;
            LoadData();
        }
        
        private void LoadData() {
            _randomColorOnStart = PlayerPrefsUtils.GetBool(RandomColorKey, true);
            _ballColor = PlayerPrefsUtils.GetColor(ColorKey, Color.white);
            _bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
            _controlType = PlayerPrefsUtils.GetEnum(ControlTypeKey, ControlType.ControlPanels);
        }

        private void SaveData() {
            PlayerPrefsUtils.SetBool(RandomColorKey, _randomColorOnStart);
            PlayerPrefsUtils.SetColor(ColorKey, _ballColor);
            PlayerPrefs.SetInt(BestScoreKey, _bestScore);
            PlayerPrefsUtils.SetEnum(ControlTypeKey, _controlType);
        }
    }
}