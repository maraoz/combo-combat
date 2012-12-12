using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallCaster : SpellCaster {

    public GameObject brick;
    public float wallBrickLength = 1.0f;
    private List<Vector3> points;

    public bool PlanCast(List<Vector3> pts) {
        this.points = new List<Vector3>(pts);
        return PlanCast();
    }

    public override void DoCastSpell() {
        int count = points.Count;
        int totalBricks = 0;
        for (int i = 0; i < count - 1; i++) {
            Vector3 current = points[i];
            Vector3 next = points[i + 1];

            float dist = Vector3.Distance(current, next);

            int bricksNeeded = (int) (dist / wallBrickLength);
            if (bricksNeeded == 0) bricksNeeded = 1;
            float adaptedBrickLenght = dist / bricksNeeded;
            for (int j = 0; j < bricksNeeded; j++) {
                Vector3 currentBrick = Vector3.Lerp(current, next, adaptedBrickLenght * j / dist);
                Vector3 nextBrick = Vector3.Lerp(current, next, adaptedBrickLenght * (j + 1) / dist);
                Vector3 middleBrick = (currentBrick + nextBrick) / 2;

                networkView.RPC("SpawnBrick", RPCMode.All, middleBrick, next, adaptedBrickLenght);
            }
            totalBricks += bricksNeeded;
        }
        Debug.Log(totalBricks);
    }

    [RPC]
    public void SpawnBrick(Vector3 middleBrick, Vector3 next, float adaptedBrickLenght) {
        GameObject piece = GameObject.Instantiate(brick, middleBrick, Quaternion.identity) as GameObject;
        piece.transform.LookAt(next);
        Vector3 euler = piece.transform.eulerAngles;
        euler.y += 90;
        piece.transform.eulerAngles = euler;
        Vector3 scale = piece.transform.localScale;
        scale.x *= adaptedBrickLenght;
        piece.transform.localScale = scale;
        Vector3 position = piece.transform.position;
        position.y += 1.5f;
        piece.transform.position = position;
    }

    public override void OnFinishCasting() {
        points = null;
    }

}
