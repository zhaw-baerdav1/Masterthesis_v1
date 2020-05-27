using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//responsible for steamvr action set setup
public class JoinAreaSetup : MonoBehaviour
{
    public SteamVR_ActionSet actionSetLobby;

    //activate action set on awake
    private void Awake()
    {
        actionSetLobby.Activate();
    }

    //set default selection on lobby screen
    private void Start()
    {
        WorkspaceList.HandleWorkspaceActivate(true);
        CharacterList.HandleCharacterActivate(false);
    }

    //deactivate action set on destroy
    private void OnDestroy()
    {
        actionSetLobby.Deactivate();
    }
}
