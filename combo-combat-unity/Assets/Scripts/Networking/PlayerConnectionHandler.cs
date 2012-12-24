using UnityEngine;
using System.Collections;

public class PlayerConnectionHandler : MonoBehaviour {

    private MessageSystem messages;
    private string username;

    public void SetUsername(string username) {
        this.username = username;
    }

    public string GetUsername() {
        return username;
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
        username = "";
    }

    void LoadMessages() {
        if (messages == null) messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
    }

    // called on client
    void OnDisconnectedFromServer() {
        LoadMessages();
        messages.AddSystemMessage("Connection to server lost :(", false);
    }

    // called on server
    void OnPlayerConnected(NetworkPlayer player) {
        LoadMessages();
        messages.AddSystemMessage("Player connected from " + player.ipAddress + ":" + player.port, true);
        PlaySound();
    }

    // called on server
    void OnPlayerDisconnected(NetworkPlayer player) {
        LoadMessages();
        messages.AddSystemMessage("Player disconnected: " + player, true);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    [RPC]
    void PlaySound() {
        networkView.Others("PlaySound");
        audio.Play();
    }

}
