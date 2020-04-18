using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class DrawingBoard : MonoBehaviour
{

    [SerializeField]
    public CubeRepresentation cubePrefab;

    private bool spawnLocationFree = true;
              
    public void Awake()
    {
        CubeList.OnNewCubeDefinitionList += CubeList_OnNewCubeDefinitionList;
    }

    private void CubeList_OnNewCubeDefinitionList(List<CubeDefinition> cubeDefinitionList)
    {
        RemoveCubes();

        foreach (CubeDefinition cubeDefinition in cubeDefinitionList)
        {
            CubeRepresentation cube = Instantiate(cubePrefab);
            cube.setCubeDefinition(cubeDefinition);
            cube.AttachToDrawingBoard(transform);
            cube.Initialize();
        }
    }

    public void RemoveCubes()
    {
        var cubes = GetComponentsInChildren<CubeRepresentation>();
        foreach (var cube in cubes)
        {
            Destroy(cube.gameObject);
        }
    }

    public bool isSpawnLocationFree()
    {
        return spawnLocationFree;
    }
}
