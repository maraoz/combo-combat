using UnityEngine;
using System.Collections;

public class MageLifeController : MonoBehaviour {

    public Texture backgroundTexture;
    public Texture foregroundTexture;
    public int healthBarLength = 100;
    public int healthBarHeight = 10;
    public int maxLife = 100;

    private float life;

    void Awake() {
        life = maxLife;
    }

    public void DoDamage(float damage) {
        if (networkView.isMine) {
            life -= damage;
            if (life < 0) {
                life = 0;
                SendMessage("DoDie");
            }
            if (life > maxLife) {
                life = maxLife;
            }
        }
    }

    void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 3);
        int lifePercent = (int) ((life * healthBarLength) / maxLife);

        GUI.DrawTexture(new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, healthBarLength, healthBarHeight), backgroundTexture, ScaleMode.StretchToFill, true, 0);
        GUI.DrawTexture(new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, lifePercent, healthBarHeight), foregroundTexture, ScaleMode.StretchToFill, true, 0);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            float send = life;
            stream.Serialize(ref send);
        } else {
            float rcv = 0;
            stream.Serialize(ref rcv);
            life = rcv;
        }

    }



}
