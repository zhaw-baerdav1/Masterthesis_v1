using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using static System.Net.Mime.MediaTypeNames;

public class WorkspaceListPlane : MonoBehaviour
{

    private void Awake()
    {
        WorkspaceList.OnWorkspaceListChanged += WorkplaceList_OnWorkspaceListChanged;
        UpdateWorkspacePlane(new List<MatchInfoSnapshot>());
    }

    private void WorkplaceList_OnWorkspaceListChanged(List<MatchInfoSnapshot> workspaceList)
    {
        UpdateWorkspacePlane(workspaceList);
    }

    private void UpdateWorkspacePlane(List<MatchInfoSnapshot> workspaceList)
    {

        var workspaceListItems = GetComponentsInChildren<WorkplaceListItem>(true);
        foreach (var workspaceListItem in workspaceListItems)
        {
            if (workspaceListItem.name.Equals("ReadyPlane") && workspaceList.Count > 0)
            {
                workspaceListItem.gameObject.SetActive(true);
            }
            if (workspaceListItem.name.Equals("WaitingPlane") && workspaceList.Count > 0)
            {
                workspaceListItem.gameObject.SetActive(false);
            }
            if (workspaceListItem.name.Equals("ReadyPlane") && workspaceList.Count < 1)
            {
                workspaceListItem.gameObject.SetActive(false);
            }
            if (workspaceListItem.name.Equals("WaitingPlane") && workspaceList.Count < 1)
            {
                workspaceListItem.gameObject.SetActive(true);
            }
        }

    }
}
