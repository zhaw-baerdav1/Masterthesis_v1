using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using Valve.VR;
using static System.Net.Mime.MediaTypeNames;

//responsible for handling steamvr events on workspacelist
public class WorkspaceListPlane : MonoBehaviour
{
    public WorkplaceListItem workplaceListItemPrefab;

    public SteamVR_Action_Boolean lobbySnapTurnRight = SteamVR_Input.GetBooleanAction("Lobby", "LobbySnapTurnRight");
    public SteamVR_Action_Boolean lobbySnapTurnUp = SteamVR_Input.GetBooleanAction("Lobby", "LobbySnapTurnUp");
    public SteamVR_Action_Boolean lobbySnapTurnDown = SteamVR_Input.GetBooleanAction("Lobby", "LobbySnapTurnDown");

    private int count = 0;
    private int selectedNumber = 0;

    //bind events on awake
    private void Awake()
    {
        WorkspaceList.OnWorkspaceListChanged += WorkplaceList_OnWorkspaceListChanged;
        WorkspaceList.OnWorkspaceActivated += WorkplaceList_OnWorkspaceActivated;
    }

    //bind events on destroy
    private void OnDestroy()
    {
        WorkspaceList.OnWorkspaceListChanged -= WorkplaceList_OnWorkspaceListChanged;
        WorkspaceList.OnWorkspaceActivated -= WorkplaceList_OnWorkspaceActivated;

        DeactivateInput();
    }

    //activate listeners if workspaces are selected
    private void ActivateInput()
    {
        lobbySnapTurnRight.AddOnChangeListener(OnWorkspaceSwitch, SteamVR_Input_Sources.Any);
        lobbySnapTurnUp.AddOnChangeListener(OnWorkspaceListUp, SteamVR_Input_Sources.Any);
        lobbySnapTurnDown.AddOnChangeListener(OnWorkspaceListDown, SteamVR_Input_Sources.Any);
    }

    //activate listeners if characters are selected
    private void DeactivateInput()
    {
        lobbySnapTurnRight.RemoveOnChangeListener(OnWorkspaceSwitch, SteamVR_Input_Sources.Any);
        lobbySnapTurnUp.RemoveOnChangeListener(OnWorkspaceListUp, SteamVR_Input_Sources.Any);
        lobbySnapTurnDown.RemoveOnChangeListener(OnWorkspaceListDown, SteamVR_Input_Sources.Any);
    }

    //capture steamvr action to go down on workspace list
    private void OnWorkspaceListDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            //if end of list is reached
            if ( selectedNumber >= (count-1))
            {
                return;
            }

            selectedNumber++;
        }
    }

    //capture steamvr action to go up on workspace list
    private void OnWorkspaceListUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            //if start of list is reached
            if (selectedNumber == 0)
            {
                return;
            }

            selectedNumber--;
        }
    }

    //captures switch between workspace and characters
    private void OnWorkspaceSwitch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            WorkspaceList.HandleWorkspaceActivate(false);
            CharacterList.HandleCharacterActivate(true);
        }
    }

    //triggered if list of workspace has changed
    private void WorkplaceList_OnWorkspaceListChanged(List<WorkspaceNetworkInfo> workspaceList)
    {
        //reset existing workspace
        CleanupWorkspacePlane();

        //update new list
        UpdateWorkspacePlane(workspaceList);
    }

    //triggered if switch has been used
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

    //destroys all existing workspace objects
    private void CleanupWorkspacePlane()
    {
        var workplaceListItems = GetComponentsInChildren<WorkplaceListItem>(true);
        foreach (var workplaceListItem in workplaceListItems)
        {
            Destroy(workplaceListItem.gameObject);
        }
    }

    //add all items in the workspace list to the UI
    private void UpdateWorkspacePlane(List<WorkspaceNetworkInfo> workspaceList)
    {
        count = 0;
        foreach (WorkspaceNetworkInfo workspace in workspaceList)
        {
            WorkplaceListItem workplaceListItem = Instantiate(workplaceListItemPrefab);
            workplaceListItem.Initialize(workspace, transform, count);

            count++;
        }

        //if selected item is not anymore on the list
        if ( selectedNumber > (count-1) )
        {
            selectedNumber = 0;
        }
    }

    public void Update()
    {
        WorkplaceListItem[] workplaceListItems = GetComponentsInChildren<WorkplaceListItem>();

        //mark item on the list with respective color
        for(int i = 0; i<workplaceListItems.Length; i++)
        {
            WorkplaceListItem workplaceListItem = workplaceListItems[i];

            if ( i == selectedNumber)
            {
                workplaceListItem.MarkAsSelected();
                WorkspaceList.HandleWorkspaceSelected(workplaceListItem.GetWorkspaceNetworkInfo());

                continue;
            }

            workplaceListItem.RemoveMarkAsSelected();
        }
    }
}
