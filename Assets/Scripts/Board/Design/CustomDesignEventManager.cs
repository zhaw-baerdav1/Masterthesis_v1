using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDesignEventManager : MonoBehaviour
{
    [SerializeField]
    public DrawingBoard drawingBoard;

    private void Awake()
    {
        CubeList.OnUpdateCubeDefinitionList += CubeList_OnUpdateCubeDefinitionList;
    }

    private void CubeList_OnUpdateCubeDefinitionList(List<CubeDefinition> cubeList)
    {
        drawingBoard.removeCubes();
        drawingBoard.drawCubeList(cubeList);
    }
}
