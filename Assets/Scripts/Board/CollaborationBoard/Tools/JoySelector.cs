using IBM.Watson.NaturalLanguageUnderstanding.V1.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for the logic on the button expressing joy
public class JoySelector : MonoBehaviour
{
    //inidicate if button is should be available for pushing and triggering actions
    private bool isColliding = false;

    //material to show if button is active or not
    public Material onMaterial;
    public Material offMaterial;

    private void OnTriggerEnter(Collider other)
    {
        //do not execute if button shouldn't be available again
        if (isColliding) return;
        isColliding = true;

        //start coroutine to delay availability of button
        StartCoroutine(Reset());

        //initiate emotion to be expressed
        EmotionScores emotionScores = new EmotionScores();
        emotionScores.Joy = 0.6f;

        //prepare data transfer object to use existing emotion events
        Emotion emotion = new Emotion();
        emotion.setEmotionScores(emotionScores);

        //trigger emotion event
        EmotionList.HandleNewEmotion(emotion);

        //start coroutine to temporarly activate emotion
        StartCoroutine(SetTemporarilyOn());
    }


    //responsible for delaying the availability of the button
    IEnumerator Reset()
    {
        //wait until availability is activated again
        yield return new WaitForSeconds(5);
        isColliding = false;
    }

    //responsible for indicating the user the activity of the button
    IEnumerator SetTemporarilyOn()
    {
        //set active material for 5 seconds and the reset to inactive material
        MeshRenderer meshRenderer = transform.parent.GetComponent<MeshRenderer>();

        meshRenderer.material = onMaterial;

        yield return new WaitForSeconds(5);

        meshRenderer.material = offMaterial;
    }
}
