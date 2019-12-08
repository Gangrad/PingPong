using System.Net;
using CsExtensions;
using TMPro;
using Unity.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI {
    public class MultiplayerMenu : MonoBehaviour {
        [SerializeField] private Button _startHostButton;
        [SerializeField] private TextMeshProUGUI _myIpLabel;
        [SerializeField] private TMP_InputField _hostIpInput;
        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _backButton;
        
        public readonly Signal OnHostButtonClicked = new Signal();
        public readonly Signal<IPAddress> OnConnectButtonClicked = new Signal<IPAddress>();
        public readonly Signal OnBackButtonClicked = new Signal();

        private IPAddress EnteredAddress {
            get {
                IPAddress result;
                return IPAddress.TryParse(_hostIpInput.text, out result) ? result : null;
            }
        }

        private void Start() {
            _startHostButton.SetOnClickAction(() => {
                SetInteractable(false);
                OnHostButtonClicked.Dispatch();
            });
            _connectButton.SetOnClickAction(() => {
                var address = EnteredAddress;
                if (address != null) {
                    SetInteractable(false);
                    OnConnectButtonClicked.Dispatch(address);
                }
                else
                    Debug.LogWarningFormat("Cant parse ip address");
            });
            _backButton.SetOnClickAction(() => {
                SetInteractable(true);
                OnBackButtonClicked.Dispatch();
            });
            var myIp = NetUtils.GetLocalIPAddress();
            _myIpLabel.text = myIp;
            _hostIpInput.text = "192.168.0.20";
            SetInteractable(true);
        }

        public void SetInteractable(bool state) {
            _startHostButton.interactable = state;
            _hostIpInput.interactable = state;
            _connectButton.interactable = state;
        }
    }
}