using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Valve.VR;

public class RoomManager : NetworkBehaviour
{

    public SteamVR_Action_Boolean menu = SteamVR_Input.GetBooleanAction("Menu");

    void Start()
    {
        menu.AddOnChangeListener(OnSceneSwitch, SteamVR_Input_Sources.Any);
    }

    private void OnSceneSwitch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
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
        networkManager.ServerChangeScene("ForestRoom");
    }
}
