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

using IBM.Watson.NaturalLanguageUnderstanding.V1;
using IBM.Watson.NaturalLanguageUnderstanding.V1.Model;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Cloud.SDK;



public class TextAnalyzer : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string iamApikey;
    [Tooltip("The service URL (optional). This defaults to \"https://gateway.watsonplatform.net/natural-language-understanding/api\"")]
    [SerializeField]
    private string serviceUrl;
    [Tooltip("The version date with which you would like to use the service in the form YYYY-MM-DD.")]
    [SerializeField]
    private string versionDate;
    [Tooltip("Text field to display the results of text analysis.")]
    public TextMesh ResultsField;
    #endregion

    private NaturalLanguageUnderstandingService service;
    
    private void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(iamApikey))
        {
            throw new IBMException("Please add IAM ApiKey to the Iam Apikey field in the inspector.");
        }

        IamAuthenticator authenticator = new IamAuthenticator(apikey: iamApikey);

        while (!authenticator.CanAuthenticate())
        {
            yield return null;
        }

        service = new NaturalLanguageUnderstandingService(versionDate, authenticator);
        service.SetServiceUrl(serviceUrl);
    }

    public IEnumerator Analyze(string textToAnalyse)
    {
        Features features = new Features()
        {
            Emotion = new EmotionOptions()
            {
                Document = true
            },
            Sentiment = new SentimentOptions()
            {
                Document = true
            }
        };

        service.Analyze(
            callback: OnAnalyzeResponse,
            features: features,
            text: textToAnalyse,
            language: "en"
        );

        yield return null;
    }

    private void OnAnalyzeResponse(DetailedResponse<AnalysisResults> response, IBMError error)
    {
        AnalysisResults analysisResults = response.Result;
        if (analysisResults == null)
        {
            ResultsField.text = "No Results found.";
            return;
        }
        EmotionResult emotionResult = analysisResults.Emotion;
        if ( emotionResult == null)
        {
            ResultsField.text = "No Emotions found.";
            return;
        }

        ResultsField.text = "Joy: " + emotionResult.Document.Emotion.Joy.Value;
        ResultsField.text += ", Sadness: " + emotionResult.Document.Emotion.Sadness.Value;
        ResultsField.text += ", Disgust: " + emotionResult.Document.Emotion.Disgust.Value;
        ResultsField.text += ", Anger: " + emotionResult.Document.Emotion.Anger.Value;
        ResultsField.text += ", Fear: " + emotionResult.Document.Emotion.Fear.Value;

        SentimentResult sentimentResult = analysisResults.Sentiment;
        if (sentimentResult == null)
        {
            ResultsField.text = " - No Sentiment found.";
            return;
        }

        DocumentSentimentResults documentSentimentResults = sentimentResult.Document;
        ResultsField.text += " - Sentiment: " + documentSentimentResults.Label + " - " + documentSentimentResults.Score;

        Emotion emotion = new Emotion();
        emotion.setEmotionScores(emotionResult.Document.Emotion);

        EmotionList.HandleNewEmotion(emotion);
    }
}
