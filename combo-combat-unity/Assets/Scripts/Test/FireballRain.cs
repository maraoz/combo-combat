using UnityEngine;
using System.Collections;

public class FireballRain : MonoBehaviour {

    public GameObject fireballPrefab;
    public float timeBetweenSpawn = 1.0f;
    public float range = 10;

    private float accumTime;

    // Use this for initialization
    void Start () {
        accumTime = 0.0f;
    }
    
    // Update is called once per frame
    void Update () {
        accumTime += Time.deltaTime;
        if (accumTime >= timeBetweenSpawn) {
            accumTime -= timeBetweenSpawn;
            GameObject.Instantiate(fireballPrefab,
                new Vector3(transform.position.x + Random.Range(-range, range),
                    transform.position.y,
                    transform.position.z + Random.Range(-range, range)),
                Quaternion.Euler(90.0f,0.0f,0.0f));
        }
    }
}
