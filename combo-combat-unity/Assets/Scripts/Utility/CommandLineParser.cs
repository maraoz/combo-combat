using UnityEngine;
using System.Collections;
using System;

public class CommandLineParser : MonoBehaviour {


    private static int maxPlayersAllowed = 32;
    private static bool isFreeMode = true;
    private static bool isBatchMode = false;



    void Awake() {

        DontDestroyOnLoad(gameObject);
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++) {
            if (args[i].Equals("-n")) {
                maxPlayersAllowed = Int32.Parse(args[i + 1]);
                break;
            }
            if (args[i].Equals("-batchmode")) {
                isBatchMode = true;
            }
            if (args[i].Equals("-matchmode")) {
                isFreeMode = false;
            }
        }

    }

    internal static int GetMaxPlayersAllowed() {
        return maxPlayersAllowed;
    }

    internal static bool IsBatchMode() {
        return isBatchMode;
    }

    internal static bool IsFreeMode() {
        return isFreeMode;
    }
}
