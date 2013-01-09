using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistentSingleton : MonoBehaviour {

    private static HashSet<System.Type> taken = new HashSet<System.Type>();

    internal virtual void Awake() {
        System.Type type = GetType();
        if (taken.Contains(type)) {
            enabled = false;
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            taken.Add(type);
        }
    }

}
