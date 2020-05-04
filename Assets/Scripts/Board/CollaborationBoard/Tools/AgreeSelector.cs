using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgreeSelector : MonoBehaviour
{
    private bool isColliding = false;
    private bool isAgreeOn = false;

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
        if (isColliding) return;
        isColliding = true;
        StartCoroutine(Reset());

        AnimationManager animationManager = other.gameObject.GetComponentInParent<AnimationManager>();
        if (animationManager == null)
        {
            return;
        }

        AnimationEventSystem.ResetIdeaAnimations();
        
        isAgreeOn = !isAgreeOn;
        SwitchMode(isAgreeOn);

        if (isAgreeOn)
        {
            animationManager.SetAgreeAnimation();
            return;
        }

        animationManager.SetDefaultAnimation();

    }

    private void SwitchMode(bool on)
    {
        MeshRenderer meshRenderer = transform.parent.GetComponent<MeshRenderer>();
        if (on)
        {
            meshRenderer.material = onMaterial;
            return;
        }

        meshRenderer.material = offMaterial;
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1);
        isColliding = false;
    }
}
