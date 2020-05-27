using Dissonance;
using Dissonance.Audio.Capture;
using IBM.Cloud.SDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//responsible for binding modules for voice to player
public class VoiceManager : NetworkBehaviour
{
    private DissonanceComms dissonanceComms;
    private TextStreamer textStreamer;
    private BasicMicrophoneCapture basicMicrophoneCapture;

    private IEnumerator coroAudioCaptureLinkage;
    private const float PollTimer = .5f;

    //initiate local module
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //do not continue if no text is streamed
        textStreamer = FindObjectOfType<TextStreamer>();
        if(textStreamer == null)
        {
            return;
        }

        //find dissonance module
        dissonanceComms = FindObjectOfType<DissonanceComms>();

        //start coroutine to link dissonance to the text streamer
        if (coroAudioCaptureLinkage != null)
        {
            StopCoroutine(coroAudioCaptureLinkage);
        }
        coroAudioCaptureLinkage = WaitAudioCaptureLink();
        StartCoroutine(coroAudioCaptureLinkage);

    }

    //unsubscribe to dissonance if destroyed
    private void OnDestroy()
    {
        if (basicMicrophoneCapture != null && textStreamer != null)
        {
            basicMicrophoneCapture.Unsubscribe(textStreamer);
        }
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

        //subscribe to dissonance to receive voice
        basicMicrophoneCapture.Subscribe(textStreamer);
    }
}
