using UnityEngine;
using System.Collections;

public class NetworkInformationText : MonoBehaviour {

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
    }
    
    void OnGUI() {
        if (Network.isServer)
            GUI.Label(new Rect(20, Screen.height - 20, 200, 20), "Running as a dedicated server");
        if (Network.isClient) {
            GUI.Label(new Rect(20, Screen.height - 20, 200, 20), "Combo Combat v "+GameConstants.gameVersion);
        }
    }
}
