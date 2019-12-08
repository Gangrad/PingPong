using System;
using CsExtensions;
using UnityEngine;

namespace Unity.Utils {
    public static class PlayerPrefsUtils {
        public static void SetBool(string key, bool value) {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool GetBool(string key, bool defaultValue) {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
        }
        
        public static void SetColor(string keyPrefix, Color color) {
            PlayerPrefs.SetFloat(keyPrefix + "_r", color.r);
            PlayerPrefs.SetFloat(keyPrefix + "_g", color.g);
            PlayerPrefs.SetFloat(keyPrefix + "_b", color.b);
            PlayerPrefs.SetFloat(keyPrefix + "_a", color.a);
        }

        public static Color GetColor(string keyPrefix, Color defaultColor) {
            Color result;
            return TryGetColor(keyPrefix, out result) ? result : defaultColor;
        }
        
        public static bool TryGetColor(string keyPrefix, out Color color) {
            float r, g, b, a;
            if (TryGetFloat(keyPrefix + "_r", out r) &&
                TryGetFloat(keyPrefix + "_g", out g) &&
                TryGetFloat(keyPrefix + "_b", out b) &&
                TryGetFloat(keyPrefix + "_a", out a)) {
                color = new Color(r, g, b, a);
                return true;
            }
            
            color = Color.black;
            return false;
        }

        public static bool TryGetFloat(string key, out float value) {
            if (PlayerPrefs.HasKey(key)) {
                value = PlayerPrefs.GetFloat(key);
                return true;
            }

            value = 0f;
            return false;
        }

        public static void SetEnum<T>(string key, T value) where T : struct, IConvertible {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            PlayerPrefs.SetString(key, value.ToString());
        }

        public static T GetEnum<T>(string key, T defaultValue) where T : struct, IConvertible {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            if (!PlayerPrefs.HasKey(key))
                return defaultValue;
            var stringValue = PlayerPrefs.GetString(key);
            return EnumUtils.TryParse(stringValue, defaultValue);
        }
    }
}