using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class CustomJoinSelector : MonoBehaviour
{
    private MatchInfoSnapshot workspaceToJoin;

    private void Awake()
    {
        WorkspaceList.OnWorkspaceListChanged += WorkplaceList_OnWorkspaceListChanged;
    }

    private void WorkplaceList_OnWorkspaceListChanged(List<MatchInfoSnapshot> workspaceList)
    {
        if (workspaceList.Count < 1)
        {
            return;
        }

        if (workspaceList.Count > 1)
        {
            throw new NotImplementedException("More workspace created than planned.");
        }

        workspaceToJoin = workspaceList[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (workspaceToJoin != null) { 
            FindObjectOfType<CustomNetworkManager>().JoinWorkspace(workspaceToJoin);
        }
    }
}
