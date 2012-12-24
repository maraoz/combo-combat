using UnityEngine;
using System.Collections;

public class UsernameHolder : MonoBehaviour {

    private string username;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void SetUsername(string username) {
        this.username = username;
    }

    public string GetUsername() {
        return username;
    }
}
