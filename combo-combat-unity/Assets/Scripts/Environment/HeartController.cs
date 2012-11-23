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



    internal void Destroy() {
        Debug.Log("Destroy heart");
        Network.Destroy(gameObject);
        Network.RemoveRPCs(networkView.viewID);
        if (effect != null) {
            GameObject.Instantiate(effect, transform.position, Quaternion.identity);
        }
    }
}
