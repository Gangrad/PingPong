using Unity.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay {
    public class Platform : MonoBehaviour {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Image _image;
        
        public float Width { get { return ((RectTransform) transform).rect.width; } }
        public float Position { get { return transform.position.x; } }

        private void Awake() {
            _collider.ApplyRectSize();
        }

        public void SetColor(Color color) {
            _image.color = color;
        }

        public void UpdatePosition(float xPos) {
            var pos = transform.position;
            transform.position = new Vector3(xPos, pos.y, pos.z);
        }
    }
}