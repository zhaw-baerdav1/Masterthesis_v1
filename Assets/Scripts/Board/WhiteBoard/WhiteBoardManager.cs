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

        WhiteBoardEventSystem.OnApplyTexture += WhiteBoardEventSystem_OnApplyTexture;

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

        WhiteBoardEventSystem.OnApplyTexture -= WhiteBoardEventSystem_OnApplyTexture;

        resetPen.RemoveOnChangeListener(OnResetPen, SteamVR_Input_Sources.Any);
        switchColorLeft.RemoveOnChangeListener(OnSwitchColorLeft, SteamVR_Input_Sources.Any);
        switchColorRight.RemoveOnChangeListener(OnSwitchColorRight, SteamVR_Input_Sources.Any);
    }

    private void WhiteBoardEventSystem_OnApplyTexture(int connectionId, int startX, int startY, int width, int height, byte[] textureBytes)
    {
        CmdApplyTexture(connectionId, startX, startY, width, height, textureBytes);
    }

    [Command(channel = 4)]
    private void CmdApplyTexture(int connectionId, int startX, int startY, int width, int height, byte[] textureBytes)
    {
        RpcApplyTexture(connectionId, startX, startY, width, height, textureBytes);
    }

    [ClientRpc]
    private void RpcApplyTexture(int connectionId, int startX, int startY, int width, int height, byte[] textureBytes)
    {
        WhiteBoardEventSystem.ApplyTexture(connectionId, startX, startY, width, height, textureBytes);
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
