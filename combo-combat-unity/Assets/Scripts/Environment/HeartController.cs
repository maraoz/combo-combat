using UnityEngine;
using System.Collections;

public class HeartController : MonoBehaviour {

    public GameObject effect;
    public float rotationAmount = 5.0f;
    public float healing = 15;

    void Update() {
        transform.Rotate(new Vector3(0, rotationAmount, 0));
    }


    void OnNetworkInstantiate(NetworkMessageInfo info) {
        //Network.RemoveRPCs(info.sender, GameConstants.HEART_GROUP);
    }



    internal void DestroyedBy(GameObject mage) {
        Network.Destroy(gameObject);
        Network.RemoveRPCs(networkView.viewID);
    }

    public void SpawnEffect(GameObject mage) {
        if (effect != null) {
            GameObject instance = GameObject.Instantiate(effect, mage.transform.position, Quaternion.identity) as GameObject;
            instance.transform.parent = mage.transform;
        }
    }

}
