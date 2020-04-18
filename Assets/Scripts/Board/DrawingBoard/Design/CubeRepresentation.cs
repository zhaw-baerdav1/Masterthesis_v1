using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRepresentation : MonoBehaviour
{
    private CubeDefinition cubeDefinition;

    public CubeDefinition GetCubeDefinition()
    {
        return cubeDefinition;
    }

    public void setCubeDefinition(CubeDefinition _cubeDefinition)
    {
        cubeDefinition = _cubeDefinition;
    }

    public void AttachToDrawingBoard(Transform panelTransform)
    {
        transform.SetParent(panelTransform);
    }

    public void Initialize()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        transform.localRotation = Quaternion.identity;
        transform.localPosition = _cubeDefinition.position;
        transform.localScale = new Vector3(0.5f, 0.07f, 1.5f);

        TextMesh[] textMeshes = GetComponentsInChildren<TextMesh>();
        foreach (TextMesh textMesh in textMeshes)
        {
            textMesh.text = _cubeDefinition.naming;
        }
    }
}
