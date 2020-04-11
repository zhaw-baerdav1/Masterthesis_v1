using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : MonoBehaviour
{
    public static event Action<List<GameObject>> OnCharacterListChanged = delegate { };
    private static List<GameObject> characterList = new List<GameObject>();

    public static event Action<GameObject> OnCharacterSelected = delegate { };
    private static GameObject selectedCharacter = null;

    public static event Action<bool> OnCharacterActivated = delegate { };
    private static bool activated;
    
    public static void HandleCharacterList(List<GameObject> _characterList)
    {
        characterList = _characterList;

        OnCharacterListChanged(characterList);
    }

    public static void HandleCharacterSelected(GameObject _selectedCharacter)
    {
        selectedCharacter = _selectedCharacter;

        OnCharacterSelected(selectedCharacter);
    }

    public static void HandleCharacterActivate(bool _activated)
    {
        activated = _activated;

        OnCharacterActivated(activated);
    }
}
