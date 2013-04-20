using UnityEngine;
using System.Collections;

public class UsernameHolder : PersistentSingleton {

    private string username = "Server";

    override internal void Awake() {
        base.Awake();
    }

    private void _SetUsername(string username) {
        this.username = username;
    }

    private string _GetUsername() {
        return username;
    }

    public static void SetUsername(string username) {
        GameObject uh = GameObject.Find("UsernameHolder");
        if (uh == null) {
            return;
        }
        uh.GetComponent<UsernameHolder>()._SetUsername(username);
    }

    public static string GetUsername() {
        GameObject uh = GameObject.Find("UsernameHolder");
        if (uh == null) {
            return "NoUsername";
        }
        return uh.GetComponent<UsernameHolder>()._GetUsername();
    }
}
