using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoardEventSystem : MonoBehaviour
{
    public static event Action<int, Rect, byte[]> OnSendTexture = delegate { };
    public static event Action<int, Rect, byte[]> OnReceiveTexture = delegate { };

    public static event Action OnResetPens = delegate { };

    public static void SendTexture(int connectionId, Rect sendableRectangle, byte[] textureBytes)
    {
        OnSendTexture(connectionId, sendableRectangle, textureBytes);
    }
    public static void ReceiveTexture(int connectionId, Rect receivableRectangle, byte[] textureBytes)
    {
        OnReceiveTexture(connectionId, receivableRectangle, textureBytes);
    }

    public static void ResetPens()
    {
        OnResetPens();
    }
}
