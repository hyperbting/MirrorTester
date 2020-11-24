using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainSync : NetworkBehaviour
{
    [SyncVar]
    public string usrID;

    readonly string ScrTag = "PlayerMainSync";

    [Header("Interactee")]
    public NetworkObjectBase targetNI;

    [Header("Debug")]
    [SerializeField] Transform tra;
    public AuthorityHelper authorityHelper;
    public InstantiateHelper instantiateHelper;

    private void Start()
    {
        if (hasAuthority)
        {
            gameObject.name = string.Format($"HostPlayerToken [{netId}]");

            tra = MainService.Instance.playerManager.localPlayer.transform;

            //assign myself to 
            MainService.Instance.playerManager.localPlayer.NetworkSetup(this);

            Debug.Log($"{ScrTag} {gameObject} track HostPlayer!");
        }
        else
        {
            gameObject.name = string.Format($"NetPlayer {netId} Token");

            // instantiate Player for Other
            var scr = MainService.Instance.playerManager.Instantiate();
            scr.transform.SetParent(transform, false);
            tra = null;
        }
    }

    private void Update()
    {
        if (hasAuthority && tra != null)
        {
            transform.position = tra.position;
            transform.rotation = tra.rotation;
        }
    }

    public void RequestAuthority()
    {
        authorityHelper.CmdRequestAuthority(targetNI?.NID);
    }

    public void RemoveClientAuthority()
    {
        authorityHelper.CmdRemoveClientAuthority(targetNI?.NID);
    }

    #region Client2Server
    [Command]
    public void CmdTryUpdatePlayerSyncData(string uid)
    {
        usrID = uid;
    }
    #endregion
}
