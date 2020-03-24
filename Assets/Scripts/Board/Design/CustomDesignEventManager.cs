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
        CubeList.OnNewCubeDefinitionList += CubeList_OnNewCubeDefinitionList;
    }

    private void CubeList_OnNewCubeDefinitionList(List<CubeDefinition> cubeList)
    {
        if (!drawingBoard.isSpawnLocationFree())
        {
            return;
        }

        drawingBoard.removeCubes();
        drawingBoard.drawNewCubeList(cubeList);
    }
}
