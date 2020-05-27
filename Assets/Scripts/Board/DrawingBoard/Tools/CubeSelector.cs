using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//responsible for the logic on the button of the cube
public class CubeSelector : MonoBehaviour
{
    private bool allowNewCube = true;

    //bind events
    private void Awake()
    {
        CubeList.OnCubeChangeCompleted += CubeList_OnCubeChangeCompleted;
    }

    //unbind events
    private void OnDestroy()
    {
        CubeList.OnCubeChangeCompleted -= CubeList_OnCubeChangeCompleted;
    }

    //activate new cube if list of cube at least has changed once
    private void CubeList_OnCubeChangeCompleted()
    {
        allowNewCube = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //do not execute if button shouldn't be available again
        if (!allowNewCube)
        {
            return;
        }

        //create default cube definition
        CubeDefinition cubeDefinition = new CubeDefinition(0, "[Name me]", new Vector3(0, 0.05f, 0));

        //inform event system on new cube
        CubeList.TriggerNewCubeDefinition(cubeDefinition);
        allowNewCube = false;
    }
}
