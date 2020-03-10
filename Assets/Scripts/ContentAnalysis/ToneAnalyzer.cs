/**
* Copyright 2019 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

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

        List<string> tones = new List<string>()
        {
            "emotion",
            "language",
            "social"
        };
        service.Tone(callback: OnToneResponse, toneInput: toneInput, sentences: true, tones: tones, contentLanguage: "en", acceptLanguage: "en", contentType: "application/json");

        
        yield return null;
    }

    private void OnToneResponse(DetailedResponse<ToneAnalysis> response, IBMError error)
    {
        ToneAnalysis toneAnalysis = response.Result;
        if (toneAnalysis == null)
        {
            ResultsField.text = "No Tone found.";
            return;
        }
        DocumentAnalysis documentAnalysis = toneAnalysis.DocumentTone;
        if (documentAnalysis == null || documentAnalysis.Tones.Count == 0)
        {
            ResultsField.text = "No Tone found.";
            return;
        }

        string resultText = "";
        foreach(ToneScore toneScore in documentAnalysis.Tones)
        {
            resultText += toneScore.ToneName + "/";
        }
        ResultsField.text = resultText;
    }
}

