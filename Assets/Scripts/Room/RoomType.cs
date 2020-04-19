using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    private static List<string> roomTypeList = new List<string>(){
         "OfficeRoom",
         "MedievalRoom",
         "ForestRoom"
    };

    public static string getNextRoom(string _currentRoomName)
    {
        int currentIndex = roomTypeList.IndexOf(_currentRoomName);

        if ( currentIndex == (roomTypeList.Count - 1) )
        {
            return roomTypeList[0];
        }

        return roomTypeList[currentIndex+1];
    }
}
