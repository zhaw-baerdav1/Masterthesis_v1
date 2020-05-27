
using IBM.Watson.NaturalLanguageUnderstanding.V1;
using IBM.Watson.NaturalLanguageUnderstanding.V1.Model;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Cloud.SDK;

//responsible for analyzing text (emotion, sentiment, etc.)
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
    
    //create service on start
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
        //prepare features to be analyzed
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

        //prepare input and call service
        service.Analyze(
            callback: OnAnalyzeResponse,
            features: features,
            text: textToAnalyse,
            language: "en"
        );

        yield return null;
    }

    //if response has been recevied
    private void OnAnalyzeResponse(DetailedResponse<AnalysisResults> response, IBMError error)
    {
        //if analysis result is empty
        AnalysisResults analysisResults = response.Result;
        if (analysisResults == null)
        {
            ResultsField.text = "No Results found.";
            return;
        }

        //if emotionresult is empty
        EmotionResult emotionResult = analysisResults.Emotion;
        if ( emotionResult == null)
        {
            ResultsField.text = "No Emotions found.";
            return;
        }

        //update visualisation on UI
        ResultsField.text = "Joy: " + emotionResult.Document.Emotion.Joy.Value;
        ResultsField.text += ", Sadness: " + emotionResult.Document.Emotion.Sadness.Value;
        ResultsField.text += ", Disgust: " + emotionResult.Document.Emotion.Disgust.Value;
        ResultsField.text += ", Anger: " + emotionResult.Document.Emotion.Anger.Value;
        ResultsField.text += ", Fear: " + emotionResult.Document.Emotion.Fear.Value;

        //if sentiment result is empty
        SentimentResult sentimentResult = analysisResults.Sentiment;
        if (sentimentResult == null)
        {
            ResultsField.text = " - No Sentiment found.";
            return;
        }

        //update visualisation on UI
        DocumentSentimentResults documentSentimentResults = sentimentResult.Document;
        ResultsField.text += " - Sentiment: " + documentSentimentResults.Label + " - " + documentSentimentResults.Score;

        //prepare data transfer object
        Emotion emotion = new Emotion();
        emotion.setEmotionScores(emotionResult.Document.Emotion);

        //trigger emotion on character
        EmotionList.HandleNewEmotion(emotion);
    }
}
