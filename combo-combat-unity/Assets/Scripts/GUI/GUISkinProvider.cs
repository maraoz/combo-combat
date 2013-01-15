using UnityEngine;
using System.Collections;

public class GUISkinProvider : PersistentSingleton {

    public GUISkin guiSkin;

    private static GUISkinProvider instance;

    override internal void Awake() {
        base.Awake();
        instance = this;
    }

    public static GUISkin GetSkin() {
        if (instance == null) {
            instance = GameObject.Find("GUISkinProvider").GetComponent<GUISkinProvider>();
        }
        return instance.guiSkin;
    }

}
