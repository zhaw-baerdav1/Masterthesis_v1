using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

public class WhiteBoardManager : NetworkBehaviour
{
    public SteamVR_Action_Boolean switchColorLeft = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    public SteamVR_Action_Boolean switchColorRight = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        switchColorLeft.AddOnChangeListener(OnSwitchColorLeft, SteamVR_Input_Sources.Any);
        switchColorRight.AddOnChangeListener(OnSwitchColorRight, SteamVR_Input_Sources.Any);
    }

    public void OnDestroy()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        switchColorLeft.RemoveOnChangeListener(OnSwitchColorLeft, SteamVR_Input_Sources.Any);
        switchColorRight.RemoveOnChangeListener(OnSwitchColorRight, SteamVR_Input_Sources.Any);
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
}
