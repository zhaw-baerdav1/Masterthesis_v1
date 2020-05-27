using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//responsible for setting up steamvr action set
public class DrawingBoardSetup : MonoBehaviour
{
    public SteamVR_ActionSet actionSetDrawingBoard;

    //activate action set when started
    private void Awake()
    {
        actionSetDrawingBoard.Activate();
    }

    //deactivate action set when destroyed
    private void OnDestroy()
    {
        actionSetDrawingBoard.Deactivate();
    }
}
