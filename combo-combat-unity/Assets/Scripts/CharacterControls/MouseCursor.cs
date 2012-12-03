using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour {

    public Texture2D defaultCursor;
    public Texture2D attackCursor;
    public static MouseCursor main;

    private Texture2D currentCursor; 

    void Awake() {
        Screen.showCursor = false;
        DontDestroyOnLoad(gameObject);
        main = this;
        currentCursor = defaultCursor;
    }

    void OnGUI() {
        GUI.depth = 0;
        GUI.DrawTexture(new Rect(Input.mousePosition.x,
                               Screen.height - Input.mousePosition.y,
                               currentCursor.width,
                               currentCursor.height),
                         currentCursor);
    }

    public void SetAttackCursor() {
        currentCursor = attackCursor;
    }

}
