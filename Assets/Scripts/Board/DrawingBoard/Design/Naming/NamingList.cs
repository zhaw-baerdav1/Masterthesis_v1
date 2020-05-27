using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for handling events of the naming changes
public class NamingList : MonoBehaviour
{
    public static event Action<bool> OnRecordingModeChange = delegate { };
    public static bool recordingMode = false;

    //triggers the change in the recording mode
    public static void ChangeRecordingMode(bool on)
    {
        recordingMode = on;

        //triggers all listeners
        OnRecordingModeChange(recordingMode);
    }

    //received if IBM cloud has answered with a final text
    public static void UsableNamingDetected(string text)
    {
        //do not apply if not recording
        if (!recordingMode)
        {
            return;
        }

        //do not apply if text is empty
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        //ensure first character is upper case
        char[] textChar = text.ToCharArray();
        textChar[0] = char.ToUpper(textChar[0]);
        text = new string(textChar);

        //trigger cube change and reset recording mode
        CubeList.TriggerCubeChange(text);
        ChangeRecordingMode(false);
    }
}
