﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHostSelector : MonoBehaviour
{
    private bool isHosting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isHosting)
        {
            return;
        }

        WorkspaceList.HandleWorkspaceActivate(false);
        CharacterList.HandleCharacterActivate(false);

        foreach (CustomPlayer customPlayer in FindObjectsOfType<CustomPlayer>())
        {
            if (customPlayer.name.Equals("OfflinePlayer"))
            {
                customPlayer.gameObject.SetActive(false);
            }
        }

        FindObjectOfType<CustomNetworkManager>().StartHosting();
        isHosting = true;
    }
}
