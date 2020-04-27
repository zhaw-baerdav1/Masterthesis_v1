using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingSelector : MonoBehaviour
{
    private bool isColliding = false;
    private bool isRecording = false;

    private long selectedCubeDefinitiondId = -1;

    public Material recordingActiveMaterial;
    public Material recordingInactiveMaterial;
    public Material recordingRecordingMaterial;

    private void Awake()
    {
        CubeList.OnCubeSelected += CubeList_OnCubeSelected;
        CubeList.OnCubeDeselected += CubeList_OnCubeDeselected;

        NamingList.OnRecordingModeChange += NamingList_OnRecordingModeChange;
    }

    private void OnDestroy()
    {
        CubeList.OnCubeSelected -= CubeList_OnCubeSelected;
        CubeList.OnCubeDeselected -= CubeList_OnCubeDeselected;

        NamingList.OnRecordingModeChange -= NamingList_OnRecordingModeChange;
    }

    private void CubeList_OnCubeSelected(long cubeDefinitionId)
    {
        selectedCubeDefinitiondId = cubeDefinitionId;
        ApplyState(recordingActiveMaterial);
    }

    private void CubeList_OnCubeDeselected(long obj)
    {
        selectedCubeDefinitiondId = -1;
        ApplyState(recordingInactiveMaterial);
    }

    private void NamingList_OnRecordingModeChange(bool on)
    {
        isRecording = on;
        if (on)
        {
            ApplyState(recordingRecordingMaterial);
        }
        else
        {
            ApplyState(recordingActiveMaterial);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        StartCoroutine(Reset());

        if (selectedCubeDefinitiondId.Equals(-1))
        {
            return;
        }

        NamingList.ChangeRecordingMode(!isRecording);
    }

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
