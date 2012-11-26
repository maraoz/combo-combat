using UnityEngine;
using System.Collections;

public class SpawnMageScript : MonoBehaviour {

    public GameObject playerPrefab;

    void Start() {
        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);
        if (Network.isClient) {
            GameObject mageObject = Network.Instantiate(playerPrefab, transform.position, Quaternion.identity, GameConstants.MAGE_GROUP) as GameObject;
            Mage mage = mageObject.GetComponent<Mage>();
            mage.SetSpawnPosition(transform);
        }
    }
}