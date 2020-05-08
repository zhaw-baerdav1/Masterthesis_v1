using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class WorkspaceList : MonoBehaviour
{
    public static event Action<List<WorkspaceNetworkInfo>> OnWorkspaceListChanged = delegate { };

    private static List<MatchInfoSnapshot> inernetWorkspaceList = new List<MatchInfoSnapshot>();
    private static List<LanConnectionInfo> localWorkspaceList = new List<LanConnectionInfo>();

    public static event Action<WorkspaceNetworkInfo> OnWorkspaceSelected = delegate { };
    private static WorkspaceNetworkInfo selectedWorkspace = null;

    public static event Action<bool> OnWorkspaceActivated = delegate { };
    private static bool activated;
    
    public static void HandleInternetWorspaceList(List<MatchInfoSnapshot> _inernetWorkspaceList)
    {
        inernetWorkspaceList = _inernetWorkspaceList;

        OnWorkspaceListChanged(GetCompleteWorkspaceList());
    }

    public static void HandleLocalWorspaceList(List<LanConnectionInfo> _localWorkspaceList)
    {
        localWorkspaceList = _localWorkspaceList;

        OnWorkspaceListChanged(GetCompleteWorkspaceList());
    }

    private static List<WorkspaceNetworkInfo> GetCompleteWorkspaceList()
    {
        List<WorkspaceNetworkInfo> workspaceList = new List<WorkspaceNetworkInfo>();
        foreach(MatchInfoSnapshot matchInfoSnapshot in inernetWorkspaceList)
        {
            WorkspaceNetworkInfo workspaceNetworkInfo = new WorkspaceNetworkInfo();
            workspaceNetworkInfo.internetMatch = matchInfoSnapshot;

            workspaceList.Add(workspaceNetworkInfo);
        }
        foreach (LanConnectionInfo lanConnectionInfo in localWorkspaceList)
        {
            WorkspaceNetworkInfo workspaceNetworkInfo = new WorkspaceNetworkInfo();
            workspaceNetworkInfo.localMatch = lanConnectionInfo;

            workspaceList.Add(workspaceNetworkInfo);
        }

        return workspaceList;
    }

    public static void HandleWorkspaceSelected(WorkspaceNetworkInfo _selectedWorkspace)
    {
        selectedWorkspace = _selectedWorkspace;

        OnWorkspaceSelected(selectedWorkspace);
    }

    public static void HandleWorkspaceActivate(bool _activated)
    {
        activated = _activated;

        OnWorkspaceActivated(activated);
    }
}
