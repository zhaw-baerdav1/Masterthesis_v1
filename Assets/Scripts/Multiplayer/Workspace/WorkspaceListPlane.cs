using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using static System.Net.Mime.MediaTypeNames;

public class WorkspaceListPlane : MonoBehaviour
{
    [SerializeField]
    private WorkplaceListItem readyPlanePrefab;
    [SerializeField]
    private WorkplaceListItem waitingPlanePrefab;

    private void Awake()
    {
        WorkspaceList.OnWorkspaceListChanged += WorkplaceList_OnWorkspaceListChanged;
        UpdateWorkspacePlane(new List<MatchInfoSnapshot>());
    }

    private void WorkplaceList_OnWorkspaceListChanged(List<MatchInfoSnapshot> workspaceList)
    {
        UpdateWorkspacePlane(workspaceList);
    }

    private void RemovePlanes()
    {
        var workspaceListItems = GetComponentsInChildren<WorkplaceListItem>();
        foreach (var workspaceListItem in workspaceListItems)
        {
            Destroy(workspaceListItem.gameObject);
        }
    }

    private void UpdateWorkspacePlane(List<MatchInfoSnapshot> workspaceList)
    {
        RemovePlanes();
        if ( workspaceList.Count > 0)
        {
            var statusPlane = Instantiate(readyPlanePrefab);
            statusPlane.InitiatePlane(this.transform);
        } else
        {
            var statusPlane = Instantiate(waitingPlanePrefab);
            statusPlane.InitiatePlane(this.transform);
        }

    }
}
