using UnityEngine;
using System.Collections;

public class HeartSpawner : MonoBehaviour {

    public GameObject heartPrefab;
    public float spawnDeltaTime = 1.0f;
    public int heartsPerMage = 3;
    public int range = 10;

    private float timeSpent = 0f;

    void Awake() {
        if (!Network.isServer) {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        timeSpent += Time.deltaTime;
        if (timeSpent >= spawnDeltaTime) {
            timeSpent -= spawnDeltaTime;
            int players = Network.connections.Length;
            if (GameObject.FindGameObjectsWithTag(GameConstants.TAG_HEART).Length < heartsPerMage * players+2) {
                Vector3 pos = new Vector3(transform.position.x + Random.Range(-range, range),
                    transform.position.y,
                    transform.position.z + Random.Range(-range, range));
                Network.Instantiate(heartPrefab, pos, Quaternion.identity, GameConstants.GROUP_HEART);
            }
        }
    }
}
