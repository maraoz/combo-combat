using UnityEngine;
using System.Collections;

public class SpawnMageScript : MonoBehaviour {

    public Transform playerPrefab;

    void Start() {
        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);
        if (Network.isClient) {
            Network.Instantiate(playerPrefab, transform.position, Quaternion.identity, GameConstants.CHARACTER_GROUP);
        }
    }
}
