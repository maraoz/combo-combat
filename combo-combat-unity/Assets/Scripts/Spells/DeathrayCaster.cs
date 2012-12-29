using UnityEngine;
using System.Collections;

public class DeathrayCaster : SpellCaster {

    public GameObject ray;

    private Vector3 target;
    private GameObject casted;

    public override KeyCode GetHotkey() {
        return Hotkeys.DEATHRAY_HOTKEY;
    }

    public override void DoCastSpell() {
        casted.GetComponent<DeathrayController>().ActivateDamage();
    }

    public override void OnFinishCasting() {
        target = Vector3.zero;
        casted = null;
    }

    public override void OnFinishPerforming() {
        if (target != Vector3.zero) {
            GetMage().LookAt(transform.position, target);
            if (Network.isServer) {
                CreateDeathrayObject();
            }
            PlanCastDeathray();
        }
    }

    [RPC]
    void CreateDeathrayObject() {
        networkView.ClientsUnbuffered("CreateDeathrayObject");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 spawnPosition = transform.position + (0.1f * forward) + (0.1f * Vector3.up) + (0.3f * right);
        casted = Instantiate(ray, spawnPosition, transform.rotation) as GameObject;
        casted.GetComponent<DeathrayController>().SetCaster(GetComponent<MageLifeController>());
    }

    [RPC]
    void PlanCastDeathray() {
        networkView.ClientsUnbuffered("PlanCastDeathray");
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
