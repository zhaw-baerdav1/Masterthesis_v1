using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

//responsible for capturing steam vr actions and delegating actions
public class DrawingBoardManager : NetworkBehaviour
{

    public SteamVR_Action_Boolean dBGrabPinch = SteamVR_Input.GetBooleanAction("DrawingBoard", "DBGrabPinch");

    public SteamVR_Action_Boolean dBSnapTurnUp = SteamVR_Input.GetBooleanAction("DrawingBoard", "DBSnapTurnUp");
    public SteamVR_Action_Boolean dBSnapTurnDown = SteamVR_Input.GetBooleanAction("DrawingBoard", "DBSnapTurnDown");
    public SteamVR_Action_Boolean dBSnapTurnLeft = SteamVR_Input.GetBooleanAction("DrawingBoard", "DBSnapTurnLeft");
    public SteamVR_Action_Boolean dBSnapTurnRight = SteamVR_Input.GetBooleanAction("DrawingBoard", "DBSnapTurnRight");
    
    //binding all actions/events to player when initiated
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        CubeList.OnNewCubeDefinition += CubeList_OnNewCubeDefinition;
        CubeList.OnTriggerCubeChange += CubeList_OnTriggerCubeChange;
        ArrowList.OnNewArrowDefinition += ArrowList_OnNewArrowDefinition;

        dBGrabPinch.AddOnChangeListener(OnCubeSelected, SteamVR_Input_Sources.Any);

        dBSnapTurnUp.AddOnChangeListener(OnCubeMoveUp, SteamVR_Input_Sources.Any);
        dBSnapTurnDown.AddOnChangeListener(OnCubeMoveDown, SteamVR_Input_Sources.Any);
        dBSnapTurnLeft.AddOnChangeListener(OnCubeMoveLeft, SteamVR_Input_Sources.Any);
        dBSnapTurnRight.AddOnChangeListener(OnCubeMoveRight, SteamVR_Input_Sources.Any);
    }

    //removing all actions/events if object is destroyed
    public void OnDestroy()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CubeList.OnNewCubeDefinition -= CubeList_OnNewCubeDefinition;
        CubeList.OnTriggerCubeChange -= CubeList_OnTriggerCubeChange;
        ArrowList.OnNewArrowDefinition -= ArrowList_OnNewArrowDefinition;

        dBGrabPinch.RemoveOnChangeListener(OnCubeSelected, SteamVR_Input_Sources.Any);

        dBSnapTurnUp.RemoveOnChangeListener(OnCubeMoveUp, SteamVR_Input_Sources.Any);
        dBSnapTurnDown.RemoveOnChangeListener(OnCubeMoveDown, SteamVR_Input_Sources.Any);
        dBSnapTurnLeft.RemoveOnChangeListener(OnCubeMoveLeft, SteamVR_Input_Sources.Any);
        dBSnapTurnRight.RemoveOnChangeListener(OnCubeMoveRight, SteamVR_Input_Sources.Any);
    }
       
    //triggered when cube list is updated by player
    private void CubeList_OnNewCubeDefinition(CubeDefinition cubeDefinition)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //trigger command on server to update all clients
        CmdAddCube(cubeDefinition.id, cubeDefinition.naming, cubeDefinition.position);
    }

    //trigger update of cube on all clients
    [Command]
    private void CmdAddCube(long id, string naming, Vector3 position)
    {
        RpcAddCube(id, naming, position);
    }

    //update cube list on all clients
    [ClientRpc]
    private void RpcAddCube(long id, string naming, Vector3 position)
    {
        CubeDefinition cubeDefinition = new CubeDefinition(id, naming, position);
        CubeList.AddNewCubeDefinitionList(cubeDefinition);
    }

    //triggered by cube change (naming/position) of player
    private void CubeList_OnTriggerCubeChange(CubeDefinition cubeDefinition)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //fire command to server
        CmdChangeCube(cubeDefinition.id, cubeDefinition.naming, cubeDefinition.position);
    }

    //trigger cube change on all clients
    [Command]
    private void CmdChangeCube(long id, string naming, Vector3 position)
    {
        RpcChangeCube(id, naming, position);
    }

    //apply cube change on all clients
    [ClientRpc]
    private void RpcChangeCube(long id, string naming, Vector3 position)
    {
        CubeDefinition cubeDefinition = new CubeDefinition(id, naming, position);
        CubeList.CubeChangeCompleted(cubeDefinition);
    }

    //triggered by changes of arrows by player
    private void ArrowList_OnNewArrowDefinition(ArrowDefinition arrowDefinition)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //fire command to server
        CmdAddArrow(arrowDefinition.id, arrowDefinition.startCubeDefinitionId, arrowDefinition.endCubeDefinitionId);
    }

    //update all clients on arrow change
    [Command]
    private void CmdAddArrow(long id, long startCubeDefinitionId, long endCubeDefinitionId)
    {
        RpcAddArrow(id, startCubeDefinitionId, endCubeDefinitionId);
    }

    //apply arrow change on client
    [ClientRpc]
    private void RpcAddArrow(long id, long startCubeDefinitionId, long endCubeDefinitionId)
    {
        ArrowDefinition arrowDefinition = new ArrowDefinition(id, startCubeDefinitionId, endCubeDefinitionId);
        ArrowList.AddNewArrowDefinitionList(arrowDefinition);
    }

    //handle steamvr action to select cube
    private void OnCubeSelected(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeSelection();
        }
    }

    //handle steamvr action to move cube right
    private void OnCubeMoveRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeMoveRight();
        }
    }

    //handle steamvr action to move cube left
    private void OnCubeMoveLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeMoveLeft();
        }
    }

    //handle steamvr action to move cube down
    private void OnCubeMoveDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeMoveDown();
        }
    }

    //handle steamvr action to move cube up
    private void OnCubeMoveUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            CubeList.CubeMoveUp();
        }
    }
}
