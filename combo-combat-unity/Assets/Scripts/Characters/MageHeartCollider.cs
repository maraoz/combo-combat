using UnityEngine;
using System.Collections;

public class MageHeartCollider : MonoBehaviour {

    public GameObject effect;

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.collider.tag == GameConstants.HEART_TAG) {
            if (effect != null) {
                GameObject instance = GameObject.Instantiate(effect, transform.position, Quaternion.identity) as GameObject;
                instance.transform.parent = transform;
            }
            if (Network.isServer) {
                HeartController heart = hit.collider.gameObject.GetComponent<HeartController>();
                heart.Destroy();
                GetComponent<MageLifeController>().DoDamage(-heart.healing, null);
            }
        }
    }

}
