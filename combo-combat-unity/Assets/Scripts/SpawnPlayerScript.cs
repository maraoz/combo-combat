using UnityEngine;
using System.Collections;

public class SpawnPlayerScript : MonoBehaviour {

    public Transform playerPrefab;

    void Start() {
        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);
        if (Network.isClient)
            Network.Instantiate(playerPrefab, transform.position, Quaternion.identity, 0);
    }

    void OnPlayerDisconnected (NetworkPlayer player)
    {
	    Debug.Log("Server destroying player");
	    Network.RemoveRPCs(player, 0);
	    Network.DestroyPlayerObjects(player);
    }

    void OnGUI() {
        if (Network.isServer)
            GUI.Label(new Rect(20, Screen.height - 50, 200, 20), "Running as a dedicated server");
    }

}
