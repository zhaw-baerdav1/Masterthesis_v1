using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DrawingBoardManager : NetworkBehaviour
{

    public class CubeDefinitionList : SyncListStruct<CubeDefinition> { }

    private CubeDefinitionList cubeDefinitionList = new CubeDefinitionList();

    public override void OnStartLocalPlayer()
    {
        CubeList.OnNewCubeDefinition += CubeList_OnNewCubeDefinition;
        cubeDefinitionList.Callback = CubeListUpdated;

        base.OnStartLocalPlayer();
    }

    public void OnDestroy()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CubeList.OnNewCubeDefinition += CubeList_OnNewCubeDefinition;
        cubeDefinitionList.Callback = null;
    }

    private void CubeListUpdated(SyncList<CubeDefinition>.Operation op, int itemIndex)
    {
        List<CubeDefinition> updateCubeDefinitionList = new List<CubeDefinition>();
        foreach(CubeDefinition cubeDefinition in cubeDefinitionList)
        {
            updateCubeDefinitionList.Add(cubeDefinition);
        }

        CubeList.AddNewCubeDefinitionList(updateCubeDefinitionList);
    }

    private void CubeList_OnNewCubeDefinition(CubeDefinition cubeDefinition)
    {
        CmdAddCube(cubeDefinition);
    }

    [Command]
    public void CmdAddCube(CubeDefinition cubeDefinition)
    {
        cubeDefinitionList.Add(cubeDefinition);
    }
}
