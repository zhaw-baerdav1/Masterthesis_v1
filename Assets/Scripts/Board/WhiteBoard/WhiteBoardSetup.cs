using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WhiteBoardSetup : MonoBehaviour
{
    public SteamVR_ActionSet actionSetWhiteBoard;

    private void Awake()
    {
        actionSetWhiteBoard.Activate();
    }

    private void OnDestroy()
    {
        actionSetWhiteBoard.Deactivate();
    }
}
