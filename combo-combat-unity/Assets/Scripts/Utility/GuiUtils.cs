using UnityEngine;
using System.Collections;

public class GuiUtils {

    private static bool isGUIFocused = false;

    public static void SetGUIFocused(bool value) {
        isGUIFocused = value;
    }

    public static bool IsGUIFocused() {
        return isGUIFocused;
    }
}
