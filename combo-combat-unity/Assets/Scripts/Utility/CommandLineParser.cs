using UnityEngine;
using System.Collections;
using System;

public class CommandLineParser : PersistentSingleton {

    public int defaultMaxPlayersAllowedFree = 32;
    public int defaultMaxPlayersAllowedMatch = 4;
    public string defaultServerName = "Default ";
    public string defaultServerComment = "Argentina {0} mode game.";
    public int defaultServerPort = 34200;
    public bool defaultIsFreeMode = true;
    public bool defaultIsDynamicPort = true;

    private static int maxPlayersAllowed = -1;
    private static bool isFreeMode;
    private static bool isBatchMode;
    private static string serverName;
    private static string serverComment;
    private static int serverPort = -1;
    private static bool dynamicPort;

    override internal void Awake() {
        base.Awake();

        isBatchMode = false;
        isFreeMode = defaultIsFreeMode;
        serverPort = defaultServerPort;
        dynamicPort = defaultIsDynamicPort;


        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++) {
            if (args[i].Equals("-n")) {
                maxPlayersAllowed = Int32.Parse(args[i + 1]);
            } else if (args[i].Equals("-batchmode")) {
                isBatchMode = true;
            } else if (args[i].Equals("-matchmode")) {
                isFreeMode = false;
            } else if (args[i].Equals("-name")) {
                serverName = args[i + 1];
            } else if (args[i].Equals("-comment")) {
                serverComment = args[i + 1];
            } else if (args[i].Equals("-p")) {
                serverPort = Int32.Parse(args[i + 1]);
            } else if (args[i].Equals("-dynport")) {
                dynamicPort = true;
            }
        }

        if (maxPlayersAllowed == -1) {
            maxPlayersAllowed = isFreeMode ? defaultMaxPlayersAllowedFree : defaultMaxPlayersAllowedMatch;
        }
        if (serverName == null) {
            serverName = defaultServerName + (isFreeMode ? "Free Mode" : "Match Mode");
        }
        if (serverComment == null) {
            serverComment = String.Format(defaultServerComment, isFreeMode ? "free" : "match");
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

    internal static string GetServerName() {
        return serverName;
    }

    internal static string GetServerComment() {
        return serverComment;
    }

    internal static int GetServerPort() {
        return serverPort;
    }

    internal static bool IsDynamicPort() {
        return dynamicPort;
    }
}
