using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public static void TriggerNewCubeDefinition(CubeDefinition cubeDefinition)
    {
        int newCubeDefinitionId = cubeDefinitionList.Count + 1;
        cubeDefinition.id = newCubeDefinitionId;

        OnNewCubeDefinition(cubeDefinition);
    }
    public static void AddNewCubeDefinitionList(CubeDefinition cubeDefinition)
    {
        cubeDefinitionList.Add(cubeDefinition);

        OnNewCubeDefinitionList(cubeDefinitionList);
    }

    public static void CubeLookAt(long cubeDefinitionId)
    {
        if (cubeDefinitionId.Equals(selectedCubeDefinitionId))
        {
            return;
        }

        lookAtCubeDefinitionId = cubeDefinitionId;

        OnCubeLookAt(lookAtCubeDefinitionId);
    }

    public static void CubeLookAway()
    {
        if (lookAtCubeDefinitionId.Equals(-1))
        {
            return;
        }

        if (lookAtCubeDefinitionId.Equals(selectedCubeDefinitionId))
        {
            lookAtCubeDefinitionId = -1;
            return;
        }

        OnCubeLookAway(lookAtCubeDefinitionId);
        lookAtCubeDefinitionId = -1;
    }

    public static void CubeSelection()
    {
        TriggerPotentialArrowCreation();

        if (!selectedCubeDefinitionId.Equals(-1))
        {
            OnCubeDeselected(selectedCubeDefinitionId);

            if (lookAtCubeDefinitionId.Equals(selectedCubeDefinitionId))
            {
                selectedCubeDefinitionId = -1;
                return;
            }
            
            selectedCubeDefinitionId = -1;
        }


        if (!lookAtCubeDefinitionId.Equals(-1))
        {
            selectedCubeDefinitionId = lookAtCubeDefinitionId;

            OnCubeSelected(selectedCubeDefinitionId);
        }
    }

    private static void TriggerPotentialArrowCreation()
    {
        if (!selectedCubeDefinitionId.Equals(-1) && !lookAtCubeDefinitionId.Equals(-1) && !lookAtCubeDefinitionId.Equals(selectedCubeDefinitionId))
        {
            ArrowList.TriggerNewArrowDefinition(selectedCubeDefinitionId, lookAtCubeDefinitionId);
        }
    }

    public static void CubeMoveUp()
    {
        if (selectedCubeDefinitionId.Equals(-1))
        {
            return;
        }

        OnCubeMoveUp(selectedCubeDefinitionId);
    }

    public static void CubeMoveDown()
    {
        if (selectedCubeDefinitionId.Equals(-1))
        {
            return;
        }

        OnCubeMoveDown(selectedCubeDefinitionId);
    }

    public static void CubeMoveLeft()
    {
        if (selectedCubeDefinitionId.Equals(-1))
        {
            return;
        }

        OnCubeMoveLeft(selectedCubeDefinitionId);
    }

    public static void CubeMoveRight()
    {
        if (selectedCubeDefinitionId.Equals(-1))
        {
            return;
        }

        OnCubeMoveRight(selectedCubeDefinitionId);
    }

    public static void TriggerCubeChange(long id, Vector3 position)
    {
        CubeDefinition cubeDefinition = GetCubeDefinitionById(id);
        cubeDefinition.position = position;

        OnTriggerCubeChange(cubeDefinition);
    }

    public static void TriggerCubeChange(string naming)
    {
        CubeDefinition cubeDefinition = GetCubeDefinitionById(selectedCubeDefinitionId);
        cubeDefinition.naming = naming;

        OnTriggerCubeChange(cubeDefinition);
    }

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

    public static void CubeChangeCompleted(CubeDefinition cubeDefinition)
    {
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

        OnCubeChangeCompleted();
        OnNewCubeDefinitionList(cubeDefinitionList);
    }
}
