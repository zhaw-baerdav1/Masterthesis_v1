using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for handling events on cubes
public class CubeList : MonoBehaviour
{
    public static event Action<CubeDefinition> OnNewCubeDefinition = delegate { };

    public static event Action<List<CubeDefinition>> OnNewCubeDefinitionList = delegate { };
    private static List<CubeDefinition> cubeDefinitionList = new List<CubeDefinition>();

    public static event Action<long> OnCubeLookAt = delegate { };
    public static event Action<long> OnCubeLookAway = delegate { };
    private static long lookAtCubeDefinitionId = -1;

    public static event Action<long> OnCubeSelected = delegate { };
    public static event Action<long> OnCubeDeselected = delegate { };
    private static long selectedCubeDefinitionId = -1;

    public static event Action<long> OnCubeMoveUp = delegate { };
    public static event Action<long> OnCubeMoveDown = delegate { };
    public static event Action<long> OnCubeMoveLeft = delegate { };
    public static event Action<long> OnCubeMoveRight = delegate { };

    public static event Action<CubeDefinition> OnTriggerCubeChange = delegate { };
    public static event Action OnCubeChangeCompleted = delegate { };

    //triggers the addition of new cube
    public static void TriggerNewCubeDefinition(CubeDefinition cubeDefinition)
    {
        //set new ID
        int newCubeDefinitionId = cubeDefinitionList.Count + 1;
        cubeDefinition.id = newCubeDefinitionId;

        //trigger all listeners
        OnNewCubeDefinition(cubeDefinition);
    }

    //triggers update of new list of cubes
    public static void AddNewCubeDefinitionList(CubeDefinition cubeDefinition)
    {
        cubeDefinitionList.Add(cubeDefinition);

        //trigger all listeners
        OnNewCubeDefinitionList(cubeDefinitionList);
    }

    //triggers if a cube is looked at
    public static void CubeLookAt(long cubeDefinitionId)
    {
        //do not trigger if looked at cube is already selected
        if (cubeDefinitionId.Equals(selectedCubeDefinitionId))
        {
            return;
        }

        lookAtCubeDefinitionId = cubeDefinitionId;

        //trigger all listeners
        OnCubeLookAt(lookAtCubeDefinitionId);
    }

    //triggers if a cube is not anymore looked at
    public static void CubeLookAway()
    {
        //do not continue if no cube is looked at
        if (lookAtCubeDefinitionId.Equals(-1))
        {
            return;
        }

        //if looked at cube and selected cube are the, reset looked at cube ID
        if (lookAtCubeDefinitionId.Equals(selectedCubeDefinitionId))
        {
            lookAtCubeDefinitionId = -1;
            return;
        }

        //trigger all listeners
        OnCubeLookAway(lookAtCubeDefinitionId);
        lookAtCubeDefinitionId = -1;
    }

    //triggers if a cube should be selected
    public static void CubeSelection()
    {
        //if an arrow should be created
        TriggerPotentialArrowCreation();

        //if a cube is already selected
        if (!selectedCubeDefinitionId.Equals(-1))
        {
            //trigger all listeners
            OnCubeDeselected(selectedCubeDefinitionId);

            //if the looked at cube is already selected, reset selected cube ID
            if (lookAtCubeDefinitionId.Equals(selectedCubeDefinitionId))
            {
                selectedCubeDefinitionId = -1;
                return;
            }
            
            selectedCubeDefinitionId = -1;
        }

        //if there is a looked at cube, select it
        if (!lookAtCubeDefinitionId.Equals(-1))
        {
            selectedCubeDefinitionId = lookAtCubeDefinitionId;

            //trigger all listeners
            OnCubeSelected(selectedCubeDefinitionId);
        }
    }

    //responsible for verifying and trigger arrow creations
    private static void TriggerPotentialArrowCreation()
    {
        if (!selectedCubeDefinitionId.Equals(-1) && !lookAtCubeDefinitionId.Equals(-1) && !lookAtCubeDefinitionId.Equals(selectedCubeDefinitionId))
        {
            ArrowList.TriggerNewArrowDefinition(selectedCubeDefinitionId, lookAtCubeDefinitionId);
        }
    }

    //triggers the cube to move up
    public static void CubeMoveUp()
    {
        //do not execute if no cube is selected
        if (selectedCubeDefinitionId.Equals(-1))
        {
            return;
        }

        //trigger all listeners
        OnCubeMoveUp(selectedCubeDefinitionId);
    }

    //triggers the cube to move up
    public static void CubeMoveDown()
    {
        //do not execute if no cube is selected
        if (selectedCubeDefinitionId.Equals(-1))
        {
            return;
        }

        //trigger all listeners
        OnCubeMoveDown(selectedCubeDefinitionId);
    }

    //triggers the cube to move up
    public static void CubeMoveLeft()
    {
        //do not execute if no cube is selected
        if (selectedCubeDefinitionId.Equals(-1))
        {
            return;
        }

        //trigger all listeners
        OnCubeMoveLeft(selectedCubeDefinitionId);
    }

    //triggers the cube to move up
    public static void CubeMoveRight()
    {
        //do not execute if no cube is selected
        if (selectedCubeDefinitionId.Equals(-1))
        {
            return;
        }

        //trigger all listeners
        OnCubeMoveRight(selectedCubeDefinitionId);
    }

    //triggers if a cube position has changed
    public static void TriggerCubeChange(long id, Vector3 position)
    {
        CubeDefinition cubeDefinition = GetCubeDefinitionById(id);
        cubeDefinition.position = position;

        //trigger all listeners
        OnTriggerCubeChange(cubeDefinition);
    }

    //triggers if a cube naming has changed
    public static void TriggerCubeChange(string naming)
    {
        CubeDefinition cubeDefinition = GetCubeDefinitionById(selectedCubeDefinitionId);
        cubeDefinition.naming = naming;

        //deselect cube if required
        CubeSelection();

        //trigger all listeners
        OnTriggerCubeChange(cubeDefinition);
    }

    //find cube by id in complete cube list
    private static CubeDefinition GetCubeDefinitionById(long id)
    {
        foreach(CubeDefinition cubeDefinition in cubeDefinitionList)
        {
            if (cubeDefinition.id.Equals(id))
            {
                return cubeDefinition;
            }
        }

        return null;
    }

    //triggered when cube changes have been successfully received by server
    public static void CubeChangeCompleted(CubeDefinition cubeDefinition)
    {
        //ensure cubes are in sync on all clients
        for (int i = 0; i < cubeDefinitionList.Count; i++)
        {
            CubeDefinition _cubeDefinition = cubeDefinitionList[i];
            if (!cubeDefinition.id.Equals(_cubeDefinition.id))
            {
                continue;
            }

            cubeDefinitionList[i].id = cubeDefinition.id;
            cubeDefinitionList[i].naming = cubeDefinition.naming;
            cubeDefinitionList[i].position = cubeDefinition.position;
        }

        //trigger all listeners
        OnCubeChangeCompleted();

        //trigger all listeners
        OnNewCubeDefinitionList(cubeDefinitionList);
    }
}
