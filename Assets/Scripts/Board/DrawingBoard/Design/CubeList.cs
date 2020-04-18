using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeList : MonoBehaviour
{
    public static event Action<CubeDefinition> OnNewCubeDefinition = delegate { };

    public static event Action<List<CubeDefinition>> OnNewCubeDefinitionList = delegate { };

    public static void AddNewCubeDefinition(CubeDefinition cubeDefinition)
    {
        OnNewCubeDefinition(cubeDefinition);
    }
    public static void AddNewCubeDefinitionList(List<CubeDefinition> cubeDefinitionList)
    {
        OnNewCubeDefinitionList(cubeDefinitionList);
    }
}
