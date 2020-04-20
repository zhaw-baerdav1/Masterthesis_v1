using System.Collections;
using UnityEngine;
using Dissonance;

namespace CrazyMinnow.SALSA.DissonanceLink
{
    [AddComponentMenu( "Crazy Minnow Studio/SALSA LipSync/Add-ons/SalsaDissonanceLink" )]
    public class SalsaDissonanceLink : MonoBehaviour
    {
        // RELEASE NOTES & TODO ITEMS:
        //    2.0.0-BETA : Initial release for SALSA LipSync v2.
        // ==========================================================================
        // PURPOSE: This script connects output streams from Dissonance Voice Chat
        //    to SALSA's ananlysisValue property. NOTE: for Dissonance support,
        //    please contact Placeholder Software. For the latest information
        //    about SalsaDissonanceLink, visit crazyminnowstudio.com.
        // ========================================================================================
        // LOCATION OF FILES:
        //		Assets\Crazy Minnow Studio\Addons\SalsaDissonanceLink
        //		Assets\Crazy Minnow Studio\Examples\Scenes      (if applicable)
        //		Assets\Crazy Minnow Studio\Examples\Scripts     (if applicable)
        // ========================================================================================
        // INSTRUCTIONS:
        //		(visit https://crazyminnowstudio.com/docs/salsa-lip-sync/ for the latest info)
        //		To extend/modify these files, copy their contents to a new set of files and
        //		use a different namespace to ensure there are no scoping conflicts if/when this
        //		add-on is updated.
        // ========================================================================================
        // SUPPORT: Contact assetsupport@crazyminnow.com. Provide:
        //		1) your purchase email and invoice number
        //		2) version numbers (OS, Unity, SALSA, etc.)
        //		3) full details surrounding the problem you are experiencing.
        //		4) relevant information for what you have tried/implemented.
        //		NOTE: Support is only provided for Crazy Minnow Studio products with valid
        //			proof of purchase.
        // ========================================================================================
        // KNOWN ISSUES: none.
        // ==========================================================================
        // DISCLAIMER: While every attempt has been made to ensure the safe content
        //    and operation of these files, they are provided as-is, without
        //    warranty or guarantee of any kind. By downloading and using these
        //    files you are accepting any and all risks associated and release
        //    Crazy Minnow Studio, LLC of any and all liability.
        // ==========================================================================


        public bool isDebug = false;                // display debug messages?
        public bool useLocalLipSync = false;        // process local lipsync if desired
        [Range(0f, 10f)] public float amplifyMultipleExperimental = 1.0f;

        private VoicePlayerState playerState;       // provides SALSA with audio data (via ARV)
        private DissonanceComms dissonanceComms;    // allows reference to spawned player/audio objects
        private IDissonancePlayer dissonancePlayer; // reference to the player for discovery
        private Salsa salsa;                    // link up and feed SALSA instance average data
        private IEnumerator coroAudioSourceLinkage; // coro pointer (best-practice for GC reduction)
        private const float PollTimer = .5f;        // how often the coro rechecks for playerState discovery

        // using OnEnable() since it's probably necessary to re-process if the player is disabled/enabled for any reason
        private void OnEnable()
        {
            // link up required components
            salsa = GetComponent<Salsa>();

            dissonancePlayer = GetComponent<IDissonancePlayer>();
            dissonanceComms = FindObjectOfType<DissonanceComms>();

            if ( !salsa )
                Debug.LogError( "[" + GetType().Name + "] SALSA was not found on the player object." );
            if ( dissonancePlayer == null )
                Debug.LogError( "[" + GetType().Name + "] an IDissonancePlayer component was not found on the player object." );
            if ( dissonanceComms == null )
                Debug.LogError( "[" + GetType().Name + "] the DissonanceComms component was not found in the scene." );


            // start coroutine to wait for Dissonance to register the network player
            if ( coroAudioSourceLinkage != null )
            {
                StopCoroutine(coroAudioSourceLinkage);
            }
            coroAudioSourceLinkage = WaitSalsaDissonanceLink();
            StartCoroutine(coroAudioSourceLinkage);

        }

        // bypass SALSA's internal audio processing and direct-feed values to the triggering engine
        // (LateUpdate used to avoid animation conflicts with Mechanim, etc.)
        // NOTE: SALSA's linked AudioSource component must not be playing to direct-feed the data to SALSA's
        // trigger processing algorithm. We've tried to prevent that from happening in OnEnable().
        private void LateUpdate()
        {
            if ( playerState != null )
            {
                if ( dissonancePlayer.Type == NetworkPlayerType.Local && !useLocalLipSync )
                    return;     // Bail out: local player and lip-sync not desired

                // boosting VoicePlayerState.Amplitude by fractional exponent for better lip-sync dynamics
                salsa.analysisValue = playerState.Amplitude * amplifyMultipleExperimental;
            }
        }

        // linkage routine - wait for Dissonance to register the player and spawn audio components
        private IEnumerator WaitSalsaDissonanceLink()
        {
            // implement internal timer to avoid WaitForSeconds GC
            var timeCheck = Time.time;

            //Find the playerstate for this playerid
            while ( playerState == null )
            {
                if ( Time.time - timeCheck > PollTimer )
                {
                    if ( isDebug )
                        Debug.Log(string.Format("[" + GetType().Name + "] - Looking for {0} player audiosource", dissonancePlayer.PlayerId));

                    playerState = dissonanceComms.FindPlayer(dissonancePlayer.PlayerId);
                    timeCheck = Time.time;
                }

                yield return null;
            }

            if ( isDebug )
                Debug.Log("[" + GetType().Name + "] - SALSA and Dissonance are linked.");
        }
    }
}
