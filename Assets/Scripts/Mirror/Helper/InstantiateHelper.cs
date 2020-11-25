using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateHelper : NetworkBehaviour
{
    public GameObject networkedBase;

    [Command]
    public void CmdAddOne(CreationNetworkMessage cnm)
    {
        GameObject nBase = MainService.Instance.poolManager.Instantiate(networkedBase, transform.position, Quaternion.identity); //GameObject nBase = Instantiate(networkedBase);

        switch (cnm.networkSyncType)
        {
            case NetworkSyncType.PlayerObject: // this player will have Authority
                NetworkServer.Spawn(nBase, connectionToClient);
                break;
            case NetworkSyncType.SceneObject:
                NetworkServer.Spawn(nBase);
                break;
            case NetworkSyncType.Player:
                break;
            default:
                break;
        }

        var nob = nBase.GetComponent<NetworkObjectBase>();
        nob.ServerInit(cnm);
    }

    [Command]
    public void CmdRemoveOne(GameObject go)
    {
        if (go.GetComponent<IPooledObject>() == null)
        {
            Debug.LogWarning($"{go.name} is not PooledObject");
            return;
        }

        MainService.Instance.poolManager.Destroy(go);
    }
}
