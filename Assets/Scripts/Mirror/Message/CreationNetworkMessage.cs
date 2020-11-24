using Mirror;

[System.Serializable]
public struct CreationNetworkMessage : NetworkMessage
{
    public NetworkSyncType GetSyncType()
    {
        return networkSyncType;
    }
    
    #region Component
    //public bool useAnimator;
    public NetworkSyncType networkSyncType;
    #endregion

    public override string ToString()
    {
        return UnityEngine.JsonUtility.ToJson(this);
    }

    //// Mirror will automatically implement message that are empty
    //public void Deserialize(NetworkReader reader) { }
    //public void Serialize(NetworkWriter writer) { }
}

public enum NetworkSyncType : byte
{
    None,
    Player,
    PlayerObject,
    SceneObject,

}
public enum NetworkTransformUsage: byte
{
    None,
    NetworkTransform,
    NetworkTransformChild
}