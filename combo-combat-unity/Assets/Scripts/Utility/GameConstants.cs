using UnityEngine;
using System.Collections;

public class GameConstants {

    public static string gameVersion = "alpha-1.4.7";
    public static int MAGE_GROUP = 0;
    public static int FIREBALL_GROUP = 1;
    public static int HEART_GROUP = 2;

    // player prefs
    public static string PREFS_USERNAME = "username";

    //tags
    public static string MAGE_TAG = "Mage";
    public static string HEART_TAG = "Heart";

    // layers

    public static int LAYER_CLICKABLE = 8;
    public static int LAYER_MASK_CLICKABLE = 1 << LAYER_CLICKABLE;

    public static int Invert(int mask) {
        return ~(mask);
    }

}
