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

        CmdPlayAnimation(animationDictionary[AnimationType.Standup]);
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

        CmdPlayAnimation(animationDictionary[AnimationType.Office]);
    }

    [Command]
    private void CmdPlayAnimation(string animationName)
    {
        RpcPlayAnimation(animationName);
    }

    [ClientRpc]
    private void RpcPlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
