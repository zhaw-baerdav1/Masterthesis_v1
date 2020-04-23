using Dissonance;
using Dissonance.Audio.Capture;
using IBM.Cloud.SDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VoiceManager : NetworkBehaviour
{
    private DissonanceComms dissonanceComms;
    private TextStreamer textStreamer;
    private BasicMicrophoneCapture basicMicrophoneCapture;

    private IEnumerator coroAudioCaptureLinkage;
    private const float PollTimer = .5f;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        textStreamer = FindObjectOfType<TextStreamer>();
        if(textStreamer == null)
        {
            return;
        }

        dissonanceComms = FindObjectOfType<DissonanceComms>();

        if (coroAudioCaptureLinkage != null)
        {
            StopCoroutine(coroAudioCaptureLinkage);
        }
        coroAudioCaptureLinkage = WaitAudioCaptureLink();
        StartCoroutine(coroAudioCaptureLinkage);

    }

    private IEnumerator WaitAudioCaptureLink()
    {
        // implement internal timer to avoid WaitForSeconds GC
        var timeCheck = Time.time;

        //Find the playerstate for this playerid
        while (basicMicrophoneCapture == null)
        {
            if (Time.time - timeCheck > PollTimer)
            {
                basicMicrophoneCapture = dissonanceComms.GetComponent<BasicMicrophoneCapture>();
                timeCheck = Time.time;
            }

            yield return null;
        }

        basicMicrophoneCapture.Subscribe(textStreamer);
    }
}
