using System;
using TMPro;
using UnityEngine.Events;

namespace TextMeshPro.Utils {
    public static class TmpUtils {
        public static void SetOnValueChangedAction(this TMP_Dropdown dropdown, Action<int> action) {
            dropdown.onValueChanged.RemoveAllListeners();
            if (action != null)
                dropdown.onValueChanged.AddListener(new UnityAction<int>(action));
        }
    }
}