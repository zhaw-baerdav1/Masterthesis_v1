using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DrawingBoardSetup : MonoBehaviour
{
    public SteamVR_ActionSet actionSetDrawingBoard;

    void Start()
    {
        actionSetDrawingBoard.Activate();
    }
}
