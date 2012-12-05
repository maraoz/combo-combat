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

    public void SetMoveCursor() {
        currentCursor = defaultCursor;
    }

    // TODO: do this like this, with hardware cursors
    private void SetCursor(Texture2D cursor) {
        Debug.Log("changing state");
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

}
