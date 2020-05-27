using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

//responsible for event handling on workspaces
public class WorkspaceList : MonoBehaviour
{
    public static event Action<List<WorkspaceNetworkInfo>> OnWorkspaceListChanged = delegate { };

    private static List<MatchInfoSnapshot> inernetWorkspaceList = new List<MatchInfoSnapshot>();
    private static List<LanConnectionInfo> localWorkspaceList = new List<LanConnectionInfo>();

    public static event Action<WorkspaceNetworkInfo> OnWorkspaceSelected = delegate { };
    private static WorkspaceNetworkInfo selectedWorkspace = null;

    public static event Action<bool> OnWorkspaceActivated = delegate { };
    private static bool activated;
    
    //triggers if new internet workspace list has been found
    public static void HandleInternetWorspaceList(List<MatchInfoSnapshot> _inernetWorkspaceList)
    {
        inernetWorkspaceList = _inernetWorkspaceList;

        //trigger all listeners
        OnWorkspaceListChanged(GetCompleteWorkspaceList());
    }

    //triggers if new LAN workspace list has been found
    public static void HandleLocalWorspaceList(List<LanConnectionInfo> _localWorkspaceList)
    {
        localWorkspaceList = _localWorkspaceList;

        //trigger all listeners
        OnWorkspaceListChanged(GetCompleteWorkspaceList());
    }

    //update list of workspaces as it could contain LAN and internet matches
    private static List<WorkspaceNetworkInfo> GetCompleteWorkspaceList()
    {
        List<WorkspaceNetworkInfo> workspaceList = new List<WorkspaceNetworkInfo>();
        
        //update list of internet matches
        foreach(MatchInfoSnapshot matchInfoSnapshot in inernetWorkspaceList)
        {
            WorkspaceNetworkInfo workspaceNetworkInfo = new WorkspaceNetworkInfo();
            workspaceNetworkInfo.internetMatch = matchInfoSnapshot;

            workspaceList.Add(workspaceNetworkInfo);
        }

        //update list of LAN matches
        foreach (LanConnectionInfo lanConnectionInfo in localWorkspaceList)
        {
            WorkspaceNetworkInfo workspaceNetworkInfo = new WorkspaceNetworkInfo();
            workspaceNetworkInfo.localMatch = lanConnectionInfo;

            workspaceList.Add(workspaceNetworkInfo);
        }

        return workspaceList;
    }

    //triggers if workspace has been selected
    public static void HandleWorkspaceSelected(WorkspaceNetworkInfo _selectedWorkspace)
    {
        selectedWorkspace = _selectedWorkspace;

        //triggers all listeners
        OnWorkspaceSelected(selectedWorkspace);
    }

    //triggers if workspace has been activated
    public static void HandleWorkspaceActivate(bool _activated)
    {
        activated = _activated;

        //triggers all listeners
        OnWorkspaceActivated(activated);
    }
}
