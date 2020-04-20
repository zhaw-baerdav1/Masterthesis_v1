using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Valve.VR;

public class RoomManager : NetworkBehaviour
{

    public SteamVR_Action_Boolean roomSwitch = SteamVR_Input.GetBooleanAction("RoomSwitch");
    
    public override void OnStartLocalPlayer()
    {
        roomSwitch.AddOnChangeListener(OnRoomSwitch, SteamVR_Input_Sources.Any);

        base.OnStartLocalPlayer();
    }

    public void OnDestroy()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        roomSwitch.RemoveOnChangeListener(OnRoomSwitch, SteamVR_Input_Sources.Any);
    }

    private void OnRoomSwitch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (!newState)
        {
            return;
        }

        CmdChangeToNextRoom();    
    }

    [Command]
    private void CmdChangeToNextRoom()
    {
        CustomNetworkManager networkManager = FindObjectOfType<CustomNetworkManager>();
        networkManager.ChangeToNextRoom();
    }
}
