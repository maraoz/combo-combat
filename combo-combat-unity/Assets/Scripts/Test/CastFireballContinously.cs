using UnityEngine;
using System.Collections;

public class CastFireballContinously : MonoBehaviour {

    void Update() {
        FireballCaster caster = GetComponent<FireballCaster>();
        caster.OnClickDown(transform.position + 5 * transform.TransformDirection(Vector3.forward));
    }
}
