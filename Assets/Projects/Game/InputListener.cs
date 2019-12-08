using Unity.Utils;
using UnityEngine;

namespace Game {
    public interface IInputListener : IUpdatable {
        float XPosition { get; }
    }

    public class PointerPositionListener : IInputListener {
        public float XPosition { get { return Input.mousePosition.x; } }

        void IUpdatable.Update() { }
    }

    public class ControlSideListener : IInputListener {
        private readonly IPressable _leftControl;
        private readonly IPressable _rightControl;
        private readonly float _speed;
        public float XPosition { get; private set; }

        public ControlSideListener(IPressable leftControl, IPressable rightControl, float speed) {
            _leftControl = leftControl;
            _rightControl = rightControl;
            _speed = speed;
        }

        public void Update() {
            if (_leftControl.Pressed) {
                if (_rightControl.Pressed)
                    return;
                XPosition -= _speed * Time.deltaTime;
            } else if (_rightControl.Pressed)
                XPosition += _speed * Time.deltaTime;
        }
    }
}