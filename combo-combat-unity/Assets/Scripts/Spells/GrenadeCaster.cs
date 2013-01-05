using UnityEngine;
using System.Collections;

public class GrenadeCaster : SpellCaster {

    public GameObject grenade;

    private Vector3 target;

    public override KeyCode GetHotkey() {
        return Hotkeys.GRENADE_HOTKEY;
    }

    public override void DoCastSpell() {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 spawnPosition = transform.position + (2f * forward) + (1f * Vector3.up) + (0.5f * right);
        GameObject casted = Instantiate(grenade, spawnPosition, transform.rotation) as GameObject;
        casted.GetComponent<GrenadeController>().SetCaster(GetComponent<MageLifeController>());
        casted.GetComponent<GrenadeController>().AddForce(forward);
    }

    public override void OnFinishCasting() {
        target = Vector3.zero;
    }

    public override void OnFinishPerforming() {
        if (target != Vector3.zero && Network.isServer) {
            Mage mage = GetMage();
            mage.LookAt(mage.transform.position, target);
            PlanCastGrenade();
        }
    }

    [RPC]
    void PlanCastGrenade() {
        networkView.ClientsUnbuffered("PlanCastGrenade");
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
