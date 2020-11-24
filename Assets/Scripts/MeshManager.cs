using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{
    [SerializeField] Transform parent;

    public void KillChildren()
    {
        if (parent == null)
            return;

        foreach (var tra in parent)
        {

        }
    }
}
