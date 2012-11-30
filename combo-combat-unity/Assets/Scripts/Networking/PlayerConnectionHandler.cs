using UnityEngine;
using System.Collections;

public class PlayerConnectionHandler : MonoBehaviour {

    private MessageSystem messages;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void OnDisconnectedFromServer() {
        if (messages == null) messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
        messages.AddSystemMessage("Connection to server lost :(", false);
    }

    void OnPlayerConnected(NetworkPlayer player) {
        if (messages == null) messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
        messages.AddSystemMessage("Player connected from " + player.ipAddress + ":" + player.port, true);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        if (messages == null) messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
        messages.AddSystemMessage("Player disconnected: " + player, true);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }
}
