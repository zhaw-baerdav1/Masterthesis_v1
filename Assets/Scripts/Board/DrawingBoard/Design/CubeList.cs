using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeList : MonoBehaviour
{
    public static event Action<List<CubeDefinition>> OnNewCubeDefinitionList = delegate { };
    private static List<CubeDefinition> cubeDefinitionList = new List<CubeDefinition>();

    public static void AddNewCubeDefinition(CubeDefinition cubeDefinition)
    {
        cubeDefinitionList.Add(cubeDefinition);

        OnNewCubeDefinitionList(cubeDefinitionList);
    }
}
