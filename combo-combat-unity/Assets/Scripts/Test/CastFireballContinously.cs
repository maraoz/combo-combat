using UnityEngine;
using System.Collections;

public class CastFireballContinously : MonoBehaviour {

    void Update() {
        Spell fireball = GetComponent<Spell>();
        fireball.form.OnClickDown(transform.position + 5 * transform.TransformDirection(Vector3.forward));
    }
}
