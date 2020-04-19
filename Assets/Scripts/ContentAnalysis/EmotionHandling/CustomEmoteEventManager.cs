using CrazyMinnow.SALSA;
using IBM.Watson.NaturalLanguageUnderstanding.V1.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEmoteEventManager : MonoBehaviour
{
    enum EmotionType
    {
        Anger,
        Disgust,
        Fear,
        Joy,
        Sadness
    }

    private static double emotionScoreThreshold = 0.5;

    private static Dictionary<EmotionType, string> emotionDictionary = new Dictionary<EmotionType, string>()
        {
            { EmotionType.Anger, "anger" },
            { EmotionType.Disgust, "disgust" },
            { EmotionType.Fear, "fear" },
            { EmotionType.Joy, "joy" },
            { EmotionType.Sadness, "sadness" }
        };

    private void Awake()
    {
        EmotionList.OnNewEmotion += EmotionList_OnNewEmotion;
    }

    private void OnDestroy()
    {
        EmotionList.OnNewEmotion -= EmotionList_OnNewEmotion;
    }

    private void EmotionList_OnNewEmotion(Emotion emotion)
    {

        Dictionary<EmotionType, double> relevantEmotionTypeDictionary = getRelevantEmotionTypeDictionary(emotion.getEmotionScores());
        foreach (KeyValuePair<EmotionType, double> emotionTypeEntry in relevantEmotionTypeDictionary) {
            string expressionComponentName = emotionDictionary[emotionTypeEntry.Key];
            double score = emotionTypeEntry.Value;

            float frac = (float) (1 - (1 - score));
            Emoter[] emoterList = FindObjectsOfType<Emoter>();
            foreach (Emoter emoter in emoterList) { 
                emoter.ManualEmote(expressionComponentName, ExpressionComponent.ExpressionHandler.OneWay, 1, true, frac);
            }
        }

    }

    private Dictionary<EmotionType, double> getRelevantEmotionTypeDictionary(EmotionScores emotionScores)
    {
        Dictionary<EmotionType, double> emotionTypeDictonary = new Dictionary<EmotionType, double>();

        if (emotionScores.Anger.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Anger, emotionScores.Anger.Value);
        }

        if (emotionScores.Disgust.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Disgust, emotionScores.Disgust.Value);
        }

        if (emotionScores.Fear.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Fear, emotionScores.Fear.Value);
        }

        if (emotionScores.Joy.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Joy, emotionScores.Joy.Value);
        }

        if (emotionScores.Sadness.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Sadness, emotionScores.Sadness.Value);
        }

        return emotionTypeDictonary;
    }
}
