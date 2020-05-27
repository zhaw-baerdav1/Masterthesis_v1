using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for handling events of emotions
public class EmotionList : MonoBehaviour
{
    public static event Action<Emotion> OnNewEmotion = delegate { };
    private static Emotion emotion;

    //triggers if a new emotion has to be shown
    public static void HandleNewEmotion(Emotion newEmotion)
    {
        emotion = newEmotion;

        //trigger all listeners
        OnNewEmotion(emotion);
    }
}
