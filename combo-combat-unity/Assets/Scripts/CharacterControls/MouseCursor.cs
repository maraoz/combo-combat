using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour {

    public Texture2D defaultCursor;
    public Texture2D attackCursor;
    public static MouseCursor main;
    public float timeToStart = 1.0f;
    private float timePast;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        main = this;
        timePast = 0f;
    }

    void Update() {
        timePast += Time.deltaTime;
        // horrible hack because Cursor API doesnt work at startup
        if (timePast >= timeToStart) {
            SetCursor(defaultCursor);
            this.enabled = false;
        }
    }
    public void SetAttackCursor() {
        SetCursor(attackCursor);
    }

    public void SetMoveCursor() {
        SetCursor(defaultCursor);
    }

    private void SetCursor(Texture2D cursor) {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

}
