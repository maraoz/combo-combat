using UnityEngine;
using System.Collections;

public class ProyectileForm : SpellForm {

    public GameObject proyectile;

    private Vector3 target;

    public override void DoCastForm(SpellNature nature) {
        if (Network.isServer) {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            Vector3 spawnPosition = transform.position + (1.5f * forward) + (1f * Vector3.up) + (0.0f * right);
            GameObject casted = Network.Instantiate(proyectile, spawnPosition, transform.rotation, GameConstants.GROUP_FIREBALL) as GameObject;
            Network.RemoveRPCsInGroup(GameConstants.GROUP_FIREBALL);
            casted.GetComponent<ProyectileController>().SetCaster(GetComponent<MageLifeController>());
            casted.GetComponent<ProyectileController>().SetNature(nature);
        }
    }

    public override void OnFinishCasting() {
        target = Vector3.zero;
    }

    public override void OnFinishPerforming() {
        if (target != Vector3.zero && Network.isServer) {
            Mage mage = GetMage();
            mage.LookAt(mage.transform.position, target);
            PlanCastProyectile();
        }
    }

    [RPC]
    void PlanCastProyectile() {
        networkView.ClientsUnbuffered("PlanCastProyectile");
        GetSpell().PlanCast();
    }

    public override void OnClickDown(Vector3 position) {
        target = position;
        OnFinishPerforming();
    }

}
