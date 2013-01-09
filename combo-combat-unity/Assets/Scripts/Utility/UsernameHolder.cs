using UnityEngine;
using System.Collections;

public class UsernameHolder : PersistentSingleton {

    private string username = "Server";

    override internal void Awake() {
        base.Awake();
    }

    public void SetUsername(string username) {
        this.username = username;
    }

    public string GetUsername() {
        return username;
    }

    public static string MyUsername() {
        return GameObject.Find("UsernameHolder").GetComponent<UsernameHolder>().GetUsername();
    }
}
