using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

    public GameObject fireball; 

    public void Cast(Vector3 target) {
        GameObject.Instantiate(fireball, transform.position, Quaternion.identity);
    }
}
