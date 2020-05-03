using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

public class WhiteBoardManager : NetworkBehaviour
{
    public SteamVR_Action_Boolean wBSnapTurnUp = SteamVR_Input.GetBooleanAction("WhiteBoard", "WBSnapTurnUp");
    public SteamVR_Action_Boolean wBSnapTurnDown = SteamVR_Input.GetBooleanAction("WhiteBoard", "WBSnapTurnDown");
    public SteamVR_Action_Boolean wBSnapTurnLeft = SteamVR_Input.GetBooleanAction("WhiteBoard", "WBSnapTurnLeft");
    public SteamVR_Action_Boolean wBSnapTurnRight = SteamVR_Input.GetBooleanAction("WhiteBoard", "WBSnapTurnRight");
    
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        WhiteBoardEventSystem.OnSendTexture += WhiteBoardEventSystem_OnSendTexture;

        wBSnapTurnUp.AddOnChangeListener(OnResetPen, SteamVR_Input_Sources.Any);
        wBSnapTurnDown.AddOnChangeListener(OnResetWhiteBoard, SteamVR_Input_Sources.Any);
        wBSnapTurnLeft.AddOnChangeListener(OnSwitchColorLeft, SteamVR_Input_Sources.Any);
        wBSnapTurnRight.AddOnChangeListener(OnSwitchColorRight, SteamVR_Input_Sources.Any);
    }

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

    private void WhiteBoardEventSystem_OnSendTexture(int connectionId, Rect sendableRectangle, byte[] textureBytes)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdApplyTexture(connectionId, sendableRectangle, textureBytes);
    }

    [Command(channel = 4)]
    private void CmdApplyTexture(int connectionId, Rect sendableRectangle, byte[] textureBytes)
    {
        RpcApplyTexture(connectionId, sendableRectangle, textureBytes);
    }

    [ClientRpc]
    private void RpcApplyTexture(int connectionId, Rect sendableRectangle, byte[] textureBytes)
    {
        WhiteBoardEventSystem.ReceiveTexture(connectionId, sendableRectangle, textureBytes);
    }

    [Command]
    private void CmdResetWhiteBoard()
    {
        RpcResetWhiteBoard();
    }

    [ClientRpc]
    private void RpcResetWhiteBoard()
    {
        WhiteBoardEventSystem.ResetWhiteBoard();
    }

    private void OnSwitchColorRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            ColorList.SwitchColorRight();
        }
    }

    private void OnSwitchColorLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {

        if (newState)
        {
            ColorList.SwitchColorLeft();
        }
    }

    private void OnResetPen(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            WhiteBoardEventSystem.ResetPens();
        }
    }

    private void OnResetWhiteBoard(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            if (!isLocalPlayer)
            {
                return;
            }

            CmdResetWhiteBoard();
        }
    }
}
