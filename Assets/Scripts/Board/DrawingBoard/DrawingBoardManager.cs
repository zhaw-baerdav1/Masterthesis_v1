using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

public class DrawingBoardManager : NetworkBehaviour
{

    public SteamVR_Action_Boolean selectedCube = SteamVR_Input.GetBooleanAction("GrabPinch");

    public SteamVR_Action_Boolean moveCubeUp = SteamVR_Input.GetBooleanAction("SnapTurnUp");
    public SteamVR_Action_Boolean moveCubeDown = SteamVR_Input.GetBooleanAction("SnapTurnDown");
    public SteamVR_Action_Boolean moveCubeLeft = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    public SteamVR_Action_Boolean moveCubeRight = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        CubeList.OnNewCubeDefinition += CubeList_OnNewCubeDefinition;
        CubeList.OnTriggerCubeChange += CubeList_OnTriggerCubeChange;
        ArrowList.OnNewArrowDefinition += ArrowList_OnNewArrowDefinition;

        selectedCube.AddOnChangeListener(OnCubeSelected, SteamVR_Input_Sources.Any);

        moveCubeUp.AddOnChangeListener(OnCubeMoveUp, SteamVR_Input_Sources.Any);
        moveCubeDown.AddOnChangeListener(OnCubeMoveDown, SteamVR_Input_Sources.Any);
        moveCubeLeft.AddOnChangeListener(OnCubeMoveLeft, SteamVR_Input_Sources.Any);
        moveCubeRight.AddOnChangeListener(OnCubeMoveRight, SteamVR_Input_Sources.Any);
    }

    public void OnDestroy()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CubeList.OnNewCubeDefinition -= CubeList_OnNewCubeDefinition;
        CubeList.OnTriggerCubeChange -= CubeList_OnTriggerCubeChange;
        ArrowList.OnNewArrowDefinition -= ArrowList_OnNewArrowDefinition;

        selectedCube.RemoveOnChangeListener(OnCubeSelected, SteamVR_Input_Sources.Any);

        moveCubeUp.RemoveOnChangeListener(OnCubeMoveUp, SteamVR_Input_Sources.Any);
        moveCubeDown.RemoveOnChangeListener(OnCubeMoveDown, SteamVR_Input_Sources.Any);
        moveCubeLeft.RemoveOnChangeListener(OnCubeMoveLeft, SteamVR_Input_Sources.Any);
        moveCubeRight.RemoveOnChangeListener(OnCubeMoveRight, SteamVR_Input_Sources.Any);
    }
       
    private void CubeList_OnNewCubeDefinition(CubeDefinition cubeDefinition)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdAddCube(cubeDefinition.id, cubeDefinition.naming, cubeDefinition.position);
    }

    [Command]
    private void CmdAddCube(long id, string naming, Vector3 position)
    {
        RpcAddCube(id, naming, position);
    }

    [ClientRpc]
    private void RpcAddCube(long id, string naming, Vector3 position)
    {
        CubeDefinition cubeDefinition = new CubeDefinition(id, naming, position);
        CubeList.AddNewCubeDefinitionList(cubeDefinition);
    }

    private void CubeList_OnTriggerCubeChange(CubeDefinition cubeDefinition)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdChangeCube(cubeDefinition.id, cubeDefinition.naming, cubeDefinition.position);
    }

    [Command]
    private void CmdChangeCube(long id, string naming, Vector3 position)
    {
        RpcChangeCube(id, naming, position);
    }

    [ClientRpc]
    private void RpcChangeCube(long id, string naming, Vector3 position)
    {
        CubeDefinition cubeDefinition = new CubeDefinition(id, naming, position);
        CubeList.CubeChangeCompleted(cubeDefinition);
    }

    private void ArrowList_OnNewArrowDefinition(ArrowDefinition arrowDefinition)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdAddArrow(arrowDefinition.id, arrowDefinition.startCubeDefinitionId, arrowDefinition.endCubeDefinitionId);
    }

    [Command]
    private void CmdAddArrow(long id, long startCubeDefinitionId, long endCubeDefinitionId)
    {
        RpcAddArrow(id, startCubeDefinitionId, endCubeDefinitionId);
    }

    [ClientRpc]
    private void RpcAddArrow(long id, long startCubeDefinitionId, long endCubeDefinitionId)
    {
        ArrowDefinition arrowDefinition = new ArrowDefinition(id, startCubeDefinitionId, endCubeDefinitionId);
        ArrowList.AddNewArrowDefinitionList(arrowDefinition);
    }

    private void OnCubeSelected(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeSelection();
        }
    }

    private void OnCubeMoveRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeMoveRight();
        }
    }

    private void OnCubeMoveLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeMoveLeft();
        }
    }

    private void OnCubeMoveDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeMoveDown();
        }
    }

    private void OnCubeMoveUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeMoveUp();
        }
    }
}
