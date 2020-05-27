using IBM.Watson.ToneAnalyzer.V3;
using IBM.Watson.ToneAnalyzer.V3.Model;
using IBM.Cloud.SDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using System;

//responsible for tone analysis of IBM
public class ToneAnalyzer : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string iamApikey;
    [Tooltip("The service URL (optional). This defaults to \"https://gateway.watsonplatform.net/assistant/api\"")]
    [SerializeField]
    private string serviceUrl;
    [Tooltip("The version date with which you would like to use the service in the form YYYY-MM-DD.")]
    [SerializeField]
    private string versionDate;
    [Tooltip("Text field to display the results of text analysis.")]
    public TextMesh ResultsField;
    #endregion

    private ToneAnalyzerService service;        

    private void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(iamApikey))
        {
            throw new IBMException("Plesae provide IAM ApiKey for the service.");
        }

        //  Create credential and instantiate service
        IamAuthenticator authenticator = new IamAuthenticator(apikey: iamApikey);

        //  Wait for tokendata
        while (!authenticator.CanAuthenticate())
            yield return null;

        service = new ToneAnalyzerService(versionDate, authenticator);
        service.SetServiceUrl(serviceUrl);
    }

    public IEnumerator Analyze(string textToAnalyze)
    {
        ToneInput toneInput = new ToneInput()
        {
            Text = textToAnalyze
        };

        //prepare input on elements to be analyzed
        List<string> tones = new List<string>()
        {
            "emotion",
            "language",
            "social"
        };

        //call service
        service.Tone(callback: OnToneResponse, toneInput: toneInput, sentences: true, tones: tones, contentLanguage: "en", acceptLanguage: "en", contentType: "application/json");

        
        yield return null;
    }

    //if response has been recevied
    private void OnToneResponse(DetailedResponse<ToneAnalysis> response, IBMError error)
    {
        //if tone analysis is empty
        ToneAnalysis toneAnalysis = response.Result;
        if (toneAnalysis == null)
        {
            ResultsField.text = "No Tone found.";
            return;
        }

        //if document analysis is empty
        DocumentAnalysis documentAnalysis = toneAnalysis.DocumentTone;
        if (documentAnalysis == null || documentAnalysis.Tones.Count == 0)
        {
            ResultsField.text = "No Tone found.";
            return;
        }

        //update visualisation on found tones
        string resultText = "";
        foreach(ToneScore toneScore in documentAnalysis.Tones)
        {
            resultText += toneScore.ToneName + "/";
        }
        ResultsField.text = resultText;
    }
}

