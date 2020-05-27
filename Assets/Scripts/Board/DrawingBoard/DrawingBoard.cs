using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

//responsible for handling activities of the player on the drawing board
public class DrawingBoard : MonoBehaviour
{

    [SerializeField]
    public CubeRepresentation cubePrefab;
    [SerializeField]
    public ArrowRepresentation arrowPrefab;

    [SerializeField]
    public Material defaultMaterial;
    [SerializeField]
    public Material lookAtMaterial;
    [SerializeField]
    public Material selectedMaterial;

    private CubeDefinition movedCubeDefinition = null;

    //bind all events to this component
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

        ArrowList.OnNewArrowDefinitionList += ArrowList_OnNewArrowDefinitionList;
    }

    //remove all binding events from component
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

        ArrowList.OnNewArrowDefinitionList -= ArrowList_OnNewArrowDefinitionList;
    }

    //triggered when new list of cubes is received
    private void CubeList_OnNewCubeDefinitionList(List<CubeDefinition> cubeDefinitionList)
    {
        //ensure all cubes are resetted first
        RemoveCubes();

        //loop though cubes and apply them to drawing board
        foreach (CubeDefinition cubeDefinition in cubeDefinitionList)
        {
            CubeRepresentation cube = Instantiate(cubePrefab);
            cube.setCubeDefinition(cubeDefinition);
            cube.AttachToDrawingBoard(transform);
            cube.Initialize();
        }

        //update arrows between them
        ArrowList.RefreshArrowDefinitionList();
    }

    //responsible for resetting all existing cubes
    public void RemoveCubes()
    {
        //destroys all cubes on the drawing board
        var cubes = GetComponentsInChildren<CubeRepresentation>();
        foreach (var cube in cubes)
        {
            Destroy(cube.gameObject);
        }
    }

    //triggered when player looks at the cube
    private void CubeList_OnCubeLookAt(long cubeDefinitionId)
    {
        ApplyCubeHighlighting(cubeDefinitionId, lookAtMaterial);
    }

    //triggered when player not anymore looks at the cube
    private void CubeList_OnCubeLookAway(long cubeDefinitionId)
    {
        ApplyCubeHighlighting(cubeDefinitionId, defaultMaterial);
    }

    //triggered when the cube is selected by player
    private void CubeList_OnCubeSelected(long cubeDefinitionId)
    {
        ApplyCubeHighlighting(cubeDefinitionId, selectedMaterial);
    }

    //triggered when cube is deselected by player
    private void CubeList_OnCubeDeselected(long cubeDefinitionId)
    {
        ApplyCubeHighlighting(cubeDefinitionId, defaultMaterial);

        //apply change of potential movement to all participants
        if(movedCubeDefinition != null)
        {
            CubeList.TriggerCubeChange(movedCubeDefinition.id, movedCubeDefinition.position);

            movedCubeDefinition = null;
        }
    }

    //responsible for show respective highlight on cube
    public void ApplyCubeHighlighting(long cubeDefinitionId, Material material)
    {
        //apply new material to cube if found in existing list
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

    //triggered when the cube is moved up
    private void CubeList_OnCubeMoveUp(long cubeDefinitionId)
    {
        //find respective cube on client side and move it
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

    //triggered when the cube is moved up
    private void CubeList_OnCubeMoveDown(long cubeDefinitionId)
    {
        //find respective cube on client side and move it
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

    //triggered when the cube is moved left
    private void CubeList_OnCubeMoveLeft(long cubeDefinitionId)
    {
        //find respective cube on client side and move it
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

    //triggered when the cube is moved right
    private void CubeList_OnCubeMoveRight(long cubeDefinitionId)
    {
        //find respective cube on client side and move it
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

    //triggered when list of arrows is updated
    private void ArrowList_OnNewArrowDefinitionList(List<ArrowDefinition> arrowDefinitionList)
    {
        //ensure all existing arrows are removed from drawing board
        RemoveArrows();

        //apply complete list of arrows on the drawing board
        foreach (ArrowDefinition arrowDefinition in arrowDefinitionList)
        {
            ArrowRepresentation arrow = Instantiate(arrowPrefab);
            arrow.SetArrowDefinition(arrowDefinition);
            arrow.AttachToDrawingBoard(transform);
            arrow.Initialize();
        }
    }

    //responsible for resetting all existing arrows
    public void RemoveArrows()
    {
        //destroys all arrows on the drawing board
        var arrows = GetComponentsInChildren<ArrowRepresentation>();
        foreach (var arrow in arrows)
        {
            Destroy(arrow.gameObject);
        }
    }
}
