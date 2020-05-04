using IBM.Watson.NaturalLanguageUnderstanding.V1.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadnessSelector : MonoBehaviour
{

    private bool isColliding = false;

    public Material onMaterial;
    public Material offMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        StartCoroutine(Reset());
        
        EmotionScores emotionScores = new EmotionScores();
        emotionScores.Sadness = 0.6f;

        Emotion emotion = new Emotion();
        emotion.setEmotionScores(emotionScores);

        EmotionList.HandleNewEmotion(emotion);

        StartCoroutine(SetTemporarilyOn());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(5);
        isColliding = false;
    }

    IEnumerator SetTemporarilyOn()
    {
        MeshRenderer meshRenderer = transform.parent.GetComponent<MeshRenderer>();

        meshRenderer.material = onMaterial;

        yield return new WaitForSeconds(5);

        meshRenderer.material = offMaterial;
    }
}
