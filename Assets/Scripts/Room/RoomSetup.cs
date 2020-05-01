using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RoomSetup : MonoBehaviour
{
    public SteamVR_ActionSet actionSetRoomControl;

    private void Start()
    {
        actionSetRoomControl.Activate();
    }
}
