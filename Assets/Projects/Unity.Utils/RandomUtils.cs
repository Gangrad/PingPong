using UnityEngine;

namespace Unity.Utils {
    public static class RandomUtils {
        public static float GetRandomInRange(Vector2 range) {
            return Random.Range(range.x, range.y);
        }

        public static bool GetRandomBool() {
            return Random.Range(0, 2) == 0;
        }

        public static Color GetRandomColor() {
            return new Color(GetRandomFloat01(), GetRandomFloat01(), GetRandomFloat01(), GetRandomFloat01());
        }

        public static float GetRandomFloat01() {
            return Random.Range(0f, 1f);
        }
    }
}