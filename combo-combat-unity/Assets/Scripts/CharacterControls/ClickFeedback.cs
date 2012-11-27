using UnityEngine;
using System.Collections;

public class ClickFeedback : MonoBehaviour {

    public float duration = 0.5f;
    public float cycleDuration = 0.5f;
    public float speed = 0.2f;
    public string asd;

    private float timeSpent;
    private int cycle;
    private Vector3 initialPosition;

    void Start () {
        timeSpent = 0;
        cycle = 1;
        initialPosition = transform.position;
    }
    
    void Update () {

        transform.position += Vector3.down * speed * Time.deltaTime; 

        timeSpent += Time.deltaTime;
        if (timeSpent >= cycle * cycleDuration) {
            cycle += 1;
            transform.position = initialPosition;
        }
        if (duration > 0 && timeSpent >= duration) {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color) {
        light.color = color;
    }
}
