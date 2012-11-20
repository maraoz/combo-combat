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

    void OnPlayerConnected(NetworkPlayer player) {
        Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Debug.Log("Player disconnected: " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

}