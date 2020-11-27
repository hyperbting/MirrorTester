using Mirror;
using System;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public partial class NetworkObjectBase : NetworkBehaviour, IPooledObject, IObjectAuthority
{
    [SyncVar(hook = nameof(ClientSideRemoteInit))]
    [SerializeField] CreationNetworkMessage message;

    #region Properties
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
    #endregion

    #region IObjectAuthority
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
    #endregion

    public string GetPooledObjectTypeID()
    {
        return netIdentity.assetId.ToString();
    }

    //When CreationNetworkMessage message set, trigger this method
    public void ClientSideRemoteInit(CreationNetworkMessage last, CreationNetworkMessage cnm)
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
}

public interface IObjectAuthority
{
    NetworkIdentity NID { get; }
    Action OnAuthorityObtained { get; set; }
    Action OnAuthorityReleased { get; set; }
}

public interface IPooledObject
{
    string GetPooledObjectTypeID();
}