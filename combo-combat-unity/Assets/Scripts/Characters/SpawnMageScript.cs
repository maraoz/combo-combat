using UnityEngine;
using System.Collections;

public class SpawnMageScript : MonoBehaviour {

    public GameObject playerPrefab;
    public MessageSystem messages;

    void Start() {
        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);
        if (Network.isClient) {
            string username = GameObject.Find("UsernameHolder").GetComponent<UsernameHolder>().GetUsername();
            SpawnMage(username);
        }
    }

    [RPC]
    void SpawnMage(string username) {
        if (networkView.Server("SpawnMage", username)) {
            messages.AddSystemMessage(username + " connected.", true);
            GameObject mageObject = Network.Instantiate(playerPrefab, transform.position, Quaternion.identity, GameConstants.MAGE_GROUP) as GameObject;
            MageLifeController mage = mageObject.GetComponent<MageLifeController>();
            mage.SetUsername(username);
            mage.SetSpawnPosition(transform);
        }
    }

    // called on client
    void OnDisconnectedFromServer() {
        messages.AddSystemMessage("Connection to server lost :( Please refresh page.", false);
    }

    // called on server
    void OnPlayerConnected(NetworkPlayer player) {
        PlaySound();
    }

    // called on server
    void OnPlayerDisconnected(NetworkPlayer player) {
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