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
        CubeList.OnNewCubeDefinitionList += CubeList_OnNewCubeDefinitionList;
    }

    private void OnDestroy()
    {
        CubeList.OnNewCubeDefinitionList -= CubeList_OnNewCubeDefinitionList;
    }

    private void CubeList_OnNewCubeDefinitionList(List<CubeDefinition> cubeDefinitionList)
    {
        allowNewCube = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!allowNewCube)
        {
            return;
        }

        CubeDefinition cubeDefinition = new CubeDefinition(0, "Test1", new Vector3(0, 0.05f, 0));
        CubeList.TriggerNewCubeDefinition(cubeDefinition);
        allowNewCube = false;
    }
}
