using Game.SinglePlayer;
using UnityEngine;

namespace Game.MultiPlayer {
    public class MultiplayerMatchModel : Model {
        public readonly bool IsHost;

        public float TopPlatformPos { get; set; }
        public Vector2 BallPosition { get; set; }

        public MultiplayerMatchModel(bool isHost, GameData data) : base(data) {
            IsHost = isHost;
        }
    }
}