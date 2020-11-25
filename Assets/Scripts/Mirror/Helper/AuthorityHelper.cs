using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorityHelper : NetworkBehaviour
{
    IObjectAuthority myNID;
    public IObjectAuthority MyNID
    {
        get
        {
            if (myNID == null)
                myNID = GetComponent<IObjectAuthority>();
            return myNID;
        }
    }

    [Command]
    public void CmdRemoveClientAuthority(NetworkIdentity targetNI)
    {
        if (targetNI == null)
            return;

        Debug.Log($"Server] before Release {targetNI.netId}, Req:{connectionToClient} | OwnBy:{targetNI?.connectionToClient ?? null}");

        if (connectionToClient == targetNI.connectionToClient)
            targetNI.RemoveClientAuthority();
        else
            Debug.LogWarning($"{targetNI.netId} is owned by connectionId:{targetNI?.connectionToClient ?? null}");
    }

    [Command]
    public void CmdRequestAuthority(NetworkIdentity targetNI)
    {
        if (targetNI == null)
            return;

        Debug.Log($"Server] before TakeOver {targetNI.netId}, Req:{connectionToClient} | OwnBy:{targetNI?.connectionToClient ?? null}");

        // targetNI.connectionToClient means OwnedByNone
        if (targetNI.connectionToClient == null)
            targetNI.AssignClientAuthority(connectionToClient);
        else if (targetNI.connectionToClient == connectionToClient)
        {
            Debug.Log($"this is Already owned by Requester");
        }
        else
        {
            Debug.LogWarning($"{targetNI.netId} is owned by connectionId:{targetNI?.connectionToClient ?? null}");
        }
    }
}
