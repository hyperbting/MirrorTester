using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkObjectBase : NetworkBehaviour, IObjectAuthority
{

    [SyncVar(hook = nameof(InitSyncRemote))]
    [SerializeField] CreationNetworkMessage message;
    public Transform meshRoot;

    readonly string ScrTag = "NetworkObjectBase";

    [SerializeField] NetworkTransform netTransform;
    public NetworkTransform NetTransform
    {
        get
        {
            if (TryGetComponent<NetworkTransform>(out var res))
            {
                return res;
            }

            return gameObject.AddComponent<NetworkTransform>();
        }
    }

    [SerializeField] NetworkTransformChild netTransformChild;
    public NetworkTransformChild NetTransformChild
    {
        get
        {
            if (TryGetComponent<NetworkTransformChild>(out var res))
            {
                return res;
            }

            return gameObject.AddComponent<NetworkTransformChild>();
        }
    }

    public NetworkIdentity NID
    {
        get
        {
            return netIdentity;
        }
    }

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
    public void ServerInit(CreationNetworkMessage pcnMsg)
    {
        if (!isServer) return;

        Debug.Log($"NetworkObjectBase.ServerInit {pcnMsg.ToString()}");

        message = pcnMsg;
        Debug.Log($"NetworkObjectBase.ServerInit Trigger Client InitSyncRemote");

        Debug.Log($"NetworkObjectBase.ServerInit End {message.ToString()}");
    }

    //When CreationNetworkMessage message set, trigger this method
    public void InitSyncRemote(CreationNetworkMessage last, CreationNetworkMessage cnm)
    {
        Debug.Log($"NetworkObjectBase.InitSyncRemote { message }");
        var nt = GetComponent<NetworkTransform>();
        switch (cnm.networkSyncType)
        {
            case NetworkSyncType.Player:

                var pms = GetComponent<PlayerMainSync>();
                if (pms == null)
                    pms = gameObject.AddComponent<PlayerMainSync>();

                nt.clientAuthority = true;
                break;
            case NetworkSyncType.PlayerObject:
                nt.clientAuthority = true;
                break;
            case NetworkSyncType.SceneObject:
                break;
            default:
                break;
        }
    }

    public Action OnAuthorityObtained { get; set; }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        Debug.Log($"I own {NID.netId} {message.ToString()}");
        OnAuthorityObtained?.Invoke();
    }

    public Action OnAuthorityReleased { get; set; }
    public override void OnStopAuthority()
    {
        base.OnStopAuthority();

        Debug.Log($"I lose {NID.netId} {message.ToString()}");
        OnAuthorityReleased?.Invoke();
    }
}

public interface IObjectAuthority
{
    NetworkIdentity NID { get; }
    Action OnAuthorityObtained { get; set; }
    Action OnAuthorityReleased { get; set; }
}