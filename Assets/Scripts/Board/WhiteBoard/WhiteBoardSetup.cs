using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//responsible for steamvr action set setup
public class WhiteBoardSetup : MonoBehaviour
{
    public SteamVR_ActionSet actionSetWhiteBoard;

    //activate action set on awake
    private void Awake()
    {
        actionSetWhiteBoard.Activate();
    }

    //deactivate action set on destroy
    private void OnDestroy()
    {
        actionSetWhiteBoard.Deactivate();
    }
}
