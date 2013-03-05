using UnityEngine;
using System.Collections;

public class RayForm : SpellForm {

    public GameObject ray;

    private Vector3 target;
    private GameObject casted;

    public override void OnClickDown(Vector3 position) {
        target = position;
        OnFinishPerforming();
    }

    public override void DoCastForm(SpellNature nature) {
        casted.GetComponent<DeathrayController>().ActivateEffects(nature);
    }

    [RPC]
    void CreateRayObject() {
        networkView.ClientsUnbuffered("CreateRayObject");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 spawnPosition = transform.position + (0.5f * forward) + (0.1f * Vector3.up) + (0.3f * right);
        casted = Instantiate(ray, spawnPosition, transform.rotation) as GameObject;
        casted.GetComponent<DeathrayController>().SetCaster(GetComponent<MageLifeController>());
    }

    [RPC]
    void PlanCastRay() {
        networkView.ClientsUnbuffered("PlanCastRay");
        GetSpell().PlanCast();
    }

    public override void OnFinishPerforming() {
        if (target != Vector3.zero) {
            GetMage().LookAt(transform.position, target);
            if (Network.isServer) {
                CreateRayObject();
            }
            PlanCastRay();
        }
    }

    public override void OnFinishCasting() {
        target = Vector3.zero;
        casted = null;
    }

}
