using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRepresentation : MonoBehaviour
{
    private ArrowDefinition arrowDefinition;
    private LineRenderer lineRenderer;

    public ArrowDefinition GetArrowDefinition()
    {
        return arrowDefinition;
    }

    public void SetArrowDefinition(ArrowDefinition _arrowDefinition)
    {
        arrowDefinition = _arrowDefinition;
    }

    public void AttachToDrawingBoard(Transform panelTransform)
    {
        transform.SetParent(panelTransform);
    }

    public void Initialize()
    {
        ApplyTransform();
    }
    
    public void ApplyTransform()
    {
        ArrowDefinition arrowDefinition = GetArrowDefinition();

        Vector3 startCubeWorldPosition = Vector3.zero;
        Vector3 endCubeWorldPosition = Vector3.zero;

        CubeRepresentation[] allCubeRepresentationList = transform.parent.GetComponentsInChildren<CubeRepresentation>();
        foreach(CubeRepresentation cubeRepresentation in allCubeRepresentationList)
        {
            CubeDefinition cubeDefinition = cubeRepresentation.GetCubeDefinition();
            if (cubeDefinition.id.Equals(arrowDefinition.startCubeDefinitionId))
            {
                Vector3 startCubeLocalPosition = cubeDefinition.position;
                startCubeWorldPosition = cubeRepresentation.gameObject.transform.parent.localToWorldMatrix.MultiplyPoint(startCubeLocalPosition);
                continue;
            }

            if (cubeDefinition.id.Equals(arrowDefinition.endCubeDefinitionId))
            {
                Vector3 endCubeLocalPosition = cubeDefinition.position;
                endCubeWorldPosition = cubeRepresentation.gameObject.transform.parent.localToWorldMatrix.MultiplyPoint(endCubeLocalPosition);
                continue;
            }
        }

        startPosition = startCubeWorldPosition;
        endPosition = endCubeWorldPosition;

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.material.SetTextureScale("_MainTex", new Vector2(5f, 1f));

        lineRenderer.SetPosition(0, startCubeWorldPosition);
        lineRenderer.SetPosition(1, endCubeWorldPosition);
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }
}
