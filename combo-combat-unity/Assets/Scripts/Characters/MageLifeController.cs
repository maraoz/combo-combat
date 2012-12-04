using UnityEngine;
using System.Collections;

public class MageLifeController : MonoBehaviour {

    public Texture backgroundTexture;
    public Texture foregroundTexture;
    public Texture frameTexture;
    public int healthBarLength = 100;
    public int healthBarHeight = 10;
    public int maxLife = 100;

    private string username;
    private float life;
    private int level;

    void Awake() {
        RestartLife();
        if (networkView.isMine) {
            string u = (GameObject.Find("PlayerConnectionHandler") as GameObject).GetComponent<PlayerConnectionHandler>().GetUsername();
            networkView.RPC("SetUsername", RPCMode.AllBuffered, u);
        }
    }

    public void RestartLife() {
        life = maxLife;
        level = 0;
    }

    void LevelUp() {
        level += 1;
    }

    public void DoDamage(float damage, MageLifeController source) {
        if (life > 0) {
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
    }

    void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.5f);
        int lifePercent = (int) ((life * healthBarLength) / maxLife);

        Rect frameRect = new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, healthBarLength, healthBarHeight);
        GUI.DrawTexture(frameRect, backgroundTexture, ScaleMode.StretchToFill, true, 0);
        GUI.DrawTexture(new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, lifePercent, healthBarHeight), foregroundTexture, ScaleMode.StretchToFill, true, 0);
        GUI.DrawTexture(frameRect, frameTexture, ScaleMode.StretchToFill, true, 0);
        GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
        centeredStyle.alignment = TextAnchor.UpperCenter;
        GUI.Label(new Rect(pos.x - healthBarLength*2, Screen.height - pos.y - 25, healthBarLength*4, 50), username + " (" + level+")", centeredStyle);
    }

    [RPC]
    void SetUsername(string u) {
        this.username = u;
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
