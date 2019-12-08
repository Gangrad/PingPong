using System;
using Unity.Utils;
using UnityEngine;

namespace Game {
    [Serializable]
    public class GameSettings {
        public Vector2 BallSizeRange;
        public Vector2 StartXForceRange;
        public Vector2 StartYAbsForceRange;
        public float PlatformSpeed;

        public Vector2 GetStartForce() {
            var xForce = RandomUtils.GetRandomInRange(StartXForceRange);
            var yForce = RandomUtils.GetRandomInRange(StartYAbsForceRange);
            var yForceSign = RandomUtils.GetRandomBool();
            if (yForceSign)
                yForce *= -1;
            return new Vector2(xForce, yForce);
        }
    }
}