using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] List<MirrorPrefabPool> poolList = new List<MirrorPrefabPool>();
    public List<MirrorPrefabPool> PoolList { get => poolList; /*set => poolList = value;*/ }

    Dictionary<string, MirrorPrefabPool> dic = new Dictionary<string, MirrorPrefabPool>();

    private void Start()
    {
        foreach (var pool in PoolList)
            dic[pool.prefab.name] = pool;
    }

    public GameObject Instantiate(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (dic.TryGetValue(prefab.name, out var tarPool))
        {
            return tarPool.GetFromPool(pos,rot);
        }

        GameObject poolGO = new GameObject();
        poolGO.transform.parent = transform;
        poolGO.SetActive(false);

        var pool = poolGO.AddComponent<MirrorPrefabPool>();
        pool.prefab = prefab;

        poolList.Add(pool);
        dic[pool.prefab.name] = pool;

        poolGO.SetActive(true);
        return pool.GetFromPool(pos, rot);
    }

    public void Destroy(GameObject go)
    {
        var firstPart = go.name.Split('_')[0];
        if (dic.TryGetValue(firstPart, out var tarPool))
        {
            tarPool.PutBackInPool(go);
            return;
        }

        Debug.LogWarning($"{go.name} is not Belong to any Pool");
    }
}
