using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class RoomList : MonoBehaviour
{
    public static event Action<List<string>> OnRoomNameListChanged = delegate { };
    private static List<string> roomNameList = new List<string>();

    public static event Action<string> OnRoomNameSelected = delegate { };
    private static string selectedRoomName = null;
    
    public static void HandleRoomNameList(List<string> _roomNameList)
    {
        roomNameList = _roomNameList;

        OnRoomNameListChanged(roomNameList);
    }

    public static void HandleRoomNameSelected(string _selectedRoomName)
    {
        selectedRoomName = _selectedRoomName;

        OnRoomNameSelected(selectedRoomName);
    }
}
