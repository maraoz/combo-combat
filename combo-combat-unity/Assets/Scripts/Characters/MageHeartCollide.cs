using UnityEngine;
using System.Collections;

public class MageHeartCollide : MonoBehaviour {

    public GameObject effect;

    void Awake() {
        if (!networkView.isMine) {
            Debug.Log("not mine colliding!");
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (!networkView.isMine) {
            Debug.Log("not mine colliding!");
        }
        if (hit.collider.tag == GameConstants.HEART_TAG) {
            Debug.Log("On collider hit heart");
            if (effect != null) {
                GameObject instance = GameObject.Instantiate(effect, transform.position, Quaternion.identity) as GameObject;
                instance.transform.parent = transform;
            }
            if (networkView.isMine) {
                HeartController heart = hit.collider.gameObject.GetComponent<HeartController>();
                heart.Destroy();
                GetComponent<MageLifeController>().DoDamage(-heart.healing, null);
            }
        }
    }

}
