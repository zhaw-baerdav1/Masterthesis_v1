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

    void Awake()
    {
        roomSwitch.AddOnChangeListener(OnRoomSwitch, SteamVR_Input_Sources.Any);
    }

    private void OnDestroy()
    {
        roomSwitch.RemoveOnChangeListener(OnRoomSwitch, SteamVR_Input_Sources.Any);
    }

    private void OnRoomSwitch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (!newState)
        {
            return;
        }

        if (!isServer)
        {
            return;
        }

        CustomNetworkManager networkManager = FindObjectOfType<CustomNetworkManager>();
        networkManager.ChangeToNextRoom();        
    }
}
