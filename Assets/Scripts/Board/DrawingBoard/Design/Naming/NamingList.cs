using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingList : MonoBehaviour
{
    public static event Action<bool> OnRecordingModeChange = delegate { };
    public static bool recordingMode = false;

    public static void ChangeRecordingMode(bool on)
    {
        recordingMode = on;
        OnRecordingModeChange(recordingMode);
    }

    public static void UsableNamingDetected(string text)
    {
        if (!recordingMode)
        {
            return;
        }

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        char[] textChar = text.ToCharArray();
        textChar[0] = char.ToUpper(textChar[0]);
        text = new string(textChar);

        CubeList.TriggerCubeChange(text);
        ChangeRecordingMode(false);
    }
}
