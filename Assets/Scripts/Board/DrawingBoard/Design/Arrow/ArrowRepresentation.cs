using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for representing arrow on UI
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

    //responsible to attach arrow to drawing board
    public void AttachToDrawingBoard(Transform panelTransform)
    {
        transform.SetParent(panelTransform);
    }

    //initial call to setup prefab
    public void Initialize()
    {
        ApplyTransform();
    }
    
    //responsible for preparing arrow for visualistion
    public void ApplyTransform()
    {
        ArrowDefinition arrowDefinition = GetArrowDefinition();

        Vector3 startCubeWorldPosition = Vector3.zero;
        Vector3 endCubeWorldPosition = Vector3.zero;

        //identify all starting and ending position of the cubes
        CubeRepresentation[] allCubeRepresentationList = transform.parent.GetComponentsInChildren<CubeRepresentation>();
        foreach(CubeRepresentation cubeRepresentation in allCubeRepresentationList)
        {
            //identify the to-be-linked cubes
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

        //update line renderer with new coordinates
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.material.SetTextureScale("_MainTex", new Vector2(5f, 1f));

        lineRenderer.SetPosition(0, startCubeWorldPosition);
        lineRenderer.SetPosition(1, endCubeWorldPosition);
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }
}
