using UnityEngine;
using System.Collections;

public class NetworkInformationText : MonoBehaviour {

    private Rect textPosition;

    void Start() {
        DontDestroyOnLoad(this);
    }

    void OnGUI() {
        textPosition = new Rect(10, 5, 500, 20);
        if (Network.isServer)
            GUI.Label(textPosition, "Running as a dedicated server");
        if (Network.isClient) {
            GUI.Label(textPosition, "Combo Combat version " + GameConstants.gameVersion);
        }
    }
}