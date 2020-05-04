﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeaSelector : MonoBehaviour
{
    private bool isColliding = false;
    private bool isIdeaOn = false;

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
        if (isColliding) return;
        isColliding = true;
        StartCoroutine(Reset());

        AnimationManager animationManager = other.gameObject.GetComponentInParent<AnimationManager>();
        if (animationManager == null)
        {
            return;
        }
        
        AnimationEventSystem.ResetAgreeAnimations();

        isIdeaOn = !isIdeaOn;
        SwitchMode(isIdeaOn);

        if (isIdeaOn)
        {
            animationManager.SetIdeaAnimation();
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
