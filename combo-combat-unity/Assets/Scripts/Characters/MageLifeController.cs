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
    private Transform spawnPosition;
    public float deathTime = 15.0f;
    private float deathTimeSpent = 0f;

    public float dieDuration = 1f;
    private float dieStart;


    void Awake() {
        RestartLife();
        mage = GetComponent<Mage>();
        messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
    }

    void Update() {
        if (isDying) {
            if (dieStart == 0f) {
                dieStart = Time.time;
            } else {
                if (Time.time - dieStart < dieDuration) {
                    Vector3 euler = transform.rotation.eulerAngles;
                    euler.x -= 90.0f * (Time.deltaTime / dieDuration);
                    transform.rotation = Quaternion.Euler(euler);
                } else {
                    dieStart = 0f;
                }
            }

            deathTimeSpent += Time.deltaTime;
            if (deathTimeSpent >= deathTime) {
                Respawn();
            }
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
            if (Network.isServer) {
                life -= damage;
                if (life <= 0) {
                    life = 0;
                    DoDie();
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

    void Respawn() {
        deathTimeSpent = 0f;
        transform.position = spawnPosition.position;
        transform.rotation = Quaternion.identity;
        Camera.main.GetComponent<IsometricCamera>().SetGrayscale(false);
        mage.OnRespawn();
        RestartLife();
    }

    public void DoDie() {
        if (!isDying) {
            mage.OnDied();
            messages.AddSystemMessage("You died. Please wait " + deathTime + " seconds to respawn.", false);
            Camera.main.GetComponent<IsometricCamera>().SetGrayscale(true);
            isDying = true;
        }
    }

    public bool IsDying() {
        return isDying;
    }

    public void SetSpawnPosition(Transform spawner) {
        spawnPosition = spawner;
    }

    public float GetLifePercentage() {
        return (life / maxLife);
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

    [RPC]
    public void SetUsername(string u) {
        networkView.Clients("SetUsername", u);
        if (u == UsernameHolder.MyUsername()) {
            Camera.main.SendMessage("SetTarget", transform);
            GameObject.Find("Hud").GetComponent<HudController>().SetMageOwner(gameObject);
        } else {
            name += "Remote";
            GetComponent<UserInputController>().enabled = false;
            GetComponent<CharacterSimpleAnimation>().enabled = true;
            GetComponent<NetworkSyncAnimation>().enabled = false;
            GetComponent<NetworkInterpolatedTransform>().enabled = false;
        }
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
