﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using Valve.VR;
using static System.Net.Mime.MediaTypeNames;

public class WorkspaceListPlane : MonoBehaviour
{
    public WorkplaceListItem workplaceListItemPrefab;

    public SteamVR_Action_Boolean snapTurnRight = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    public SteamVR_Action_Boolean snapTurnUp = SteamVR_Input.GetBooleanAction("SnapTurnUp");
    public SteamVR_Action_Boolean snapTurnDown = SteamVR_Input.GetBooleanAction("SnapTurnDown");

    private int count = 0;
    private int selectedNumber = 0;

    private void Awake()
    {
        WorkspaceList.OnWorkspaceListChanged += WorkplaceList_OnWorkspaceListChanged;
        WorkspaceList.OnWorkspaceActivated += WorkplaceList_OnWorkspaceActivated;
    }

    private void OnDestroy()
    {
        WorkspaceList.OnWorkspaceListChanged -= WorkplaceList_OnWorkspaceListChanged;
        WorkspaceList.OnWorkspaceActivated -= WorkplaceList_OnWorkspaceActivated;

        DeactivateInput();
    }

    private void ActivateInput()
    {
        snapTurnRight.AddOnChangeListener(OnWorkspaceSwitch, SteamVR_Input_Sources.Any);
        snapTurnUp.AddOnChangeListener(OnWorkspaceListUp, SteamVR_Input_Sources.Any);
        snapTurnDown.AddOnChangeListener(OnWorkspaceListDown, SteamVR_Input_Sources.Any);
    }

    private void DeactivateInput()
    {
        snapTurnRight.RemoveOnChangeListener(OnWorkspaceSwitch, SteamVR_Input_Sources.Any);
        snapTurnUp.RemoveOnChangeListener(OnWorkspaceListUp, SteamVR_Input_Sources.Any);
        snapTurnDown.RemoveOnChangeListener(OnWorkspaceListDown, SteamVR_Input_Sources.Any);
    }

    private void OnWorkspaceListDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            if ( selectedNumber >= (count-1))
            {
                return;
            }

            selectedNumber++;
        }
    }

    private void OnWorkspaceListUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            if (selectedNumber == 0)
            {
                return;
            }

            selectedNumber--;
        }
    }

    private void OnWorkspaceSwitch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            WorkspaceList.HandleWorkspaceActivate(false);
            CharacterList.HandleCharacterActivate(true);
        }
    }

    private void WorkplaceList_OnWorkspaceListChanged(List<MatchInfoSnapshot> workspaceList)
    {
        CleanupWorkspacePlane();
        UpdateWorkspacePlane(workspaceList);
    }

    private void WorkplaceList_OnWorkspaceActivated(bool activated)
    {
        if (activated)
        {
            ActivateInput();
        }
        else
        {
            DeactivateInput();
        }
    }

    private void CleanupWorkspacePlane()
    {
        var workplaceListItems = GetComponentsInChildren<WorkplaceListItem>(true);
        foreach (var workplaceListItem in workplaceListItems)
        {
            Destroy(workplaceListItem.gameObject);
        }
    }

    private void UpdateWorkspacePlane(List<MatchInfoSnapshot> workspaceList)
    {
        count = 0;
        foreach (MatchInfoSnapshot workspace in workspaceList)
        {
            WorkplaceListItem workplaceListItem = Instantiate(workplaceListItemPrefab);
            workplaceListItem.Initialize(workspace, transform, count);

            count++;
        }

        if ( selectedNumber > (count-1) )
        {
            selectedNumber = 0;
        }
    }

    private void Update()
    {
        WorkplaceListItem[] workplaceListItems = GetComponentsInChildren<WorkplaceListItem>();

        for(int i = 0; i<workplaceListItems.Length; i++)
        {
            WorkplaceListItem workplaceListItem = workplaceListItems[i];

            if ( i == selectedNumber)
            {
                workplaceListItem.MarkAsSelected();
                WorkspaceList.HandleWorkspaceSelected(workplaceListItem.GetMatchInfoSnapshot());
            }
            else
            {
                workplaceListItem.RemoveMarkAsSelected();
            }

        }
    }
}