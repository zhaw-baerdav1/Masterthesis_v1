using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeSelector : MonoBehaviour
{
    private bool allowNewCube = true;

    private void Awake()
    {
        CubeList.OnCubeChangeCompleted += CubeList_OnCubeChangeCompleted;
    }

    private void OnDestroy()
    {
        CubeList.OnCubeChangeCompleted -= CubeList_OnCubeChangeCompleted;
    }

    private void CubeList_OnCubeChangeCompleted()
    {
        allowNewCube = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!allowNewCube)
        {
            return;
        }

        CubeDefinition cubeDefinition = new CubeDefinition(0, "[Name me]", new Vector3(0, 0.05f, 0));
        CubeList.TriggerNewCubeDefinition(cubeDefinition);
        allowNewCube = false;
    }
}
