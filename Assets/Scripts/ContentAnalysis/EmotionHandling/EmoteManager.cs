using CrazyMinnow.SALSA;
using IBM.Watson.NaturalLanguageUnderstanding.V1.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//responsible for applying emotions on player
public class EmoteManager : NetworkBehaviour
{
    //enum to ensure consistency of data used
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

    Emoter emoter;

    private void Start()
    {
        //initate emoter of SALSA
        emoter = GetComponent<Emoter>();
    }

    //bind events
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        EmotionList.OnNewEmotion += EmotionList_OnNewEmotion;
    }

    //unbind events
    private void OnDestroy()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        EmotionList.OnNewEmotion -= EmotionList_OnNewEmotion;
    }

    //triggered if a new emotion has to be shown
    private void EmotionList_OnNewEmotion(Emotion emotion)
    {
        //do not continue if not local player
        if (!isLocalPlayer)
        {
            return;
        }

        //go through all emotions which should be triggered at the same time
        Dictionary<EmotionType, double> relevantEmotionTypeDictionary = getRelevantEmotionTypeDictionary(emotion.getEmotionScores());
        foreach (KeyValuePair<EmotionType, double> emotionTypeEntry in relevantEmotionTypeDictionary) {
            string expressionComponentName = emotionDictionary[emotionTypeEntry.Key];
            double score = emotionTypeEntry.Value;

            //fire command to server to express emotion
            CmdExpressEmote(expressionComponentName, score);
        }

    }

    //defines on how to express emotion
    private void ExpressEmote(string expressionComponentName, double score)
    {
        //degree of emotion expression
        float frac = (float)(1 - (1 - score));

        //use salsa interface to apply emotion on face of player
        emoter.ManualEmote(expressionComponentName, ExpressionComponent.ExpressionHandler.RoundTrip, 5f, true, frac);
    }

    //trigger emotion on clients for this player
    [Command]
    private void CmdExpressEmote(string expressionComponentName, double score)
    {
        RpcExpressEmote(expressionComponentName, score);
    }

    //apply emotion on all clients
    [ClientRpc]
    private void RpcExpressEmote(string expressionComponentName, double score)
    {
        ExpressEmote(expressionComponentName, score);
    }

    //identify relevant emotions based on result of IBM Cloud
    private Dictionary<EmotionType, double> getRelevantEmotionTypeDictionary(EmotionScores emotionScores)
    {
        Dictionary<EmotionType, double> emotionTypeDictonary = new Dictionary<EmotionType, double>();

        if (emotionScores.Anger != null &&  emotionScores.Anger.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Anger, emotionScores.Anger.Value);
        }

        if (emotionScores.Disgust != null && emotionScores.Disgust.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Disgust, emotionScores.Disgust.Value);
        }

        if (emotionScores.Fear != null && emotionScores.Fear.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Fear, emotionScores.Fear.Value);
        }

        if (emotionScores.Joy != null && emotionScores.Joy.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Joy, emotionScores.Joy.Value);
        }

        if (emotionScores.Sadness != null && emotionScores.Sadness.Value > emotionScoreThreshold)
        {
            emotionTypeDictonary.Add(EmotionType.Sadness, emotionScores.Sadness.Value);
        }

        return emotionTypeDictonary;
    }
}
