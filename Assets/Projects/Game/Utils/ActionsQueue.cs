using System;
using System.Collections.Generic;
using CsExtensions;

namespace Game.Utils {
    public class ActionsQueue {
        private readonly Queue<Action> _actions = new Queue<Action>();
        private readonly object _obj = new object();

        public void Enqueue(Action action) {
            lock (_obj) {
                _actions.Enqueue(action);
            }
        }

        public void Invoke() {
            lock (_obj) {
                while (_actions.Count > 0)
                    _actions.Dequeue().SafeInvoke();
            }
        }
    }
}