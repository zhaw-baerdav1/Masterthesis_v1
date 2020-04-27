using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRepresentation : MonoBehaviour
{
    private ArrowDefinition arrowDefinition;

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

        CubeDefinition startCubeDefinition = null;
        CubeDefinition endCubeDefinition = null;

        CubeRepresentation[] allCubeRepresentationList = transform.parent.GetComponentsInChildren<CubeRepresentation>();
        foreach(CubeRepresentation cubeRepresentation in allCubeRepresentationList)
        {
            CubeDefinition cubeDefinition = cubeRepresentation.GetCubeDefinition();
            if (cubeDefinition.id.Equals(arrowDefinition.startCubeDefinitionId))
            {
                startCubeDefinition = cubeDefinition;
                continue;
            }

            if (cubeDefinition.id.Equals(arrowDefinition.endCubeDefinitionId))
            {
                endCubeDefinition = cubeDefinition;
                continue;
            }
        }

        if ( startCubeDefinition == null || endCubeDefinition == null)
        {
            throw new NullReferenceException("One of the to-be-linked cubes has not been found.");
        }

        Vector3 startPos = startCubeDefinition.position;
        Vector3 endPos = endCubeDefinition.position;

        Vector3 midPointVector = (endPos + startPos) / 2;
        midPointVector.y = 0.001f;
        transform.localPosition = midPointVector;

        Vector3 relative = endPos - startPos;
        float maggy = relative.magnitude;

        transform.localScale = new Vector3((maggy / 2)*0.15f, 1f, 0.1f);

        //Quaternion rotationVector = Quaternion.LookRotation(relative);
        //rotationVector.z = 0;
        //rotationVector.w = 0;
        //transform.localRotation = rotationVector;

        //transform.rotation = new Quaternion(0,-90,0,0);

        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        q.z = 0;
        q.w = 0;
        transform.localRotation = q;
    }
}
