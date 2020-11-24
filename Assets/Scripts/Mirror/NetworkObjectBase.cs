using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkObjectBase : NetworkBehaviour, IObjectAuthority
{

    [SyncVar(hook = nameof(InitSyncRemote))]
    public CreationNetworkMessage message;
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
        Debug.Log($"NetworkObjectBase.Init {pcnMsg.ToString()}");

        Debug.Log($"NetworkObjectBase.Init Trigger InitSyncRemote");
        message = pcnMsg;

        Debug.Log($"NetworkObjectBase.Init End");
    }

    //When CreationNetworkMessage message set, trigger this method
    public void InitSyncRemote(CreationNetworkMessage old, CreationNetworkMessage newObj)
    {
        Debug.Log($"NetworkObjectBase.InitSyncRemote { newObj }");
        var nt = GetComponent<NetworkTransform>();
        switch (newObj.networkSyncType)
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

        Debug.Log($"I own {NID.netId}");
        OnAuthorityObtained?.Invoke();
    }

    public Action OnAuthorityReleased { get; set; }
    public override void OnStopAuthority()
    {
        base.OnStopAuthority();

        Debug.Log($"I lose {NID.netId}");
        OnAuthorityReleased?.Invoke();
    }
}

public interface IObjectAuthority
{
    NetworkIdentity NID { get; }
    Action OnAuthorityObtained { get; set; }
    Action OnAuthorityReleased { get; set; }
}