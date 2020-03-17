using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionList : MonoBehaviour
{
    public static event Action<Emotion> OnNewEmotion = delegate { };
    private static Emotion emotion;

    public static void HandleNewEmotion(Emotion newEmotion)
    {
        emotion = newEmotion;

        OnNewEmotion(emotion);
    }
}
