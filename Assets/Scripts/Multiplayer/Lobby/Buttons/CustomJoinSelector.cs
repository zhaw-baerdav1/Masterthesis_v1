using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

//responsible for handling button collider on join button
public class CustomJoinSelector : MonoBehaviour
{
    private WorkspaceNetworkInfo workspaceNetworkInfo;
    private bool isJoining = false;

    //bind events
    private void Awake()
    {
        WorkspaceList.OnWorkspaceSelected += WorkplaceList_OnWorkspaceSelected;
    }

    //unbind events
    private void OnDestroy()
    {
        WorkspaceList.OnWorkspaceSelected -= WorkplaceList_OnWorkspaceSelected;
    }

    //triggered if workspace has been selected
    private void WorkplaceList_OnWorkspaceSelected(WorkspaceNetworkInfo workspaceNetworkInfo)
    {
        this.workspaceNetworkInfo = workspaceNetworkInfo;
    }

    private void OnTriggerEnter(Collider other)
    {
        //do not join twice
        if (isJoining)
        {
            return;
        }

        //do not join if no worksapce is selected
        if (workspaceNetworkInfo == null)
        {
            return;
        }

        //join workspace with networkmanager
        FindObjectOfType<CustomNetworkManager>().JoinWorkspace(workspaceNetworkInfo);
        isJoining = true;
        
    }
}
