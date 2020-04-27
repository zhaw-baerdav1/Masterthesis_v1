using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class DrawingBoard : MonoBehaviour
{

    [SerializeField]
    public CubeRepresentation cubePrefab;

    [SerializeField]
    public Material defaultMaterial;
    [SerializeField]
    public Material lookAtMaterial;
    [SerializeField]
    public Material selectedMaterial;

    private CubeDefinition movedCubeDefinition = null;

    public void Awake()
    {
        CubeList.OnNewCubeDefinitionList += CubeList_OnNewCubeDefinitionList;

        CubeList.OnCubeLookAt += CubeList_OnCubeLookAt;
        CubeList.OnCubeLookAway += CubeList_OnCubeLookAway;
        CubeList.OnCubeSelected += CubeList_OnCubeSelected;
        CubeList.OnCubeDeselected += CubeList_OnCubeDeselected;

        CubeList.OnCubeMoveUp += CubeList_OnCubeMoveUp;
        CubeList.OnCubeMoveDown += CubeList_OnCubeMoveDown;
        CubeList.OnCubeMoveLeft += CubeList_OnCubeMoveLeft;
        CubeList.OnCubeMoveRight += CubeList_OnCubeMoveRight;
    }

    private void OnDestroy()
    {
        CubeList.OnNewCubeDefinitionList -= CubeList_OnNewCubeDefinitionList;

        CubeList.OnCubeLookAt -= CubeList_OnCubeLookAt;
        CubeList.OnCubeLookAway -= CubeList_OnCubeLookAway;
        CubeList.OnCubeSelected -= CubeList_OnCubeSelected;
        CubeList.OnCubeDeselected -= CubeList_OnCubeDeselected;

        CubeList.OnCubeMoveUp -= CubeList_OnCubeMoveUp;
        CubeList.OnCubeMoveDown -= CubeList_OnCubeMoveDown;
        CubeList.OnCubeMoveLeft -= CubeList_OnCubeMoveLeft;
        CubeList.OnCubeMoveRight -= CubeList_OnCubeMoveRight;
    }


    private void CubeList_OnNewCubeDefinitionList(List<CubeDefinition> cubeDefinitionList)
    {
        RemoveCubes();

        foreach (CubeDefinition cubeDefinition in cubeDefinitionList)
        {
            CubeRepresentation cube = Instantiate(cubePrefab);
            cube.setCubeDefinition(cubeDefinition);
            cube.AttachToDrawingBoard(transform);
            cube.Initialize();
        }
    }

    public void RemoveCubes()
    {
        var cubes = GetComponentsInChildren<CubeRepresentation>();
        foreach (var cube in cubes)
        {
            Destroy(cube.gameObject);
        }
    }

    private void CubeList_OnCubeLookAt(long cubeDefinitionId)
    {
        ApplyCubeHighlighting(cubeDefinitionId, lookAtMaterial);
    }

    private void CubeList_OnCubeLookAway(long cubeDefinitionId)
    {
        ApplyCubeHighlighting(cubeDefinitionId, defaultMaterial);
    }

    private void CubeList_OnCubeSelected(long cubeDefinitionId)
    {
        ApplyCubeHighlighting(cubeDefinitionId, selectedMaterial);
    }

    private void CubeList_OnCubeDeselected(long cubeDefinitionId)
    {
        ApplyCubeHighlighting(cubeDefinitionId, defaultMaterial);

        if(movedCubeDefinition != null)
        {
            CubeList.TriggerCubeChange(movedCubeDefinition);
            movedCubeDefinition = null;
        }
    }

    public void ApplyCubeHighlighting(long cubeDefinitionId, Material material)
    {
        CubeRepresentation[] cubes = GetComponentsInChildren<CubeRepresentation>();
        foreach (CubeRepresentation cube in cubes)
        {
            CubeDefinition cubeDefinition = cube.GetCubeDefinition();
            if (cubeDefinitionId.Equals(cubeDefinition.id)) { 
                cube.SetMaterial(material);
                break;
            }
        }
    }

    private void CubeList_OnCubeMoveUp(long cubeDefinitionId)
    {
        CubeRepresentation[] cubes = GetComponentsInChildren<CubeRepresentation>();
        foreach (CubeRepresentation cube in cubes)
        {
            CubeDefinition cubeDefinition = cube.GetCubeDefinition();
            if (cubeDefinitionId.Equals(cubeDefinition.id))
            {
                cube.MoveUp();
                movedCubeDefinition = cube.GetCubeDefinition();
                break;
            }
        }
    }

    private void CubeList_OnCubeMoveDown(long cubeDefinitionId)
    {
        CubeRepresentation[] cubes = GetComponentsInChildren<CubeRepresentation>();
        foreach (CubeRepresentation cube in cubes)
        {
            CubeDefinition cubeDefinition = cube.GetCubeDefinition();
            if (cubeDefinitionId.Equals(cubeDefinition.id))
            {
                cube.MoveDown();
                movedCubeDefinition = cube.GetCubeDefinition();
                break;
            }
        }
    }

    private void CubeList_OnCubeMoveLeft(long cubeDefinitionId)
    {
        CubeRepresentation[] cubes = GetComponentsInChildren<CubeRepresentation>();
        foreach (CubeRepresentation cube in cubes)
        {
            CubeDefinition cubeDefinition = cube.GetCubeDefinition();
            if (cubeDefinitionId.Equals(cubeDefinition.id))
            {
                cube.MoveLeft();
                movedCubeDefinition = cube.GetCubeDefinition();
                break;
            }
        }
    }

    private void CubeList_OnCubeMoveRight(long cubeDefinitionId)
    {
        CubeRepresentation[] cubes = GetComponentsInChildren<CubeRepresentation>();
        foreach (CubeRepresentation cube in cubes)
        {
            CubeDefinition cubeDefinition = cube.GetCubeDefinition();
            if (cubeDefinitionId.Equals(cubeDefinition.id))
            {
                cube.MoveRight();
                movedCubeDefinition = cube.GetCubeDefinition();
                break;
            }
        }
    }
}
