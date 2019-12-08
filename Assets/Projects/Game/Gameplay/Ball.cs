using CsExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay {
    public class Ball : MonoBehaviour {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CircleCollider2D _collider;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Image _image;
        public readonly Signal<Side> OnLeave = new Signal<Side>();
        public readonly Signal OnBounced = new Signal();
        
        public Vector2 Position { get { return transform.localPosition; } }

        public void SetColor(Color color) {
            _image.color = color;
        }

        public void SetSize(float size) {
            _rectTransform.sizeDelta = new Vector2(size, size);
            _collider.radius = size / 2;
        }

        public void Reset() {
            SetPosition(Vector2.zero);
            _rigidbody.velocity = Vector2.zero;
        }

        public void Push(Vector2 force) {
            _rigidbody.AddForce(force, ForceMode2D.Force);
        }

        public void SetPosition(Vector2 pos) {
            transform.localPosition = pos;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag(TagNames.Player)) OnBounced.Dispatch();
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag(TagNames.Field)) {
                var winner = transform.position.y > 0 ? Side.Bottom : Side.Top;
                OnLeave.Dispatch(winner);
                return;
            }
            Debug.LogWarningFormat("Ball exits trigger with unexpected tag: {0}", tag);
        }
    }
}