using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingBoard : MonoBehaviour
{
    [SerializeField]
    public CubeRepresentation cubePrefab;

    private List<CubeDefinition> cubeList;

    public void removeCubes()
    {
        var cubes = GetComponentsInChildren<CubeDefinition>();
        foreach (var cube in cubes)
        {
            Destroy(cube.gameObject);
        }
    }

    public void drawCubeList(List<CubeDefinition> cubeList)
    {
        this.cubeList = cubeList;

        foreach(CubeDefinition cubeDefinition in cubeList) { 
            var cube = Instantiate(cubePrefab);
            cube.Initialize(cubeDefinition, this.transform);
        }

    }

    private void Update()
    {
        
    }
}
