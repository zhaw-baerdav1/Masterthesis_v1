using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for the logic on the button expressing an agreement
public class AgreeSelector : MonoBehaviour
{
    //inidicate if button is should be available for pushing and triggering actions
    private bool isColliding = false;
    private bool isAgreeOn = false;

    //material to show if button is active or not
    public Material onMaterial;
    public Material offMaterial;

    private void Awake()
    {
        AnimationEventSystem.OnResetAgreeAnimations += AnimationEventSystem_OnResetAgreeAnimations;
    }

    private void OnDestroy()
    {
        AnimationEventSystem.OnResetAgreeAnimations -= AnimationEventSystem_OnResetAgreeAnimations;
    }

    private void AnimationEventSystem_OnResetAgreeAnimations()
    {
        isAgreeOn = false;

        SwitchMode(isAgreeOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        //do not execute if button shouldn't be available again
        if (isColliding) return;
        isColliding = true;

        //start coroutine to delay availability of button
        StartCoroutine(Reset());

        //do not continue if animationmanager is not existing
        AnimationManager animationManager = other.gameObject.GetComponentInParent<AnimationManager>();
        if (animationManager == null)
        {
            return;
        }

        //ensure no other animations are active
        AnimationEventSystem.ResetIdeaAnimations();
        
        //update internal status of animation
        isAgreeOn = !isAgreeOn;
        SwitchMode(isAgreeOn);

        //activate animation if it should be on
        if (isAgreeOn)
        {
            animationManager.SetAgreeAnimation();
            return;
        }

        //reset animation if it should be off
        animationManager.SetDefaultAnimation();

    }

    //responsible to update shown material to player
    private void SwitchMode(bool on)
    {
        //if active, apply "on" material. if not apply "off" material
        MeshRenderer meshRenderer = transform.parent.GetComponent<MeshRenderer>();
        if (on)
        {
            meshRenderer.material = onMaterial;
            return;
        }

        meshRenderer.material = offMaterial;
    }

    //responsible for delaying the availability of the button
    IEnumerator Reset()
    {
        //wait until availability is activated again
        yield return new WaitForSeconds(1);
        isColliding = false;
    }
}
