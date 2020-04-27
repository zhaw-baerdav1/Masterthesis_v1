using IBM.Watson.NaturalLanguageUnderstanding.V1.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerSelector : MonoBehaviour
{

    private bool isColliding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        StartCoroutine(Reset());
        
        EmotionScores emotionScores = new EmotionScores();
        emotionScores.Anger = 0.6f;

        Emotion emotion = new Emotion();
        emotion.setEmotionScores(emotionScores);

        EmotionList.HandleNewEmotion(emotion);
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1);
        isColliding = false;
    }
}
