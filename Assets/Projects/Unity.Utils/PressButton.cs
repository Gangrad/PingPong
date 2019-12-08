using CsExtensions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity.Utils {
    public interface IPressable {
        bool Pressed { get; }
    }
    
    public class PressButton : Graphic, IPointerDownHandler, IPointerUpHandler, IPressable {
        public readonly Signal OnPressed = new Signal();
        public bool Pressed { get; private set; }

        protected override void OnDisable() {
            base.OnDisable();
            Pressed = false;
        }

        private void Update() {
            if (Pressed)
                OnPressed.Dispatch();
        }
        
        public void OnPointerDown(PointerEventData eventData) {
            Pressed = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            Pressed = false;
        }
    }
}