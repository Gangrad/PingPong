using System;

namespace CsExtensions.Properties {
    public interface IProperty {
    }

    public interface IEventProperty : IProperty {
        IDisposable Subscribe(Action callback);
    }

    public interface IDispatchableProperty : IEventProperty {
        void Dispatch();
    }

    public interface IValueProperty<out T>: IProperty {
        T Value { get; }
        IDisposable Subscribe(Action<T> callback);
    }

    public interface IMutableProperty<T> : IValueProperty<T> {
        new T Value { set; }
    }

    public class MutableProperty<T> : IMutableProperty<T> where T : IEquatable<T> {
        private readonly Signal<T> _onChanged = new Signal<T>();
        private T _value;

        public T Value {
            get { return _value; }
            set {
                if (_value.Equals(value))
                    return;
                _value = value;
                _onChanged.Dispatch(_value);
            }
        }

        public IDisposable Subscribe(Action<T> callback) {
            return _onChanged.Subscribe(callback);
        }
    }

    public static class PropertyUtils {
        public static IDisposable Subscribe<T>(this IValueProperty<T> property, Action callback) {
            return property.Subscribe(value => callback());
        }
    }
}