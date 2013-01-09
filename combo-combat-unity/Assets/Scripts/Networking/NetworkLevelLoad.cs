using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkLevelLoad : MonoBehaviour {

    void OnConnectedToServer() {
        Network.SetSendingEnabled(0, false);
        Network.isMessageQueueRunning = false;
        LoadArenaLevel();
    }

    void OnServerInitialized() {
        LoadArenaLevel();
        Debug.Log("Server initialized and ready");
    }

    void LoadArenaLevel() {
        Network.SetLevelPrefix(GameConstants.LEVEL_PREFIX_MATCH);
        Application.LoadLevel(GameConstants.LEVEL_MATCH);
    }

}