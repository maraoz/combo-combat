using UnityEngine;
using System.Collections;

public class SpawnMageScript : MonoBehaviour {

    public GameObject playerPrefab;

    void Start() {
        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);
        if (Network.isClient) {
            string username = GameObject.Find("PlayerConnectionHandler").GetComponent<PlayerConnectionHandler>().GetUsername();
            SpawnMage(username);
        }
    }

    [RPC]
    void SpawnMage(string username) {
        if (networkView.Server("SpawnMage", username)) {
            GameObject mageObject = Network.Instantiate(playerPrefab, transform.position, Quaternion.identity, GameConstants.MAGE_GROUP) as GameObject;
            MageLifeController mage = mageObject.GetComponent<MageLifeController>();
            mage.SetUsername(username);
            mage.SetSpawnPosition(transform);
        }
    }
}