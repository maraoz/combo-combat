using UnityEngine;
using System.Collections;

public class MageLifeController : MonoBehaviour {

    public Texture backgroundTexture;
    public Texture foregroundTexture;
    public int healthBarLength = 100;
    public int healthBarHeight = 10;
    public int maxLife = 100;

    private float life;
    private int level;

    void Awake() {
        RestartLife();
    }

    public void RestartLife() {
        life = maxLife;
        level = 0;
    }

    void LevelUp() {
        level += 1;
    }

    public void DoDamage(float damage, MageLifeController source) {
        if (networkView.isMine) {
            life -= damage;
            if (life <= 0) {
                life = 0;
                GetComponent<Mage>().DoDie();
            }
            if (life > maxLife) {
                life = maxLife;
            }
        }
        if (life <= damage && source != null) {
            source.LevelUp();
        }
    }

    void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 3);
        int lifePercent = (int) ((life * healthBarLength) / maxLife);

        GUI.DrawTexture(new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, healthBarLength, healthBarHeight), backgroundTexture, ScaleMode.StretchToFill, true, 0);
        GUI.DrawTexture(new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, lifePercent, healthBarHeight), foregroundTexture, ScaleMode.StretchToFill, true, 0);
        GUI.Label(new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y+10, healthBarLength, 50), "Level " + level);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            float sendLife = life;
            int sendLevel = level;
            stream.Serialize(ref sendLife);
            stream.Serialize(ref sendLevel);
        } else {
            float rcvLife = 0;
            int rcvLevel = 0;
            stream.Serialize(ref rcvLife);
            stream.Serialize(ref rcvLevel);
            life = rcvLife;
            level = rcvLevel;
        }

    }



}
