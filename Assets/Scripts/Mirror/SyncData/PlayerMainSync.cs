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
    [SerializeField] Transform tra;

    [Header("Debug")]
    [SerializeField] Collider col;

    public AuthorityHelper authorityHelper;
    public InstantiateHelper instantiateHelper;

    [SerializeField] NetworkTransform nTra;
    private void OnDisable()
    {
        nTra.enabled = false;
        nTra.clientAuthority = false;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);

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

    public void RequestAuthority(GameObject go)
    {
        if (go == null)
            return;

        var ni = go.GetComponent<NetworkIdentity>();
        authorityHelper.CmdRequestAuthority(ni);
    }

    public void RemoveClientAuthority(GameObject go)
    {
        if (go == null)
            return;

        var ni = go.GetComponent<NetworkIdentity>();
        authorityHelper.CmdRemoveClientAuthority(ni);
    }

    #region Client2Server
    [Command]
    public void CmdTryUpdatePlayerSyncData(string uid)
    {
        usrID = uid;
    }
    #endregion

    //public void OnCreateCharacter(NetworkConnection conn, PlayerCreationNetworkMessage message)
    //{
    //    // playerPrefab is the one assigned in the inspector in Network Manager
    //    GameObject gameobject = MainService.Instance.poolManager.Instantiate(
    //        MainService.Instance.networkManager.playerPrefab, 
    //        transform.position, 
    //        Quaternion.identity
    //    ); //Instantiate(playerPrefab);

    //    //// Apply data from the message however appropriate for your game
    //    //Player player = gameobject.GetComponent<Player>();

    //    // call this to use this gameobject as the primary controller
    //    NetworkServer.AddPlayerForConnection(conn, gameobject);
    //}
}

public struct PlayerCreationNetworkMessage: NetworkMessage
{
    public CreationNetworkMessage baseCreateion;
}
