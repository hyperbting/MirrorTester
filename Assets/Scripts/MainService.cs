using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainService : MonoBehaviour
{
    static MainService instance;
    public static MainService Instance
    {
        get {
            if (instance == null)
            {
                var go = GameObject.Find("MainService");
                if (go != null)
                    instance = go.GetComponent<MainService>();
            }

            return instance;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);   
    }

    public PlayerManager playerManager;
    public MirrorNetworkManager networkManager;
}
