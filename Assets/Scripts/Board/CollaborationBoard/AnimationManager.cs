using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//responsible for applying the animations on the character in unity
public class AnimationManager : NetworkBehaviour
{
    public string defaultAnimationName;
    public string ideaAnimationName;
    public string agreeAnimationName;

    Animator animator = null;

    private void Start()
    {
        //initiate animator on start of component
        animator = GetComponent<Animator>();
    }

    public override void OnStartLocalPlayer()
    {
        //ensure the behaviour is correctly setup
        base.OnStartLocalPlayer();
    }

    //is getting triggered when the player pushes the idea animation button
    public void SetIdeaAnimation()
    {
        //do not execute if its not the localplayer
        if (!isLocalPlayer)
        {
            return;
        }

        //do not execute if the animator is missing or not yet initiated
        if (animator == null)
        {
            return;
        }

        //fire command to server to trigger the animation
        CmdPlayAnimation(ideaAnimationName);
    }

    //is getting triggered when the player pushes the agree animation button
    public void SetAgreeAnimation()
    {
        //do not execute if its not the localplayer
        if (!isLocalPlayer)
        {
            return;
        }

        //do not execute if the animator is missing or not yet initiated
        if (animator == null)
        {
            return;
        }

        //fire command to server to trigger the animation
        CmdPlayAnimation(agreeAnimationName);
    }

    //is getting triggered when the player pushes the agree or idea animation button to deactivate it
    public void SetDefaultAnimation()
    {
        //do not execute if its not the localplayer
        if (!isLocalPlayer)
        {
            return;
        }

        //do not execute if the animator is missing or not yet initiated
        if (animator == null)
        {
            return;
        }

        //fire command to server to trigger the animation
        CmdPlayAnimation(defaultAnimationName);
    }

    //executed on server and tells all the clients that this player is execution an animation
    [Command]
    private void CmdPlayAnimation(string animationName)
    {
        RpcPlayAnimation(animationName);
    }

    //executed on server and ensure execution of animation via the unity animator
    [ClientRpc]
    private void RpcPlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
