using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

public class WhiteBoardManager : NetworkBehaviour
{
    public SteamVR_Action_Boolean resetPen = SteamVR_Input.GetBooleanAction("SnapTurnUp");
    public SteamVR_Action_Boolean switchColorLeft = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    public SteamVR_Action_Boolean switchColorRight = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        WhiteBoardEventSystem.OnSendTexture += WhiteBoardEventSystem_OnSendTexture;

        resetPen.AddOnChangeListener(OnResetPen, SteamVR_Input_Sources.Any);
        switchColorLeft.AddOnChangeListener(OnSwitchColorLeft, SteamVR_Input_Sources.Any);
        switchColorRight.AddOnChangeListener(OnSwitchColorRight, SteamVR_Input_Sources.Any);
    }

    public void OnDestroy()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        WhiteBoardEventSystem.OnSendTexture -= WhiteBoardEventSystem_OnSendTexture;

        resetPen.RemoveOnChangeListener(OnResetPen, SteamVR_Input_Sources.Any);
        switchColorLeft.RemoveOnChangeListener(OnSwitchColorLeft, SteamVR_Input_Sources.Any);
        switchColorRight.RemoveOnChangeListener(OnSwitchColorRight, SteamVR_Input_Sources.Any);
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
}
