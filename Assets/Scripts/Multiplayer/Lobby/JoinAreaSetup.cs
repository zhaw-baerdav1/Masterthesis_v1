using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class JoinAreaSetup : MonoBehaviour
{
    public SteamVR_ActionSet actionSetLobby;

    private void Awake()
    {
        actionSetLobby.Activate();
    }

    private void Start()
    {
        WorkspaceList.HandleWorkspaceActivate(true);
        CharacterList.HandleCharacterActivate(false);
    }

    private void OnDestroy()
    {
        actionSetLobby.Deactivate();
    }
}
