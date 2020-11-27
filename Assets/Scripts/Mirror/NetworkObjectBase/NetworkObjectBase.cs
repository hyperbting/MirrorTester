using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkObjectBase : Mirror.NetworkBehaviour
{
    public Transform meshRoot;

    readonly string ScrTag = "NetworkObjectBase";

    public void Reset()
    {
        if (netTransform != null)
            netTransform.clientAuthority = false;

        // destroy all children
        foreach (Transform child in transform)
        {
            Debug.Log($"{child.gameObject.name}");
        }
    }

    void Start()
    {
        Debug.Log($"{ScrTag}.Start hasAuthority:{ hasAuthority }");
    }

    // setter @server
    public void ServerSideInit(CreationNetworkMessage pcnMsg)
    {
        if (!isServer) return;

        Debug.Log($"NetworkObjectBase.ServerInit {pcnMsg.ToString()}");

        message = pcnMsg;
        Debug.Log($"NetworkObjectBase.ServerInit Trigger ClientSideRemoteInit");

        Debug.Log($"NetworkObjectBase.ServerInit End {message.ToString()}");
    }
}