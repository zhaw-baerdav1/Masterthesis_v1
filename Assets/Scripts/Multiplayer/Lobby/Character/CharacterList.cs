using System;
using System.Collections.Generic;
using UnityEngine;

//responsible for handling events of the character list
public class CharacterList : MonoBehaviour
{
    public static event Action<List<GameObject>> OnCharacterListChanged = delegate { };
    private static List<GameObject> characterList = new List<GameObject>();

    public static event Action<int> OnCharacterSelected = delegate { };
    private static int selectedCharacterNumber = 0;

    public static event Action<bool> OnCharacterActivated = delegate { };
    private static bool activated;
    
    //triggers if new list has been received
    public static void HandleCharacterList(List<GameObject> _characterList)
    {
        characterList = _characterList;

        //triggers all listeners
        OnCharacterListChanged(characterList);
    }

    //triggers if a character has been selected
    public static void HandleCharacterSelected(int _selectedCharacterNumber)
    {
        selectedCharacterNumber = _selectedCharacterNumber;

        //triggers all listeners
        OnCharacterSelected(selectedCharacterNumber);
    }

    //triggers if a character has been activated
    public static void HandleCharacterActivate(bool _activated)
    {
        activated = _activated;

        //triggers all listeners
        OnCharacterActivated(activated);
    }
}
