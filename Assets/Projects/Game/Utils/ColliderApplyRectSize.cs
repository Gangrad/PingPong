using Unity.Utils;
using UnityEngine;

namespace Game.Utils {
    [RequireComponent(typeof(BoxCollider2D))]
    public class ColliderApplyRectSize : MonoBehaviour {
        private void Awake() {
            var boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.ApplyRectSize();
        }
    }
}