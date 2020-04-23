using System.Collections;
using UnityEngine;
using IBM.Watson.SpeechToText.V1;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;
using Dissonance.Audio.Capture;
using System;
using NAudio.Wave;

public class TextStreamer : MonoBehaviour, IMicrophoneSubscriber
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/speech-to-text/api\"")]
    [SerializeField]
    private string _serviceUrl;
    [Tooltip("Text field to display the results of streaming.")]
    public TextMesh ResultsField;
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string _iamApikey;

    [SerializeField]
    public TextAnalyzer textAnalyzer;
    [SerializeField]
    public ToneAnalyzer toneAnalyzer;

    [Header("Parameters")]
    // https://www.ibm.com/watson/developercloud/speech-to-text/api/v1/curl.html?curl#get-model
    [Tooltip("The Model to use. This defaults to en-US_BroadbandModel")]
    [SerializeField]
    private string _recognizeModel;
    #endregion


    private int _recordingRoutine = 0;
    private string _microphoneID = null;
    private int _recordingBufferSize = 1;
    private int _recordingHZ = 22050;

    private SpeechToTextService _service;

    void Start()
    {
        LogSystem.InstallDefaultReactors();
        
        
    }

    public bool Active
    {
        get { return _service.IsListening; }
        set
        {
            if (value && !_service.IsListening)
            {
                _service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "en-US_BroadbandModel" : _recognizeModel);
                _service.DetectSilence = true;
                _service.EnableWordConfidence = true;
                _service.EnableTimestamps = true;
                _service.SilenceThreshold = 0.01f;
                _service.MaxAlternatives = 1;
                _service.EnableInterimResults = true;
                _service.OnError = OnError;
                _service.InactivityTimeout = -1;
                _service.ProfanityFilter = false;
                _service.SmartFormatting = true;
                _service.SpeakerLabels = false;
                _service.WordAlternativesThreshold = null;
                _service.StartListening(OnRecognize, OnRecognizeSpeaker);
            }
            else if (!value && _service.IsListening)
            {
                _service.StopListening();
            }
        }
    }

    private void OnError(string error)
    {
        Active = false;

        Log.Debug("TextSreamer.OnError()", "Error! {0}", error);
    }

    public IEnumerator RecordingHandler(string microphoneID, AudioClip recording, int recordingHZ)
    {

        if (string.IsNullOrEmpty(_iamApikey))
        {
            throw new IBMException("Plesae provide IAM ApiKey for the service.");
        }

        IamAuthenticator authenticator = new IamAuthenticator(apikey: _iamApikey);

        //  Wait for tokendata
        while (!authenticator.CanAuthenticate())
            yield return null;

        _service = new SpeechToTextService(authenticator);
        if (!string.IsNullOrEmpty(_serviceUrl))
        {
            _service.SetServiceUrl(_serviceUrl);
        }
        _service.StreamMultipart = true;

        Active = true;

        UnityObjectUtil.StartDestroyQueue();

        Log.Debug("TextSreamer.RecordingHandler()", "devices: {0}", Microphone.devices);

        if (_service == null)
        {
           yield break;
        }

        bool bFirstBlock = true;
        int midPoint = recording.samples / 2;
        float[] samples = null;

        while (recording != null)
        {
            int writePos = Microphone.GetPosition(microphoneID);
            
            if (writePos > recording.samples || !Microphone.IsRecording(microphoneID))
            {
                Log.Error("TextSreamer.RecordingHandler()", "Microphone disconnected.");

                yield break;
            }

            if ((bFirstBlock && writePos >= midPoint)
              || (!bFirstBlock && writePos < midPoint))
            {
                // front block is recorded, make a RecordClip and pass it onto our callback.
                samples = new float[midPoint];
                recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                AudioData record = new AudioData();
                record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
                record.Clip = AudioClip.Create("Recording", midPoint, recording.channels, recordingHZ, false);
                record.Clip.SetData(samples, 0);

                _service.OnListen(record);

                bFirstBlock = !bFirstBlock;
            }
            else
            {
                // calculate the number of samples remaining until we ready for a block of audio, 
                // and wait that amount of time it will take to record.
                int remaining = bFirstBlock ? (midPoint - writePos) : (recording.samples - writePos);
                float timeRemaining = (float)remaining / (float)recordingHZ;

                yield return new WaitForSeconds(timeRemaining);
            }
        }
        yield break;
    }

    private void OnRecognize(SpeechRecognitionEvent result)
    {
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                    Log.Debug("TextSreamer.OnRecognize()", text);
                    ResultsField.text = text;
                    if ( res.final)
                    {
                        Runnable.Run(textAnalyzer.Analyze(text));
                        Runnable.Run(toneAnalyzer.Analyze(text));
                    }
                }

                if (res.keywords_result != null && res.keywords_result.keyword != null)
                {
                    foreach (var keyword in res.keywords_result.keyword)
                    {
                        Log.Debug("TextSreamer.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
                    }
                }

                if (res.word_alternatives != null)
                {
                    foreach (var wordAlternative in res.word_alternatives)
                    {
                        Log.Debug("TextSreamer.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
                        foreach (var alternative in wordAlternative.alternatives)
                            Log.Debug("TextSreamer.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                    }
                }
            }
        }
    }

    private void OnRecognizeSpeaker(SpeakerRecognitionEvent result)
    {
        if (result != null)
        {
            foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
            {
                Log.Debug("TextSreamer.OnRecognizeSpeaker()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
            }
        }
    }

    private bool called = false;
    public void ReceiveMicrophoneData(ArraySegment<float> buffer, WaveFormat format, string microphoneID, AudioClip recording)
    {
        if (called)
        {
            return;
        }

        called = true;
        Runnable.Run(RecordingHandler(microphoneID, recording, format.SampleRate));
    }

    public void Reset()
    {
        //throw new NotImplementedException();
    }
}
