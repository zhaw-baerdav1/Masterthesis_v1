using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for the logic on the button expressing an idea
public class IdeaSelector : MonoBehaviour
{
    //inidicate if button is should be available for pushing and triggering actions
    private bool isColliding = false;
    private bool isIdeaOn = false;

    //material to show if button is active or not
    public Material onMaterial;
    public Material offMaterial;

    private void Awake()
    {
        AnimationEventSystem.OnResetIdeaAnimations += AnimationEventSystem_OnResetIdeaAnimations;
    }

    private void OnDestroy()
    {
        AnimationEventSystem.OnResetIdeaAnimations -= AnimationEventSystem_OnResetIdeaAnimations;
    }

    private void AnimationEventSystem_OnResetIdeaAnimations()
    {
        isIdeaOn = false;

        SwitchMode(isIdeaOn);
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
        AnimationEventSystem.ResetAgreeAnimations();

        //update internal status of animation
        isIdeaOn = !isIdeaOn;
        SwitchMode(isIdeaOn);

        //activate animation if it should be on
        if (isIdeaOn)
        {
            animationManager.SetIdeaAnimation();
            return;
        }

        //reset animation if it should be off
        animationManager.SetDefaultAnimation();

    }

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
