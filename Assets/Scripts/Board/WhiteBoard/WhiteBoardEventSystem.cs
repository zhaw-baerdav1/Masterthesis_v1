using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoardEventSystem : MonoBehaviour
{
    public static event Action<int, int, int, int, int, byte[]> OnApplyTexture = delegate { };

    public static event Action OnResetPens = delegate { };

    public static void ApplyTexture(int connectionId, int startX, int startY, int width, int height, byte[] textureBytes)
    {
        OnApplyTexture(connectionId, startX, startY, width, height, textureBytes);
    }

    public static void ResetPens()
    {
        OnResetPens();
    }
}
