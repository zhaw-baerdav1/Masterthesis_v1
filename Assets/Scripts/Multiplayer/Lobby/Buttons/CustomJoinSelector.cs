using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class CustomJoinSelector : MonoBehaviour
{
    private WorkspaceNetworkInfo workspaceNetworkInfo;
    private bool isJoining = false;

    private void Awake()
    {
        WorkspaceList.OnWorkspaceSelected += WorkplaceList_OnWorkspaceSelected;
    }

    private void OnDestroy()
    {
        WorkspaceList.OnWorkspaceSelected -= WorkplaceList_OnWorkspaceSelected;
    }

    private void WorkplaceList_OnWorkspaceSelected(WorkspaceNetworkInfo workspaceNetworkInfo)
    {
        this.workspaceNetworkInfo = workspaceNetworkInfo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isJoining)
        {
            return;
        }

        if (workspaceNetworkInfo == null)
        {
            return;
        }

        FindObjectOfType<CustomNetworkManager>().JoinWorkspace(workspaceNetworkInfo);
        isJoining = true;
        
    }
}
