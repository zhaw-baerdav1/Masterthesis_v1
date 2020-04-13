using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DrawingBoard : NetworkBehaviour
{
    [SerializeField]
    public CubeRepresentation cubePrefab;

    private bool spawnLocationFree = true;

    private List<CubeDefinition> cubeList;

    public void removeCubes()
    {
        var cubes = GetComponentsInChildren<CubeDefinition>();
        foreach (var cube in cubes)
        {
            Destroy(cube.gameObject);
        }
    }

    public void drawNewCubeList(List<CubeDefinition> cubeList)
    {
        this.cubeList = cubeList;

        foreach(CubeDefinition cubeDefinition in cubeList) {

            CubeRepresentation cube = Instantiate(cubePrefab);
            cube.AttachToDrawingBoard(transform);
            cube.Initialize(cubeDefinition);

            CmdSpawnCube(cube.gameObject);
        }

        spawnLocationFree = false;
    }

    [Command]
    public void CmdSpawnCube(GameObject cubeGameObject)
    {
        CubeRepresentation cubeRepresentation = cubeGameObject.GetComponent<CubeRepresentation>();

        NetworkServer.Spawn(cubeGameObject);
    }

    private void Update()
    {
        
    }

    public bool isSpawnLocationFree()
    {
        return spawnLocationFree;
    }
}
