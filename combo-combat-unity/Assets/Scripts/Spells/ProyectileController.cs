using UnityEngine;
using System.Collections;

public class ProyectileController : MonoBehaviour {

    public GameObject explosion;
    public float secondsUntilExhaust = 3.0f;
    public float damage = 10;
    public float speed = .2f;
    public float knockbackMagnitude = 20f;

    private float secondsPast;
    private MageLifeController caster;

    public AudioClip[] explosionSounds;
    public AudioClip[] flyingSounds;

    void Awake() {
        if (audio == null) {
            gameObject.AddComponent<AudioSource>();
        }
        audio.clip = flyingSounds[(int) (Random.value * flyingSounds.Length)];
        audio.Play();
        secondsPast = 0.0f;
    }

    public void SetCaster(MageLifeController mage) {
        caster = mage;
    }

    internal void SetNature(SpellNature nature) {
        return;
    }

    public MageLifeController GetCaster() {
        return caster;
    }

    void Update() {
        transform.position += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
        secondsPast += Time.deltaTime;
        if (secondsPast >= secondsUntilExhaust) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        Collider other = collision.collider;
        if (other.tag == GameConstants.TAG_HEART) {
            return;
        }
        if (Network.isServer) {
            if (other.tag == GameConstants.TAG_MAGE) {
                Mage mage = other.gameObject.GetComponent<Mage>();
                MageLifeController mageLife = other.gameObject.GetComponent<MageLifeController>();
                mageLife.DoDamage(damage, caster);
                Vector3 normal = collision.contacts[0].normal;
                mage.ApplyKnockback(mage.transform.position, normal * -knockbackMagnitude);
            }
            CollideDestroy(transform.position);
        }
    }

    [RPC]
    void CollideDestroy(Vector3 currentPosition) {
        networkView.ClientsUnbuffered("CollideDestroy", currentPosition);
        GameObject created = GameObject.Instantiate(explosion, currentPosition, Quaternion.identity) as GameObject;
        created.AddComponent<AudioSource>();
        created.audio.clip = explosionSounds[(int) (Random.value * explosionSounds.Length)];
        created.audio.Play();
        Destroy(gameObject);
    }

}