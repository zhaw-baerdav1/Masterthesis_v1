using System;
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
        if (isJoining)
        {
            return;
        }

        if (workspaceToJoin == null)
        {
            return;
        }

        WorkspaceList.HandleWorkspaceActivate(false);
        CharacterList.HandleCharacterActivate(false);

        foreach (CustomPlayer customPlayer in FindObjectsOfType<CustomPlayer>())
        {
            if (customPlayer.name.Equals("OfflinePlayer"))
            {
                customPlayer.gameObject.SetActive(false);
            }
        }

        FindObjectOfType<CustomNetworkManager>().JoinWorkspace(workspaceToJoin);
        isJoining = true;
        
    }
}
