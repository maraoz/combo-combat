using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// NetworkView extensions for type-safe RPCs
public static class RPCEnabler {


    // internal
    private static bool InternalClients(this NetworkView networkView, string routineName, bool buffered, params object[] parameters) {
        if (Network.isServer) {
            networkView.RPC(routineName, buffered ? RPCMode.OthersBuffered : RPCMode.Others, parameters);
        }
        return !Network.isServer;
    }


    /**
     * Invokes the RPC in other clients if object is mine. Returns true if invoked in other clients.
     */
    public static bool Clients(this NetworkView networkView, string routineName, params object[] parameters) {
        return InternalClients(networkView, routineName, true, parameters);
    }

    /**
     * Same as above but unbuffered
     */
    public static bool ClientsUnbuffered(this NetworkView networkView, string routineName, params object[] parameters) {
        return InternalClients(networkView, routineName, false, parameters);
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