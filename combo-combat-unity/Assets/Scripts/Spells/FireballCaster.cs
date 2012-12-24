using UnityEngine;
using System.Collections;

public class FireballCaster : SpellCaster {

    public GameObject fireball;

    private Vector3 target;

    public override KeyCode GetHotkey() {
        return Hotkeys.FIREBALL_HOTKEY;
    }

    public override void DoCastSpell() {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 spawnPosition = transform.position + (1.5f * forward) + (1f * Vector3.up) + (0.5f * right);
        GameObject casted = Instantiate(fireball, spawnPosition, transform.rotation) as GameObject;
        casted.GetComponent<FireballController>().SetCaster(GetComponent<MageLifeController>());
    }

    public override void OnFinishCasting() {
    }

    public override void OnFinishPerforming() {
        if (target != Vector3.zero) {
            Mage mage = GetMage();
            mage.LookAt(mage.transform.position, target);
            PlanCastFireball();
        }
    }

    [RPC]
    void PlanCastFireball() {
        networkView.Clients("PlanCastFireball");
        PlanCast();
    }

    public override void OnClickDown(Vector3 position) {
        target = position;
        OnFinishPerforming();
    }

    public override void OnClickDragged(Vector3 position) {
        // nothing for now
    }

    public override void OnClickUp(Vector3 position) {
        // nothing for now
    }

    public override void OnInputFocusLost() {
        // nothing for now
    }

}
