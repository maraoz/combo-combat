using UnityEngine;
using System.Collections;

public class MageLifeController : MonoBehaviour {


    // match
    private MatchDirector matchDirector;
    public int lifeCount = 3;
    private int livesLeft;
    private bool isFreeMode;

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
    private int kills;

    // death
    private bool isDying = false;
    private MessageSystem messages;
    private Vector3 spawnPosition;
    public float deathTime = 15.0f;
    private float deathTimeSpent = 0f;
    public AudioClip[] deathCries;
    public AudioClip[] damageSounds;
    public float damageSoundProbability = 0.5f;
    public float dieHeight = -50f;


    void Awake() {
        RestartLife();
        matchDirector = FindObjectOfType(typeof(MatchDirector)) as MatchDirector;
        livesLeft = lifeCount;
        mage = GetComponent<Mage>();
        messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
    }

    [RPC]
    public void SetFreeMode(bool freeMode) {
        networkView.Clients("SetFreeMode", freeMode);
        isFreeMode = freeMode;
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

    public int GetLivesLeft() {
        return livesLeft;
    }

    void Update() {
        if (Network.isServer) {
            if (transform.position.y < dieHeight && !isDying) {
                Debug.Log("fallen");
                DoDamage(100, null);
            }
        }
        if (isDying && livesLeft > 0) {
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
    }

    void AddKill() {
        kills += 1;
    }

    // only called on server
    public void DoDamage(float damage, MageLifeController source) {
        if (life > 0) {
            life -= damage;
            if (life <= 0) {
                life = 0;
                if (source != null && source != this) {
                    source.AddKill();
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
        // all
        if (!isFreeMode) {
            livesLeft -= 1;
        }
        if (deathCries.Length > 0) {
            mage.audio.clip = deathCries[(int) (Random.value * deathCries.Length)];
            mage.audio.Play();
        }
        mage.OnDied();
        isDying = true;
        if (mage.IsMine()) {
            // owner player
            Camera.main.GetComponent<IsometricCamera>().SetGrayscale(true);
        }
        if (!networkView.ClientsUnbuffered("DoDie")) {
            // server
            if (livesLeft == 0) {
                matchDirector.OnPlayerLost();
            }
            if (livesLeft > 0) {
                messages.AddSystemMessageTo(mage.GetPlayer(), "You died. Please wait " + deathTime + " seconds to respawn.");
            } else {
                messages.AddSystemMessageTo(mage.GetPlayer(), "You have no more lives left.");
            }
            if (!isFreeMode) {
                messages.AddSystemMessageTo(mage.GetPlayer(), livesLeft + " " + (livesLeft == 1 ? "life" : "lives") + " left.");
            }

        }
    }

    void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.5f);
        int lifePercent = (int) (healthBarLength * GetLifePercentage());
        Rect frameRect = new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, healthBarLength, healthBarHeight);
        GUI.DrawTexture(new Rect(pos.x - healthBarLength / 2, Screen.height - pos.y, lifePercent, healthBarHeight), foregroundTexture, ScaleMode.StretchToFill, true, 0);
        GUI.DrawTexture(frameRect, backgroundTexture, ScaleMode.StretchToFill, true, 0);
        //GUI.DrawTexture(frameRect, frameTexture, ScaleMode.StretchToFill, true, 0);
        GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.normal.textColor = Color.black;
        GUI.Label(new Rect(pos.x - healthBarLength * 2, Screen.height - pos.y - 20, healthBarLength * 4, 50), username + " (" + kills + ")", centeredStyle);

    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            float sendLife = life;
            int sendKills = kills;
            stream.Serialize(ref sendLife);
            stream.Serialize(ref sendKills);
        } else {
            float rcvLife = 0;
            int rcvKills = 0;
            stream.Serialize(ref rcvLife);
            stream.Serialize(ref rcvKills);
            if (rcvLife > 0 && rcvLife < life && damageSounds.Length > 0 && Random.value < damageSoundProbability) {
                mage.audio.clip = damageSounds[(int) (Random.value * damageSounds.Length)];
                mage.audio.Play();
            }
            life = rcvLife;
            kills = rcvKills;
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
            InitializeMyMage();
        } else if (Network.isServer) { // server's version of all mages
            name += " Serverside";
            GetComponent<CharacterSimpleAnimation>().enabled = true;
        } else { // client's remote copies of server.
            name += " Remote";
            GetComponent<CharacterSimpleAnimation>().enabled = true;
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
