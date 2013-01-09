using UnityEngine;
using System.Collections;

public class GuiUtilsFocusUpdater : PersistentSingleton {

    override internal void Awake() {
        base.Awake();
    }

    void OnGUI() {
        // on the first OnGUI call, reset the guiFocused boolean
        if (Event.current.type == EventType.Layout) {
            GuiUtils.SetGUIFocused(false);
        }
    }

}
