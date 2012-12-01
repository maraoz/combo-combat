using UnityEngine;
using System.Collections;

public class GameConstants {

    public static string gameVersion = "alpha-1.0";
    public static int MAGE_GROUP = 0;
    public static int FIREBALL_GROUP = 1;
    public static int HEART_GROUP = 2;

    //tags
    public static string MAGE_TAG = "Mage";
    public static string HEART_TAG = "Heart";

    // layers

    public static int LAYER_UNCLICKABLE = 8;
    public static int LAYER_MASK_UNCLICKABLE = 1 << LAYER_UNCLICKABLE;

    public static int Invert(int mask) {
        return ~(mask);
    }

}
