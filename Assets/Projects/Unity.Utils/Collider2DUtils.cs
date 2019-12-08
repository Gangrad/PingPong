using UnityEngine;

namespace Unity.Utils {
    public static class Collider2DUtils {
        public static void ApplyRectSize(this BoxCollider2D collider) {
            var rect = ((RectTransform) collider.transform).rect;
            collider.offset = rect.center;
            collider.size = rect.size;
        }
    }
}