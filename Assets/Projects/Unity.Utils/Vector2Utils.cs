using UnityEngine;

namespace Unity.Utils {
    public static class Vector2Utils {
        public static Vector2 InverseY(this Vector2 v) {
            return new Vector2(v.x, -v.y);
        }
    }
}