using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorStatus : MonoBehaviour
{
    [SerializeField] double serverTime;
    [SerializeField] double serverRTT;

    // Update is called once per frame
    void Update()
    {
        if (!IsInvoking("UpdateMirrorStatus"))
            InvokeRepeating("UpdateMirrorStatus",0,1);
    }

    void UpdateMirrorStatus()
    {
        serverTime = NetworkTime.time;
        serverRTT = NetworkTime.rtt;
    }
}
