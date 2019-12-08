using System;

namespace CsExtensions {
    public class Signal {
        private event Action Action;

        public IDisposable Subscribe(Action callback) {
            Action += callback;
            return new Disposable(() => Unsubscribe(callback));
        }

        public void Unsubscribe(Action callback) {
            Action -= callback;
        }

        public void Dispatch() {
            Action.SafeInvoke();
        }
    }
    
    public class Signal<T> {
        private event Action<T> Action;

        public IDisposable Subscribe(Action<T> callback) {
            Action += callback;
            return new Disposable(() => Unsubscribe(callback));
        }

        public void Unsubscribe(Action<T> callback) {
            Action -= callback;
        }

        public void Dispatch(T arg) {
            Action.SafeInvoke(arg);
        }
    }
    
    public class Signal<T1, T2> {
        private event Action<T1, T2> Action;

        public IDisposable Subscribe(Action<T1, T2> callback) {
            Action += callback;
            return new Disposable(() => Unsubscribe(callback));
        }

        public void Unsubscribe(Action<T1, T2> callback) {
            Action -= callback;
        }

        public void Dispatch(T1 arg1, T2 arg2) {
            Action.SafeInvoke(arg1, arg2);
        }
    }
    
    public class Signal<T1, T2, T3> {
        private event Action<T1, T2, T3> Action;

        public IDisposable Subscribe(Action<T1, T2, T3> callback) {
            Action += callback;
            return new Disposable(() => Unsubscribe(callback));
        }

        public void Unsubscribe(Action<T1, T2, T3> callback) {
            Action -= callback;
        }

        public void Dispatch(T1 arg1, T2 arg2, T3 arg3) {
            Action.SafeInvoke(arg1, arg2, arg3);
        }
    }

    public static class SignalUtils {
        public static IDisposable SubscribeAndInvoke(this Signal signal, Action callback) {
            var result = signal.Subscribe(callback);
            callback.SafeInvoke();
            return result;
        }

        public static IDisposable SubscribeOnce(this Signal signal, Action action) {
            return signal.Subscribe(() => {
                action.SafeInvoke();
                signal.Unsubscribe(action);
            });
        }

        public static IDisposable SubscribeOnce<T>(this Signal<T> signal, Action<T> action) {
            return signal.Subscribe(arg => {
                action.SafeInvoke(arg);
                signal.Unsubscribe(action);
            });
        }
    }
}