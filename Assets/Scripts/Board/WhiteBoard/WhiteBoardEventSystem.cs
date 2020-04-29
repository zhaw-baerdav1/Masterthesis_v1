using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoardEventSystem : MonoBehaviour
{
    public static event Action<int, int, int> OnApplyTexture = delegate { };

    public static event Action OnResetPens = delegate { };

    public static void ApplyTexture(int ownerConnectionId, int x, int y)
    {
        OnApplyTexture(ownerConnectionId, x, y);
    }

    public static void ResetPens()
    {
        OnResetPens();
    }
}
