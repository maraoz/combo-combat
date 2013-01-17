using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallCaster : SpellCaster {

    public GameObject brick;
    public float wallBrickLength = 1.0f;
    public float wallDrawResolution = 1.5f;
    public float wallMaxLength = 20.0f;
    public Color wallDrawColor = Color.yellow;

    private LineRenderer lineRenderer;
    private List<Vector3> points;
    private float wallLength;

    internal override void Awake() {
        base.Awake();
        points = new List<Vector3>();
        wallLength = 0;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.SetVertexCount(0);
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(wallDrawColor, wallDrawColor);
        lineRenderer.SetWidth(0.1F, 0.1F);
    }

    public override void DoCastSpell() {
        if (!Network.isServer) {
            return;
        }
        int count = points.Count;
        int totalBricks = 0;
        for (int i = 0; i < count - 1; i++) {
            Vector3 current = points[i];
            Vector3 next = points[i + 1];

            float dist = Vector3.Distance(current, next);
            if (dist <= 0) {
                continue;
            }

            int bricksNeeded = (int) (dist / wallBrickLength);
            if (bricksNeeded == 0) bricksNeeded = 1;
            float adaptedBrickLenght = dist / bricksNeeded;
            for (int j = 0; j < bricksNeeded; j++) {
                Vector3 currentBrick = Vector3.Lerp(current, next, adaptedBrickLenght * j / dist);
                Vector3 nextBrick = Vector3.Lerp(current, next, adaptedBrickLenght * (j + 1) / dist);
                Vector3 middleBrick = (currentBrick + nextBrick) / 2;

                SpawnBrick(middleBrick, next, adaptedBrickLenght);
            }
            totalBricks += bricksNeeded;
        }
    }

    [RPC]
    public void SpawnBrick(Vector3 middleBrick, Vector3 next, float adaptedBrickLenght) {
        networkView.ClientsUnbuffered("SpawnBrick", middleBrick, next, adaptedBrickLenght);
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

    public override KeyCode GetHotkey() {
        return Hotkeys.WALL_HOTKEY;
    }

    public override void OnFinishCasting() {
        points.Clear();
    }

    public override void OnFinishPerforming() {
        if (points.Count > 1 && Network.isServer) {
            PlanCastWall();
        } else {
            points.Clear(); // TODO: use this points to cast wall with single RPC :)
        }
        WallPerformCleanup();
    }

    [RPC]
    void WallPerformCleanup() {
        networkView.ClientsUnbuffered("WallPerformCleanup");
        lineRenderer.SetVertexCount(0);
        wallLength = 0;
    }

    [RPC]
    void PlanCastWall() {
        networkView.ClientsUnbuffered("PlanCastWall");
        PlanCast();
    }

    public override void OnClickDown(Vector3 position) {
        // nothing for now
    }

    public override void OnClickDragged(Vector3 position) {
        if (this.IsCasting()) {
            return;
        }
        int count = points.Count;
        if (count == 0) {
            points.Add(position);
        } else {
            RenderWallLineFeedback();
            float distanceToLast = Vector3.Distance(points[count - 1], position);
            if (distanceToLast > wallDrawResolution) {
                if (wallLength + distanceToLast <= wallMaxLength) {
                    points.Add(position);
                    wallLength += distanceToLast;
                    if (wallLength + wallDrawResolution >= wallMaxLength) {
                        OnFinishPerforming();
                    }
                } else {
                    CompleteWall(position, wallMaxLength - wallLength);
                    OnFinishPerforming();
                }
            }
        }
    }

    public override void OnClickUp(Vector3 position) {
        CompleteWall(position, wallMaxLength - wallLength);
        OnFinishPerforming();
    }

    private void RenderWallLineFeedback() {
        int count = points.Count;
        lineRenderer.SetVertexCount(count + 1);
        int i = 0;
        while (i < count) {
            Vector3 point = new Vector3(points[i].x, points[0].y + 0.1f, points[i].z);
            lineRenderer.SetPosition(i, point);
            i++;
        }
        lineRenderer.SetPosition(i, transform.position + Vector3.up * 1.1f + transform.forward * 1.1f);
    }

    private void CompleteWall(Vector3 point, float remainingDistance) {
        int count = points.Count;
        if (count > 1) {
            Vector3 last = points[count - 1];
            float realDistance = Vector3.Distance(last, point);
            Vector3 correction = Vector3.Lerp(last, point, remainingDistance / realDistance);
            points.Add(correction);
        }
    }

    public override void OnInputFocusLost() {
        OnFinishPerforming();
    }

}
