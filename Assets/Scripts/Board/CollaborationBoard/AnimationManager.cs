using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnimationManager : NetworkBehaviour
{
    enum AnimationType
    {
        Office,
        Standup
    }

    private static Dictionary<AnimationType, string> animationDictionary = new Dictionary<AnimationType, string>()
    {
        { AnimationType.Office, "Office" },
        { AnimationType.Standup, "Gesture F1" }
    };

    Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public void HasIdea()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (animator == null)
        {
            return;
        }

        CustomPlayer customPlayer = GetComponent<CustomPlayer>();
        int connectionId = customPlayer.connectionId;

        CmdPlayAnimation(connectionId, animationDictionary[AnimationType.Standup]);
    }

    public void HasNoIdea()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (animator == null)
        {
            return;
        }

        CustomPlayer customPlayer = GetComponent<CustomPlayer>();
        int connectionId = customPlayer.connectionId;

        CmdPlayAnimation(connectionId, animationDictionary[AnimationType.Office]);
    }

    [Command]
    private void CmdPlayAnimation(int connectionId, string animationName)
    {
        RpcPlayAnimation(connectionId, animationName);
    }

    [ClientRpc]
    private void RpcPlayAnimation(int connectionId, string animationName)
    {
        CustomPlayer customPlayer = GetComponent<CustomPlayer>();
        int _connectionId = customPlayer.connectionId;

        if (!connectionId.Equals(_connectionId))
        {
            return;
        }

        animator.Play(animationName);
    }
}
