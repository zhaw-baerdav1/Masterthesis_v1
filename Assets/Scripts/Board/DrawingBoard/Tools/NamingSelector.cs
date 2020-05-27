using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for the logic on the button of the naming
public class NamingSelector : MonoBehaviour
{
    //inidicate if button is should be available for pushing and triggering actions
    private bool isColliding = false;
    private bool isRecording = false;

    private long selectedCubeDefinitiondId = -1;

    //material to show if button is active or not
    public Material recordingActiveMaterial;
    public Material recordingInactiveMaterial;
    public Material recordingRecordingMaterial;

    //bind events
    private void Awake()
    {
        CubeList.OnCubeSelected += CubeList_OnCubeSelected;
        CubeList.OnCubeDeselected += CubeList_OnCubeDeselected;

        NamingList.OnRecordingModeChange += NamingList_OnRecordingModeChange;
    }

    //unbind events
    private void OnDestroy()
    {
        CubeList.OnCubeSelected -= CubeList_OnCubeSelected;
        CubeList.OnCubeDeselected -= CubeList_OnCubeDeselected;

        NamingList.OnRecordingModeChange -= NamingList_OnRecordingModeChange;
    }

    //triggered when cube has been selected
    private void CubeList_OnCubeSelected(long cubeDefinitionId)
    {
        selectedCubeDefinitiondId = cubeDefinitionId;
        ApplyState(recordingActiveMaterial);
    }

    //triggered when cube has been deselected
    private void CubeList_OnCubeDeselected(long obj)
    {
        selectedCubeDefinitiondId = -1;
        ApplyState(recordingInactiveMaterial);
    }

    //update visualisation of button depending on availability of mode
    private void NamingList_OnRecordingModeChange(bool on)
    {
        isRecording = on;
        if (on)
        {
            ApplyState(recordingRecordingMaterial);
        }
        else
        {
            ApplyState(recordingInactiveMaterial);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //do not execute if button shouldn't be available again
        if (isColliding) return;
        isColliding = true;

        //start coroutine to delay availability of button
        StartCoroutine(Reset());

        //do not continue if no cube is selected
        if (selectedCubeDefinitiondId.Equals(-1))
        {
            return;
        }

        //inform event system that mode has changed
        NamingList.ChangeRecordingMode(!isRecording);
    }

    //update material of button if it is active
    private void ApplyState(Material material)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1);
        isColliding = false;
    }
}
