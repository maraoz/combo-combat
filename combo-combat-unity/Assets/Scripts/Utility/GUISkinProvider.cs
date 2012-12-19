using UnityEngine;
using System.Collections;

public class GUISkinProvider : MonoBehaviour {

    public GUISkin guiSkin;

    private static GUISkinProvider instance;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public static GUISkin GetSkin() {
        if (instance == null) {
            instance = GameObject.Find("GUISkinProvider").GetComponent<GUISkinProvider>();
        }
        return instance.guiSkin;
    }

}
