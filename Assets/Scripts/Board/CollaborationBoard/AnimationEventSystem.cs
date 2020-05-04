using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventSystem : MonoBehaviour
{
    public static event Action OnResetIdeaAnimations = delegate { };
    public static event Action OnResetAgreeAnimations = delegate { };

    public static void ResetIdeaAnimations()
    {
        OnResetIdeaAnimations();
    }
    public static void ResetAgreeAnimations()
    {
        OnResetAgreeAnimations();
    }
}
