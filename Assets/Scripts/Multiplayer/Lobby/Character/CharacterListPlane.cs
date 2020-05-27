using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using Valve.VR;
using static System.Net.Mime.MediaTypeNames;

//responsible for handling steamvr events on workspacelist
public class CharacterListPlane : MonoBehaviour
{

    public CharacterListItem characterListItemPrefab;

    public SteamVR_Action_Boolean lobbySnapTurnLeft = SteamVR_Input.GetBooleanAction("Lobby", "LobbySnapTurnLeft");
    public SteamVR_Action_Boolean lobbySnapTurnUp = SteamVR_Input.GetBooleanAction("Lobby", "LobbySnapTurnUp");
    public SteamVR_Action_Boolean lobbySnapTurnDown = SteamVR_Input.GetBooleanAction("Lobby", "LobbySnapTurnDown");

    private int count = 0;
    private int selectedNumber = 0;

    //bind events
    private void Awake()
    {
        CharacterList.OnCharacterListChanged += CharacterList_OnCharacterListChanged;
        CharacterList.OnCharacterActivated += CharacterList_OnCharacterActivated;
    }

    //unbind events
    private void OnDestroy()
    {
        CharacterList.OnCharacterListChanged -= CharacterList_OnCharacterListChanged;
        CharacterList.OnCharacterActivated -= CharacterList_OnCharacterActivated;

        DeactivateInput();
    }

    //activate input if characterlist is chosen
    private void ActivateInput()
    {
        lobbySnapTurnLeft.AddOnChangeListener(OnWorkspaceSwitch, SteamVR_Input_Sources.Any);
        lobbySnapTurnUp.AddOnChangeListener(OnWorkspaceListUp, SteamVR_Input_Sources.Any);
        lobbySnapTurnDown.AddOnChangeListener(OnWorkspaceListDown, SteamVR_Input_Sources.Any);
    }

    //deactivate input if workspacelist is chosen
    private void DeactivateInput()
    {
        lobbySnapTurnLeft.RemoveOnChangeListener(OnWorkspaceSwitch, SteamVR_Input_Sources.Any);
        lobbySnapTurnUp.RemoveOnChangeListener(OnWorkspaceListUp, SteamVR_Input_Sources.Any);
        lobbySnapTurnDown.RemoveOnChangeListener(OnWorkspaceListDown, SteamVR_Input_Sources.Any);
    }

    //capture steamvr action to go down on character list
    private void OnWorkspaceListDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            if ( selectedNumber >= (count-1))
            {
                return;
            }

            selectedNumber++;
        }
    }

    //capture steamvr action to go up on character list
    private void OnWorkspaceListUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            if (selectedNumber == 0)
            {
                return;
            }

            selectedNumber--;
        }
    }


    //captures switch between workspace and characters
    private void OnWorkspaceSwitch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            WorkspaceList.HandleWorkspaceActivate(true);
            CharacterList.HandleCharacterActivate(false);
        }
    }

    //triggered if switch has been used
    private void CharacterList_OnCharacterActivated(bool activated)
    {
        if (activated)
        {
            ActivateInput();
        }
        else
        {
            DeactivateInput();
        }
    }

    //triggered if list of character has changed
    private void CharacterList_OnCharacterListChanged(List<GameObject> characterList)
    {
        //reset existing characters
        CleanupCharacterPlane();

        //update new list
        UpdateCharacterPlane(characterList);
    }

    //destroys all existing character objects
    private void CleanupCharacterPlane()
    {
        var characterListItems = GetComponentsInChildren<CharacterListItem>(true);
        foreach (var characterListItem in characterListItems)
        {
            Destroy(characterListItem.gameObject);
        }
    }

    //add all items in the workspace list to the UI
    private void UpdateCharacterPlane(List<GameObject> characterList)
    {
        count = 0;
        foreach (GameObject character in characterList)
        {
            CharacterListItem characterListItem = Instantiate(characterListItemPrefab);
            characterListItem.Initialize(character, transform, count);

            count++;
        }

        //if selected item is not anymore on the list
        if ( selectedNumber > (count-1) )
        {
            selectedNumber = 0;
        }
    }

    private void Update()
    {
        CharacterListItem[] characterListItems = GetComponentsInChildren<CharacterListItem>();

        //mark item on the list with respective color
        for (int i = 0; i< characterListItems.Length; i++)
        {
            CharacterListItem characterListItem = characterListItems[i];

            if ( i == selectedNumber)
            {
                characterListItem.MarkAsSelected();
                CharacterList.HandleCharacterSelected(selectedNumber);
            }
            else
            {
                characterListItem.RemoveMarkAsSelected();
            }

        }
    }
}
