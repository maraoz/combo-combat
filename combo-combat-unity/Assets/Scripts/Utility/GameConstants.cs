using UnityEngine;
using System.Collections;

public class GameConstants {

    // game version
    public static string GAME_VERSION = "alpha-1.9.1";

    // groups
    public static int GROUP_MAGE = 0;
    public static int GROUP_FIREBALL = 1;
    public static int GROUP_HEART = 2;
    public static int GROUP_GRENADE = 3;

    // player prefs
    public static string PREFS_USERNAME = "username";
    public static string PREFS_VOLUME = "volume";

    //tags
    public static string TAG_MAGE = "Mage";
    public static string TAG_HEART = "Heart";
    public static string TAG_WALL = "Wall";
    public static string TAG_GRENADE = "Grenade";

    // layers
    public static int LAYER_CLICKABLE = 8;
    public static int LAYER_MASK_CLICKABLE = 1 << LAYER_CLICKABLE;

    public static int Invert(int mask) {
        return ~(mask);
    }

    // GUI
    public static int WIN_ID_SERVER = 0;
    public static int WIN_ID_CLIENT = 1;
    public static int WIN_ID_TOOLTIP = 2;
    public static int WIN_ID_CHAT = 3;
    public static int WIN_ID_CREDITS = 4;
    public static int WIN_ID_CONFIG = 5;


    // network 
    public static NetworkPlayer NO_PLAYER = new NetworkPlayer();

    // levels
    public static string LEVEL_LOBBY = "Lobby";
    public static string LEVEL_MATCH = "ComboGame";
    public static int LEVEL_PREFIX_LOBBY = 0;
    public static int LEVEL_PREFIX_MATCH = 1;

}
