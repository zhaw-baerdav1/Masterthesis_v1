using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using Valve.VR;
using static System.Net.Mime.MediaTypeNames;

public class CharacterListPlane : MonoBehaviour
{

    public CharacterListItem characterListItemPrefab;

    public SteamVR_Action_Boolean snapTurnLeft = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    public SteamVR_Action_Boolean snapTurnUp = SteamVR_Input.GetBooleanAction("SnapTurnUp");
    public SteamVR_Action_Boolean snapTurnDown = SteamVR_Input.GetBooleanAction("SnapTurnDown");

    private int count = 0;
    private int selectedNumber = 0;

    private void Awake()
    {
        CharacterList.OnCharacterListChanged += CharacterList_OnCharacterListChanged;
        CharacterList.OnCharacterActivated += CharacterList_OnCharacterActivated;
    }

    private void ActivateInput()
    {
        snapTurnLeft.AddOnChangeListener(OnWorkspaceSwitch, SteamVR_Input_Sources.Any);
        snapTurnUp.AddOnChangeListener(OnWorkspaceListUp, SteamVR_Input_Sources.Any);
        snapTurnDown.AddOnChangeListener(OnWorkspaceListDown, SteamVR_Input_Sources.Any);
    }

    private void DeactivateInput()
    {
        snapTurnLeft.RemoveOnChangeListener(OnWorkspaceSwitch, SteamVR_Input_Sources.Any);
        snapTurnUp.RemoveOnChangeListener(OnWorkspaceListUp, SteamVR_Input_Sources.Any);
        snapTurnDown.RemoveOnChangeListener(OnWorkspaceListDown, SteamVR_Input_Sources.Any);
    }

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



    private void OnWorkspaceSwitch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (newState)
        {
            WorkspaceList.HandleWorkspaceActivate(true);
            CharacterList.HandleCharacterActivate(false);
        }
    }

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

    private void CharacterList_OnCharacterListChanged(List<GameObject> characterList)
    {
        CleanupCharacterPlane();
        UpdateCharacterPlane(characterList);
    }

    private void CleanupCharacterPlane()
    {
        var characterListItems = GetComponentsInChildren<CharacterListItem>(true);
        foreach (var characterListItem in characterListItems)
        {
            Destroy(characterListItem.gameObject);
        }
    }

    private void UpdateCharacterPlane(List<GameObject> characterList)
    {
        count = 0;
        foreach (GameObject character in characterList)
        {
            CharacterListItem characterListItem = Instantiate(characterListItemPrefab);
            characterListItem.Initialize(character, transform, count);

            count++;
        }

        if ( selectedNumber > (count-1) )
        {
            selectedNumber = 0;
        }
    }

    private void Update()
    {
        CharacterListItem[] characterListItems = GetComponentsInChildren<CharacterListItem>();

        for(int i = 0; i< characterListItems.Length; i++)
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
