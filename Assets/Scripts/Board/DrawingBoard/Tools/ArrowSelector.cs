using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelector : MonoBehaviour
{
    private bool isColliding = false;
    private bool arrowModeOn = false;

    public Material arrowModeOnMaterial;
    public Material arrowModeOffMaterial;

    private void Awake()
    {
        ArrowList.OnArrowModeChange += ArrowList_OnArrowModeChange;
    }

    private void OnDestroy()
    {
        ArrowList.OnArrowModeChange -= ArrowList_OnArrowModeChange;
    }

    private void ArrowList_OnArrowModeChange(bool on)
    {
        SwitchArrowMode(on);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        
        ArrowList.ChangeArrowMode(!arrowModeOn);

        StartCoroutine(Reset());
    }

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

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2);
        isColliding = false;
    }
}
