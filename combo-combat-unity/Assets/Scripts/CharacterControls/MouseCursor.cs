using UnityEngine;
using System.Collections;

public class MouseCursor : PersistentSingleton {

    public static Texture2D defaultCursor;
    public static Texture2D attackCursor;

    public Texture2D _defaultCursor;
    public Texture2D _attackCursor;


    public float timeToStart = 1.0f;
    private float timePast;

    override internal void Awake() {
        base.Awake();
        defaultCursor = _defaultCursor;
        attackCursor = _attackCursor;
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
    public static void SetAttackCursor() {
        SetCursor(attackCursor);
    }

    public static void SetMoveCursor() {
        SetCursor(defaultCursor);
    }

    private static void SetCursor(Texture2D cursor) {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

}
