using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible to handle events on whiteboard
public class WhiteBoardEventSystem : MonoBehaviour
{
    public static event Action<int, Rect, byte[]> OnSendTexture = delegate { };
    public static event Action<int, Rect, byte[]> OnReceiveTexture = delegate { };

    public static event Action OnResetPens = delegate { };
    public static event Action OnResetWhiteBoard = delegate { };

    //triggers send of texture on network
    public static void SendTexture(int connectionId, Rect sendableRectangle, byte[] textureBytes)
    {
        //trigger all listener
        OnSendTexture(connectionId, sendableRectangle, textureBytes);
    }

    //triggers receive of texture on network
    public static void ReceiveTexture(int connectionId, Rect receivableRectangle, byte[] textureBytes)
    {
        //trigger all listener
        OnReceiveTexture(connectionId, receivableRectangle, textureBytes);
    }

    //triggers resetting pens
    public static void ResetPens()
    {
        //trigger all listener
        OnResetPens();
    }

    //triggers resetting whiteboard
    public static void ResetWhiteBoard()
    {
        //trigger all listener
        OnResetWhiteBoard();
    }
}
