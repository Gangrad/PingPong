using System.Reflection;

namespace CsExtensions {
    public static class ReflectionUtils {
        public const BindingFlags GetFieldFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField;
        public const BindingFlags GetMethodFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod;

        public static T GetFieldValue<T>(this object obj, string fieldName) where T : class {
            var targetObjectClassType = obj.GetType();
            var field = targetObjectClassType.GetField(fieldName, GetFieldFlag);
            return field == null ? null : field.GetValue(obj) as T;
        }

        public static void InvokeMethod(this object obj, string methodName, params object[] parameters) {
            var targetObjectClassType = obj.GetType();
            var method = targetObjectClassType.GetMethod(methodName, GetMethodFlag);
            if (method != null)
                method.Invoke(obj, parameters);
        }
    }
}