using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnimationManager : NetworkBehaviour
{
    public string defaultAnimationName;
    public string ideaAnimationName;
    public string agreeAnimationName;

    Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public void SetIdeaAnimation()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (animator == null)
        {
            return;
        }

        CmdPlayAnimation(ideaAnimationName);
    }

    public void SetAgreeAnimation()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (animator == null)
        {
            return;
        }

        CmdPlayAnimation(agreeAnimationName);
    }

    public void SetDefaultAnimation()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (animator == null)
        {
            return;
        }

        CmdPlayAnimation(defaultAnimationName);
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
