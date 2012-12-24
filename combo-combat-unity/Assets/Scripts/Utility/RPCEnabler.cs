using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// NetworkView extensions for type-safe RPCs
public static class RPCEnabler {

    /**
     * Invokes the RPC in other clients if object is mine. Returns true if invoked in other clients.
     */
    public static bool Clients(this NetworkView networkView, string routineName, params object[] parameters) {

        if (Network.isServer) {
            networkView.RPC(routineName, RPCMode.Others, parameters);
        }
        return !Network.isServer;

    }

    /**
     * Invokes RPC in server. Returns true if invoked in server
     */
    public static bool Server(this NetworkView networkView, string routineName, params object[] parameters) {
        if (!Network.isServer)
            networkView.RPC(routineName, RPCMode.Server, parameters);
        return Network.isServer;
    }


}