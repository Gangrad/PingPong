using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Unity.Utils {
    public static class ToggleUtils {
        public static void SetOnChangedAction(this Toggle toggle, Action<bool> action) {
            toggle.onValueChanged.RemoveAllListeners();
            if (action != null)
                toggle.onValueChanged.AddListener(new UnityAction<bool>(action));
        }
    }
}