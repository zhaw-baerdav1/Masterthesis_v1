using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class WorkspaceList : MonoBehaviour
{
    public static event Action<List<MatchInfoSnapshot>> OnWorkspaceListChanged = delegate { };
    private static List<MatchInfoSnapshot> workspaceList = new List<MatchInfoSnapshot>();

    public static event Action<MatchInfoSnapshot> OnWorkspaceSelected = delegate { };
    private static MatchInfoSnapshot selectedWorkspace = null;

    public static void HandleWorspaceList(List<MatchInfoSnapshot> _workspaceList)
    {
        workspaceList = _workspaceList;

        OnWorkspaceListChanged(workspaceList);
    }

    public static void HandleWorkspaceSelected(MatchInfoSnapshot _selectedWorkspace)
    {
        selectedWorkspace = _selectedWorkspace;

        OnWorkspaceSelected(selectedWorkspace);
    }
}
