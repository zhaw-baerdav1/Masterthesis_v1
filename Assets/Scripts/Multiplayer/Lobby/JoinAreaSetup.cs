using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class JoinAreaSetup : MonoBehaviour
{
    public SteamVR_ActionSet actionSetLobby;

    void Start()
    {
        WorkspaceList.HandleWorkspaceActivate(true);
        CharacterList.HandleCharacterActivate(false);

        actionSetLobby.Activate();
    }

    public void DeactivateSetup()
    {
        actionSetLobby.Deactivate();

        foreach (CustomPlayer customPlayer in FindObjectsOfType<CustomPlayer>())
        {
            if (customPlayer.name.Equals("OfflinePlayer"))
            {
                customPlayer.gameObject.SetActive(false);
            }
        }
    }
}
