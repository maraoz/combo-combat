using UnityEngine;
using System.Collections;

public class MageLifeController : MonoBehaviour {

    // lifebar
    public Texture backgroundTexture;
    public Texture foregroundTexture;
    public Texture frameTexture;
    public int healthBarLength = 100;
    public int healthBarHeight = 10;
    public int maxLife = 100;

    // mage info (shouldnt be here, but hey)
    private Mage mage;
    private string username;
    private float life;
    private int level;

    // death
    private bool isDying = false;
    private MessageSystem messages;
    private Vector3 spawnPosition;
    public float deathTime = 15.0f;
    private float deathTimeSpent = 0f;


    void Awake() {
        RestartLife();
        mage = GetComponent<Mage>();
        messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
    }

    public string GetUsername() {
        return username;
    }

    public bool IsDying() {
        return isDying;
    }

    [RPC]
    public void SetSpawnPosition(Vector3 position) {
        networkView.Clients("SetSpawnPosition", position);
        spawnPosition = position;
    }

    public float GetLifePercentage() {
        return (life / maxLife);
    }

    void Update() {
        if (isDying) {
            deathTimeSpent += Time.deltaTime;
            if (deathTimeSpent >= deathTime) {
                Respawn();
            }
        }

    }

    void Respawn() {
        RestartLife();
        deathTimeSpent = 0f;
        transform.position = spawnPosition;
        transform.rotation = Quaternion.identity;
        mage.OnRespawn();
        if (mage.IsMine()) {
            Camera.main.GetComponent<IsometricCamera>().SetGrayscale(false);
        }
    }


    public void RestartLife() {
        isDying = false;
        life = maxLife;
        level = 0;
    }

    void LevelUp() {
        level += 1;
    }

    public void DoDamage(float damage, MageLifeController source) {
        if (life > 0) {
            life -= damage;
            if (life <= 0) {
                life = 0;
                if (source != null) {
                    source.LevelUp();
                }
                DoDie();
            }
            if (life > maxLife) {
                life = maxLife;
            }
        }
    }

    [RPC]
    void DoDie() {
        if (!networkView.ClientsUnbuffered("DoDie")) {
            // server
            messages.AddSystemMessageTo(mage.GetPlayer(), "You died. Please wait " + deathTime + " seconds to respawn.");
        }
        // all
        mage.OnDied();
        isDying = true;
        if (mage.IsMine()) {
            // owner player
            Camera.main.GetComponent<IsometricCamera>().SetGrayscale(true);
        }
    }

    void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.5f);
        int lifePercent = (int) (healthBarLength * GetLifePercentage());
        Rect frameRect = new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, healthBarLength, healthBarHeight);
        GUI.DrawTexture(frameRect, backgroundTexture, ScaleMode.StretchToFill, true, 0);
        GUI.DrawTexture(new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, lifePercent, healthBarHeight), foregroundTexture, ScaleMode.StretchToFill, true, 0);
        GUI.DrawTexture(frameRect, frameTexture, ScaleMode.StretchToFill, true, 0);
        GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.normal.textColor = Color.black;
        GUI.Label(new Rect(pos.x - healthBarLength * 2, Screen.height - pos.y - 20, healthBarLength * 4, 50), username + " (" + level + ")", centeredStyle);
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

    [RPC]
    public void SetUsername(string u) {
        networkView.Clients("SetUsername", u);
        if (u == UsernameHolder.MyUsername()) {
            mage.TakeOwnership();
        }
        if (mage.IsMine()) { // client's own mage
            GetComponent<CharacterSimpleAnimation>().enabled = true;
            GetComponent<UserInputController>().enabled = true;
            InitializeMyMage();
        } else if (Network.isServer) { // server's version of all mages
            name += " Serverside";
            GetComponent<CharacterSimpleAnimation>().enabled = true;
            GetComponent<UserInputController>().enabled = false;
        } else { // client's remote copies of server.
            name += " Remote";
            GetComponent<CharacterSimpleAnimation>().enabled = true;
            GetComponent<UserInputController>().enabled = false;
        }
        GetComponent<Mage>().enabled = true;
        GetComponent<MageLifeController>().enabled = true;
        this.username = u;
    }

    private void InitializeMyMage() {
        Camera.main.GetComponent<IsometricCamera>().SetTarget(transform);
        GameObject.Find("Hud").GetComponent<HudController>().SetMageOwner(gameObject);
    }



}
