using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MirrorNetworkManager : NetworkManager, INetworkManagerCallbacks
{
    #region EventLayer
    public Action OnStartServerEvent { get; set; }          //S1,H1
    public Action OnStartHostEvent { get; set; }            //H2
    public Action OnServerConnectEvent { get; set; }        //S2,H3
    public Action OnStartClientEvent { get; set; }          //C1,H4
    public Action<NetworkConnection> OnClientConnectEvent { get; set; }        //C2,H5
    public Action OnServerSceneChangedEvent { get; set; }   //S3,H6
    public Action OnServerReadyEvent { get; set; }          //S4,H7
    public Action OnServerAddPlayerEvent { get; set; }      //S5,H8
    public Action OnClientChangeSceneEvent { get; set; }    //C3,H9
    public Action OnClientSceneChangedEvent { get; set; }   //C4,H10

    public Action OnStopHostEvent { get; set; }             //H1
    public Action OnServerDisconnectEvent { get; set; }     //S1,H2
    public Action OnStopClientEvent { get; set; }           //C1,H3
    public Action OnStopServerEvent { get; set; }           //S2,H4
    public Action OnClientDisconnectEvent { get; set; }     //C2
    #endregion EventLayer

    public void ListPlayerInRoom()
    {

    }

    #region Mirror Callback
    #region ServerOnly
    //01 OnStartServer
    public override void OnStartServer()
    {
        base.OnStartServer();

        OnStartServerEvent?.Invoke();
    }

    //03 OnServerConnect
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        OnServerConnectEvent?.Invoke();
    }

    //06 OnServerSceneChanged
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        OnServerSceneChangedEvent?.Invoke();
    }

    //07 OnServerReady
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadyEvent?.Invoke();
    }

    //08 OnServerAddPlayer 
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        OnServerAddPlayerEvent?.Invoke();
    }

    //03x OnServerDisconnect
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        OnServerDisconnectEvent?.Invoke();
    }

    //01x OnStopServer
    public override void OnStopServer()
    {
        base.OnStopServer();

        OnStopServerEvent.Invoke();
    }
    #endregion

    #region ClientOnly
    //04 OnStartClient
    public override void OnStartClient()
    {
        base.OnStartClient();

        OnStartClientEvent?.Invoke();
    }

    //05 OnClientConnect
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        //Debug.Log("OnClientConnect");
        //// you can send the message here, or wherever else you want
        //var characterMessage = new CreationNetworkMessage
        //{
        //    networkSyncType = NetworkSyncType.Player
        //};

        //conn.Send(characterMessage);

        OnClientConnectEvent?.Invoke(conn);
    }

    //09 OnClientChangeScene
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);

        OnClientChangeSceneEvent?.Invoke();
    }

    //10 OnClientChangeScene
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);

        OnClientSceneChangedEvent?.Invoke();
    }

    //05x OnStopClient
    public override void OnStopClient()
    {
        base.OnStopClient();

        OnStopClientEvent?.Invoke();
    }

    //04x OnClientDisconnect
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        OnClientDisconnectEvent?.Invoke();
    }
    #endregion

    #region Host is combination of server and client
    //01 OnStartServer shared with SERVER

    //02 OnStartHost HostOnly
    public override void OnStartHost()
    {
        base.OnStartHost();

        OnStartHostEvent?.Invoke();
    }

    //03 OnServerConnect shared with SERVER
    //04 OnStartClient shared with CLIENT
    //05 OnClientConnect shared with CLIENT
    //06 OnServerSceneChanged shared with SERVER
    //07 OnServerReady shared with SERVER
    //08 OnServerAddPlayer shared with SERVER
    //09 OnClientChangeScene shared with CLIENT
    //10 OnClientChangeScene shared with CLIENT

    //02x OnStopHost HostOnly
    public override void OnStopHost()
    {
        base.OnStopHost();

        OnStopHostEvent?.Invoke();
    }

    //03x OnServerDisconnect shared with SERVER
    //04x OnStopClient shared with CLIENT
    //01x OnStopServer shared with SERVER
    #endregion
    #endregion
}

public interface INetworkManagerCallbacks
{
    Action OnStartServerEvent { get; set; }
    Action OnStartHostEvent { get; set; }
    Action OnServerConnectEvent { get; set; }
    Action OnStartClientEvent { get; set; }
    Action<NetworkConnection> OnClientConnectEvent { get; set; }
    Action OnServerSceneChangedEvent { get; set; }
    Action OnServerReadyEvent { get; set; }
    Action OnServerAddPlayerEvent { get; set; }
    Action OnClientChangeSceneEvent { get; set; }
    Action OnClientSceneChangedEvent { get; set; }

    Action OnStopHostEvent { get; set; }
    Action OnServerDisconnectEvent { get; set; }
    Action OnStopClientEvent { get; set; }
    Action OnStopServerEvent { get; set; }
    Action OnClientDisconnectEvent { get; set; }
}