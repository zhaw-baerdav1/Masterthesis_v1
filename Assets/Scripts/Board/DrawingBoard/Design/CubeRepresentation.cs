using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRepresentation : MonoBehaviour
{
    private CubeDefinition cubeDefinition;

    public void AttachToDrawingBoard(Transform panelTransform)
    {
        transform.SetParent(panelTransform);
    }

    public void Initialize(CubeDefinition _cubeDefinition)
    {
        cubeDefinition = _cubeDefinition;

        transform.localRotation = Quaternion.identity;
        transform.localPosition = cubeDefinition.getPosition();
        transform.localScale = new Vector3(0.5f, 0.07f, 1.5f);

        TextMesh[] textMeshes = GetComponentsInChildren<TextMesh>();
        foreach (TextMesh textMesh in textMeshes)
        {
            textMesh.text = cubeDefinition.getNaming();
        }
    }
}
