using UnityEngine;
using System.Collections;

public class FireballCaster : SpellCaster {

    public GameObject fireball;

    void Start() {

    }

    public bool PlanCast(Vector3 v) {
        bool casting = this.PlanCast();
        if (casting) {
            transform.LookAt(v);
        }
        return casting;
    }

    public override void DoCastSpell() {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 spawnPosition = transform.position + (1.5f * forward) + (1f * Vector3.up) + (0.5f * right);
        GameObject casted = Network.Instantiate(fireball, spawnPosition, transform.rotation, GameConstants.FIREBALL_GROUP) as GameObject;
        casted.GetComponent<FireballController>().SetCaster(GetComponent<MageLifeController>());
    }

    override public void OnFinishCasting() {
        // nothing for now
    }
}
