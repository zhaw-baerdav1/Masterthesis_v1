using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for the logic on the button of the arrow
public class ArrowSelector : MonoBehaviour
{
    //inidicate if button is should be available for pushing and triggering actions
    private bool isColliding = false;
    private bool arrowModeOn = false;

    //material to show if button is active or not
    public Material arrowModeOnMaterial;
    public Material arrowModeOffMaterial;

    //bind events
    private void Awake()
    {
        ArrowList.OnArrowModeChange += ArrowList_OnArrowModeChange;
    }

    //unbind events
    private void OnDestroy()
    {
        ArrowList.OnArrowModeChange -= ArrowList_OnArrowModeChange;
    }

    //if arrow mode is active
    private void ArrowList_OnArrowModeChange(bool on)
    {
        SwitchArrowMode(on);
    }

    private void OnTriggerEnter(Collider other)
    {
        //do not execute if button shouldn't be available again
        if (isColliding) return;
        isColliding = true;
        
        //inform event system of arrow mode change
        ArrowList.ChangeArrowMode(!arrowModeOn);

        //start coroutine to delay availability of button
        StartCoroutine(Reset());
    }

    //update material of button if it is active
    private void SwitchArrowMode(bool on)
    {
        arrowModeOn = on;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (on)
        {
            meshRenderer.material = arrowModeOnMaterial;
            return;
        }

        meshRenderer.material = arrowModeOffMaterial;
    }

    //responsible for delaying the availability of the button
    IEnumerator Reset()
    {
        //wait until availability is activated again
        yield return new WaitForSeconds(2);
        isColliding = false;
    }
}
