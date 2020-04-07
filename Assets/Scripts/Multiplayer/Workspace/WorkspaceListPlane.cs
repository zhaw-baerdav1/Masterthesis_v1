using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using Valve.VR;
using static System.Net.Mime.MediaTypeNames;

public class WorkspaceListPlane : MonoBehaviour
{

    public WorkplaceListItem workplaceListItemPrefab;

    private SteamVR_Action_Boolean grabPinch = SteamVR_Input.GetBooleanAction("GrabPinch");

    private void Awake()
    {
        WorkspaceList.OnWorkspaceListChanged += WorkplaceList_OnWorkspaceListChanged;
    }

    private void Start()
    {
        grabPinch.AddOnChangeListener(OnWorkspaceSelected, SteamVR_Input_Sources.RightHand);
    }

    private void OnWorkspaceSelected(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            //WorkspaceList.OnWorkspaceSelected();
        }
    }

    private void WorkplaceList_OnWorkspaceListChanged(List<MatchInfoSnapshot> workspaceList)
    {
        //CleanupWorkspacePlane();
        UpdateWorkspacePlane(workspaceList);
    }

    private void CleanupWorkspacePlane()
    {
        WorkplaceListItem[] workplaceListItems = GetComponentsInChildren<WorkplaceListItem>();
        foreach (WorkplaceListItem workplaceListItem in workplaceListItems)
        {
            Destroy(workplaceListItem.gameObject);
        }
    }

    int count = 0;
    private void UpdateWorkspacePlane(List<MatchInfoSnapshot> workspaceList)
    {
        foreach(MatchInfoSnapshot workspace in workspaceList)
        {
            
            WorkplaceListItem workplaceListItem = Instantiate(workplaceListItemPrefab);
            workplaceListItem.gameObject.SetActive(true);
            workplaceListItem.Initialize(workspace, transform, count);

            count++;
        }

    }
}
