using UnityEngine;
using System.Collections;

public class NetworkInformationText : PersistentSingleton {

    private Rect textPosition;

    override internal void Awake() {
        base.Awake();
    }

    void OnGUI() {
        textPosition = new Rect(10, 5, 500, 20);
        if (Network.isServer) {
            GUI.Label(textPosition, "Running as a dedicated server");
        } else {
            GUI.Label(textPosition, "Combo Combat version " + GameConstants.GAME_VERSION);
        }
    }
}