using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class WorkspaceList : MonoBehaviour
{
    public static event Action<List<MatchInfoSnapshot>> OnWorkspaceListChanged = delegate { };
    private static List<MatchInfoSnapshot> workspaceList = new List<MatchInfoSnapshot>();

    public static void HandleWorspaceList(List<MatchInfoSnapshot> _workspaceList)
    {
        workspaceList = _workspaceList;

        OnWorkspaceListChanged(workspaceList);
    }
}
