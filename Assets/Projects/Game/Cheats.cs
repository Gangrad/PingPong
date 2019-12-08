using UnityEngine;
#if UNITY_EDITOR
using System.Net;
using CsExtensions;
using DataHelper;
using Game.Gameplay;
using Game.MultiPlayer;
using Unity.Utils;
#endif

namespace Game {
    public class Cheats : MonoBehaviour {
#if UNITY_EDITOR
        private static IPAddress CheatIP { get { return IPAddress.Parse("127.0.0.2"); } }
        private Launcher Launcher { get { return FindObjectOfType<Launcher>(); } }
        
        [Button]
        public void StartHostGame() {
            Launcher.InvokeMethod("StartMultiplayerGame", true);
        }

        [Button]
        public void CheatHandleConnectedClient() {
            var netService = Launcher.GetFieldValue<MultiPlayerNetService>("_netService");
            netService.InvokeMethod("OnHostReceiveData", new byte[]{18}, 1, CheatIP);
            StartCoroutine(CheatSendInput(netService));
        }

        private System.Collections.IEnumerator CheatSendInput(MultiPlayerNetService netService) {
            var inputListener = new PointerPositionListener();
            var delay = new WaitForSeconds(.2f);
            yield return null;
            var gameView = Launcher.GetFieldValue<GameView>("_gameView");
            while (gameView.gameObject.activeSelf) {
                yield return delay;
                var pos = inputListener.XPosition;
                var bw = new ByteWriter(5);
                bw.WriteByte(20);
                bw.WriteFloat(pos);
                netService.InvokeMethod("OnHostReceiveData", bw.Data, 5, CheatIP);
            }
        }
#endif
    }
}