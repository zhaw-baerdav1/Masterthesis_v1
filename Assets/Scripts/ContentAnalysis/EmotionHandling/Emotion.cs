using IBM.Watson.NaturalLanguageUnderstanding.V1.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//data transfer object for emotions
public class Emotion
{
    private EmotionScores emotionScores;

    public EmotionScores getEmotionScores()
    {
        return this.emotionScores;
    }

    public void setEmotionScores(EmotionScores emotionScores)
    {
        this.emotionScores = emotionScores;
    }

}
