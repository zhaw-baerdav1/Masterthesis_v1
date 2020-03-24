using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRepresentation : MonoBehaviour
{
    private CubeDefinition cubeDefinition;

    public void Initialize(CubeDefinition cubeDefinition, Transform panelTransform)
    {
        this.cubeDefinition = cubeDefinition;

        transform.SetParent(panelTransform);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = cubeDefinition.getPosition();
        transform.localScale = new Vector3(0.5f, 0.05f, 1);
    }
}
