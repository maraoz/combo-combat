using UnityEngine;
using System.Collections;

public class MageLifeController : MonoBehaviour {

    public int initialLife = 100;

    private float life;

    void Awake () {
        life = initialLife;
    }
    
    public void DoDamage(float damage) {
        life -= damage;
        if (life < 0) {
            life = 0;
        }
        Debug.Log("Mage received " + damage + "damage and life is now " + life);
    }


}
