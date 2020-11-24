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
        GameObject nBase = Instantiate(networkedBase);

        switch (cnm.networkSyncType)
        {
            case NetworkSyncType.PlayerObject:
                NetworkServer.Spawn(nBase, connectionToClient);
                break;
            case NetworkSyncType.SceneObject:
                NetworkServer.Spawn(nBase);
                break;
            case NetworkSyncType.Player:
            default:
                break;
        }

        var nob = nBase.GetComponent<NetworkObjectBase>();
        nob.ServerInit(cnm);
    }
}
