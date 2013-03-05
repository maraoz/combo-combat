using UnityEngine;
using System.Collections;

public class DeathrayController : MonoBehaviour {

    public GameObject effect;
    public float secondsUntilExhaust = 3.0f;
    public float damage = 50;
    public float rayLength = 100;
    public float rayEffectWidth = 0.5f;
    public float rayWarningWidth = 0.2f;
    public Color rayWarningColor = Color.red;
    public AudioClip[] explosionSounds;
    public AudioClip warningSound;
    private AudioSource explosionAudio;
    private AudioSource warningAudio;

    private float secondsPast;
    private LineRenderer lineRenderer;
    private Vector3 origin;
    private Vector3 end;

    private MageLifeController caster;
    private GameObject spawned;


    void Awake() {
        this.enabled = false;
        secondsPast = 0.0f;

        // setup audio
        if (audio == null) {
            explosionAudio = gameObject.AddComponent<AudioSource>();
            warningAudio = gameObject.AddComponent<AudioSource>();
        }
        warningAudio.clip = warningSound;
        warningAudio.Play();


        // setup line
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(rayWarningColor, rayWarningColor);
        lineRenderer.SetWidth(rayWarningWidth, rayEffectWidth);
        lineRenderer.SetVertexCount(2);
        origin = transform.position;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        end = transform.position + forward * rayLength;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, end);
    }

    public void SetCaster(MageLifeController mage) {
        caster = mage;
    }

    public MageLifeController GetCaster() {
        return caster;
    }

    void Update() {
        secondsPast += Time.deltaTime;
        if (secondsPast >= secondsUntilExhaust) {
            lineRenderer.SetVertexCount(0);
            Destroy(spawned);
            if (!explosionAudio.isPlaying) {
                Destroy(gameObject);
            }
        }
    }

    internal void ActivateEffects(SpellNature nature) {
        this.enabled = true;
        Color primaryColor = nature.GetPrimaryColor();
        lineRenderer.SetColors(primaryColor, primaryColor);
        lineRenderer.SetWidth(rayEffectWidth, rayEffectWidth);
        lineRenderer.SetVertexCount(0);
        
        spawned = Instantiate(effect, transform.position+transform.forward*0.1f, Quaternion.Euler(new Vector3(0,0,90))) as GameObject;
        spawned.transform.LookAt(transform);
        Vector3 euler = spawned.transform.eulerAngles;
        euler.x += 180;
        spawned.transform.eulerAngles = euler;


        if (Network.isServer) {
            RaycastHit[] hits = Physics.SphereCastAll(origin, rayEffectWidth, transform.forward, rayLength);
            foreach (RaycastHit hit in hits) {
                Collider other = hit.collider;
                if (other.tag == GameConstants.TAG_MAGE) {
                    MageLifeController mageLife = other.gameObject.GetComponent<MageLifeController>();
                    mageLife.DoDamage(damage, caster);
                }
            }
        }
        if (Network.isClient) {
            explosionAudio.clip = explosionSounds[(int) (Random.value * explosionSounds.Length)];
            explosionAudio.Play();
        }
    }

}