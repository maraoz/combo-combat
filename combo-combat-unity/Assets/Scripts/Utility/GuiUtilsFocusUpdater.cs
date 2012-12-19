using UnityEngine;
using System.Collections;

public class GuiUtilsFocusUpdater : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void OnGUI() {
        // on the first OnGUI call, reset the guiFocused boolean
        if (Event.current.type == EventType.Layout) {
            GuiUtils.SetGUIFocused(false);
        }
    }

}
