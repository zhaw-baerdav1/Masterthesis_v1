using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for delegating events for animations
public class AnimationEventSystem : MonoBehaviour
{
    public static event Action OnResetIdeaAnimations = delegate { };
    public static event Action OnResetAgreeAnimations = delegate { };

    //fires the reset of the animation which shows the idea indication
    public static void ResetIdeaAnimations()
    {
        OnResetIdeaAnimations();
    }

    //fires the reset of the animation which shows the agree indication
    public static void ResetAgreeAnimations()
    {
        OnResetAgreeAnimations();
    }
}
