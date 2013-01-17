using UnityEngine;
using System.Collections;

public class TextureInspector : MonoBehaviour {

    void Start() {
        Resources.UnloadUnusedAssets();

        Texture2D[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture2D)) as Texture2D[];
        Debug.Log(textures.Length);
        foreach (Texture t in textures) {
            Debug.Log(t);
        }

    }

    void Update() {
        Resources.UnloadUnusedAssets();
    }
}
