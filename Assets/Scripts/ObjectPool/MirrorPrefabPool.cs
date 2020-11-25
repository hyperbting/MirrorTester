using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPrefabPool : MonoBehaviour, IPrefabPool
{
    [Header("Settings")]
    public GameObject prefab;
    public PrefabPool pool;
    private void OnEnable()
    {
        ClientScene.RegisterPrefab(prefab, SpawnHandler, UnspawnHandler);
    }

    private void OnDisable()
    {
        ClientScene.UnregisterPrefab(prefab);
    }

    void Start()
    {
        pool = new PrefabPool();
        pool.InitializePool(Instantiater);
    }

    #region IPrefabPool
    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        var next = pool.GetFromPool() as GameObject;
        if ( next == null) { return null; }

        // set position/rotation and set active
        next.transform.position = position;
        next.transform.rotation = rotation;
        next.SetActive(true);
        return next;
    }

    public void PutBackInPool(GameObject spawned)
    {
        // disable object
        spawned.SetActive(false);

        // add back to pool
        pool.PutBackInPool(spawned);
    }
    #endregion

    #region
    // used by ClientScene.RegisterPrefab
    GameObject SpawnHandler(SpawnMessage msg)
    {
        var go =  pool.GetFromPool() as GameObject;
        var tra = go.transform;
        tra.position = msg.position;
        tra.rotation = msg.rotation;

        return go;
    }

    // used by ClientScene.RegisterPrefab
    void UnspawnHandler(GameObject spawned)
    {
        spawned.BroadcastMessage("Reset");
        pool.PutBackInPool(spawned);
    }
    #endregion

    #region
    GameObject Instantiater()
    {
        var go = Instantiate(prefab, transform);
        go.name = $"{prefab.name}_pooled_{pool.currentCount}";
        go.SetActive(false);
        return go;
    }
    #endregion
}

public interface IPrefabPool
{
    GameObject GetFromPool(Vector3 position, Quaternion rotation);
    void PutBackInPool(GameObject spawned);
}
