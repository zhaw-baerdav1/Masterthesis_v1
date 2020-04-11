﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class CustomJoinSelector : MonoBehaviour
{
    private MatchInfoSnapshot workspaceToJoin;
    private bool isJoining = false;

    private void Awake()
    {
        WorkspaceList.OnWorkspaceSelected += WorkplaceList_OnWorkspaceSelected;
    }

    private void WorkplaceList_OnWorkspaceSelected(MatchInfoSnapshot selectedWorkspace)
    {
        workspaceToJoin = selectedWorkspace;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (workspaceToJoin != null && !isJoining) { 
            FindObjectOfType<CustomNetworkManager>().JoinWorkspace(workspaceToJoin);
            isJoining = true;
        }
    }
}
