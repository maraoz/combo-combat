using UnityEngine;
using System.Collections;

public class CastFireballContinously : MonoBehaviour {

    void Update () {
        GetComponent<Mage>().CastFireball(transform.position + 5 * transform.TransformDirection(Vector3.forward));
    }
}
