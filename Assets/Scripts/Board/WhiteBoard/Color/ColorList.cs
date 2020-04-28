using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorList : MonoBehaviour
{
    public static event Action OnSwitchColorLeft = delegate { };
    public static event Action OnSwitchColorRight = delegate { };

    public static void SwitchColorLeft()
    {
        OnSwitchColorLeft();
    }
    public static void SwitchColorRight()
    {
        OnSwitchColorRight();
    }
}
