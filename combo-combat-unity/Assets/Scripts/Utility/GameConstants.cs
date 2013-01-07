using UnityEngine;
using System.Collections;

public class GameConstants {

    public static string gameVersion = "alpha-1.7.5";
    public static int MAGE_GROUP = 0;
    public static int FIREBALL_GROUP = 1;
    public static int HEART_GROUP = 2;
    public static int GRENADE_GROUP = 3;

    // player prefs
    public static string PREFS_USERNAME = "username";

    //tags
    public static string MAGE_TAG = "Mage";
    public static string HEART_TAG = "Heart";
    public static string WALL_TAG = "Wall";
    public static string GRENADE_TAG = "Grenade";

    // layers
    public static int LAYER_CLICKABLE = 8;
    public static int LAYER_MASK_CLICKABLE = 1 << LAYER_CLICKABLE;

    public static int Invert(int mask) {
        return ~(mask);
    }

    // GUI
    public static int SERVER_WIN_ID = 0;
    public static int CLIENT_WIN_ID = 1;
    public static int TOOLTIP_WIN_ID = 2;
    public static int CHAT_WIN_ID = 3;

}
