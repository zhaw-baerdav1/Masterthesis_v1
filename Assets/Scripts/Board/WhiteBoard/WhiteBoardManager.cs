using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

//responsible to handle all steamvr actions on the whiteboard
public class WhiteBoardManager : NetworkBehaviour
{
    public SteamVR_Action_Boolean wBSnapTurnUp = SteamVR_Input.GetBooleanAction("WhiteBoard", "WBSnapTurnUp");
    public SteamVR_Action_Boolean wBSnapTurnDown = SteamVR_Input.GetBooleanAction("WhiteBoard", "WBSnapTurnDown");
    public SteamVR_Action_Boolean wBSnapTurnLeft = SteamVR_Input.GetBooleanAction("WhiteBoard", "WBSnapTurnLeft");
    public SteamVR_Action_Boolean wBSnapTurnRight = SteamVR_Input.GetBooleanAction("WhiteBoard", "WBSnapTurnRight");
    
    //bind all events on local start of player
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        WhiteBoardEventSystem.OnSendTexture += WhiteBoardEventSystem_OnSendTexture;

        wBSnapTurnUp.AddOnChangeListener(OnResetPen, SteamVR_Input_Sources.Any);
        wBSnapTurnDown.AddOnChangeListener(OnResetWhiteBoard, SteamVR_Input_Sources.Any);
        wBSnapTurnLeft.AddOnChangeListener(OnSwitchColorLeft, SteamVR_Input_Sources.Any);
        wBSnapTurnRight.AddOnChangeListener(OnSwitchColorRight, SteamVR_Input_Sources.Any);
    }

    //unbind all events on destroying player object
    public void OnDestroy()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        WhiteBoardEventSystem.OnSendTexture -= WhiteBoardEventSystem_OnSendTexture;

        wBSnapTurnUp.RemoveOnChangeListener(OnResetPen, SteamVR_Input_Sources.Any);
        wBSnapTurnDown.RemoveOnChangeListener(OnResetWhiteBoard, SteamVR_Input_Sources.Any);
        wBSnapTurnLeft.RemoveOnChangeListener(OnSwitchColorLeft, SteamVR_Input_Sources.Any);
        wBSnapTurnRight.RemoveOnChangeListener(OnSwitchColorRight, SteamVR_Input_Sources.Any);
    }

    //triggered by sending texture of whiteboard to network
    private void WhiteBoardEventSystem_OnSendTexture(int connectionId, Rect sendableRectangle, byte[] textureBytes)
    {
        //do not continue if not local player
        if (!isLocalPlayer)
        {
            return;
        }

        //fire command to server
        CmdApplyTexture(connectionId, sendableRectangle, textureBytes);
    }

    //fire update on all clients (channel=4 ensure the correct channel is used and can handle the amount of data)
    [Command(channel = 4)]
    private void CmdApplyTexture(int connectionId, Rect sendableRectangle, byte[] textureBytes)
    {
        RpcApplyTexture(connectionId, sendableRectangle, textureBytes);
    }

    //apply texture on all clients
    [ClientRpc]
    private void RpcApplyTexture(int connectionId, Rect sendableRectangle, byte[] textureBytes)
    {
        WhiteBoardEventSystem.ReceiveTexture(connectionId, sendableRectangle, textureBytes);
    }

    //resets whiteboard on all clients
    [Command]
    private void CmdResetWhiteBoard()
    {
        RpcResetWhiteBoard();
    }

    //apply resetting whiteboard an all clients
    [ClientRpc]
    private void RpcResetWhiteBoard()
    {
        WhiteBoardEventSystem.ResetWhiteBoard();
    }

    //capture steamvr action to switch color right
    private void OnSwitchColorRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            ColorList.SwitchColorRight();
        }
    }

    //capture steamvr action to switch color right
    private void OnSwitchColorLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {

        if (newState)
        {
            ColorList.SwitchColorLeft();
        }
    }

    //capture steamvr action to switch color left
    private void OnResetPen(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            WhiteBoardEventSystem.ResetPens();
        }
    }

    //capture steamvr action to reset whiteboard
    private void OnResetWhiteBoard(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            //do not continue if not local player
            if (!isLocalPlayer)
            {
                return;
            }

            //fire command to server
            CmdResetWhiteBoard();
        }
    }
}
