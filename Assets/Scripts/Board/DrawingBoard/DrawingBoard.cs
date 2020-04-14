using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Valve.VR;

public class DrawingBoard : NetworkBehaviour
{
    [SerializeField]
    public CubeRepresentation cubePrefab;

    private bool spawnLocationFree = true;

    private List<CubeDefinition> cubeList;

    public SteamVR_Action_Boolean menu = SteamVR_Input.GetBooleanAction("Menu");

    public void Start()
    {
        menu.AddOnChangeListener(OnSceneSwitch, SteamVR_Input_Sources.Any);
    }

    private void OnSceneSwitch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if ( newState)
        {
            if (isServer) {
                SceneManager.LoadScene("ForestRoom");

                CustomNetworkManager networkManager = FindObjectOfType<CustomNetworkManager>();
                networkManager.ServerChangeScene("ForestRoom");
            }
        }
    }

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
            cube.setCubeDefinition(cubeDefinition);

            CmdSpawnCube(cube.gameObject);
        }

        spawnLocationFree = false;
    }

    [Command]
    public void CmdSpawnCube(GameObject cubeGameObject)
    {
        NetworkServer.Spawn(cubeGameObject);
        RpcInitializeCube(cubeGameObject);
    }

    [ClientRpc]
    void RpcInitializeCube(GameObject cubeGameObject)
    {
        CubeRepresentation cube = cubeGameObject.GetComponent<CubeRepresentation>();
        cube.AttachToDrawingBoard(transform);
        cube.Initialize();
    }

    private void Update()
    {
        
    }

    public bool isSpawnLocationFree()
    {
        return spawnLocationFree;
    }
}
