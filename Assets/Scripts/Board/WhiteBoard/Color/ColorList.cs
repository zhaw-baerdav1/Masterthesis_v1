using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible to handle color changes via event system
public class ColorList : MonoBehaviour
{
    public static event Action OnSwitchColorLeft = delegate { };
    public static event Action OnSwitchColorRight = delegate { };

    //triggers switch to left color
    public static void SwitchColorLeft()
    {
        //trigger all listeners
        OnSwitchColorLeft();
    }

    //triggers switch to left color
    public static void SwitchColorRight()
    {
        //trigger all listeners
        OnSwitchColorRight();
    }
}
