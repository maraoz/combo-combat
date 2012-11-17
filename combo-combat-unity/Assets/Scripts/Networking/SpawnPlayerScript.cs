using UnityEngine;
using System.Collections;

public class SpawnPlayerScript : MonoBehaviour {

    public Transform playerPrefab;

    void Start() {
        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);
        if (Network.isClient) {
            Network.Instantiate(playerPrefab, transform.position, Quaternion.identity, GameConstants.CHARACTER_GROUP);
        }
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Server destroying player");
        Network.RemoveRPCs(player, 0);
        Network.DestroyPlayerObjects(player);
    }

}
