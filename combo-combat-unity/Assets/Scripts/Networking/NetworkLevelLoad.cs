using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkLevelLoad : MonoBehaviour {

    void OnConnectedToServer() {
        Network.SetSendingEnabled(0, false);
        Network.isMessageQueueRunning = false;
        Application.LoadLevel("ComboGame");
    }

    void OnServerInitialized() {
        Application.LoadLevel("ComboGame");
        Debug.Log("Server initialized and ready");
    }
    void OnDisconnectedFromServer() {
        // TODO: Reconnecting...
    }

}
