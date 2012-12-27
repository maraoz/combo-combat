using UnityEngine;
using System.Collections;

public class SpawnMageScript : MonoBehaviour {

    public GameObject playerPrefab;
    public MessageSystem messages;

    void Start() {
        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);
        if (Network.isClient) {
            string username = UsernameHolder.MyUsername();
            networkView.RPC("SpawnMage", RPCMode.Server, username); // need NetworkMessageInfo
        }
    }

    [RPC]
    void SpawnMage(string username, NetworkMessageInfo info) {
        if (networkView.Server("SpawnMage", username)) {
            messages.AddSystemMessageBroadcast(username + " connected.");
            GameObject mageObject = Network.Instantiate(playerPrefab, transform.position, Quaternion.identity, GameConstants.MAGE_GROUP) as GameObject;
            MageLifeController life = mageObject.GetComponent<MageLifeController>();
            Mage mage = mageObject.GetComponent<Mage>();
            UserInputController input = mageObject.GetComponent<UserInputController>();
            input.ServerInit();
            life.SetUsername(username);
            mage.SetPlayer(info.sender);
            life.SetSpawnPosition(transform);
        }
    }

    // called on client
    void OnDisconnectedFromServer() {
        messages.AddSystemMessageSelf("Connection to server lost :( Please refresh page.");
    }

    // called on server
    void OnPlayerConnected(NetworkPlayer player) {
        PlaySound();
    }

    // called on server
    void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    [RPC]
    void PlaySound() {
        networkView.Clients("PlaySound");
        audio.Play();
    }
}