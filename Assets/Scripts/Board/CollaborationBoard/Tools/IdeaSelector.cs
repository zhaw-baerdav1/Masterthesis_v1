using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeaSelector : MonoBehaviour
{
    private bool isColliding = false;
    private bool isIdeaOn = false;

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

        isIdeaOn = !isIdeaOn;

        if (isIdeaOn)
        {
            animationManager.HasIdea();
            return;
        }

        animationManager.HasNoIdea();

    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1);
        isColliding = false;
    }
}
