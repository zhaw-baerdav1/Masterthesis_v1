using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowList : MonoBehaviour
{
    public static event Action<ArrowDefinition> OnNewArrowDefinition = delegate { };

    public static event Action<List<ArrowDefinition>> OnNewArrowDefinitionList = delegate { };
    private static List<ArrowDefinition> arrowDefinitionList = new List<ArrowDefinition>();

    public static event Action<bool> OnArrowModeChange = delegate { };
    public static bool arrowMode = false;


    public static void TriggerNewArrowDefinition(long startId, long endId)
    {
        if (!arrowMode)
        {
            return;
        }

        int newArrowDefinitionId = arrowDefinitionList.Count + 1;
        ArrowDefinition arrowDefinition = new ArrowDefinition(newArrowDefinitionId, startId, endId);
        
        OnNewArrowDefinition(arrowDefinition);
    }
    public static void AddNewArrowDefinitionList(ArrowDefinition arrowDefinition)
    {
        arrowDefinitionList.Add(arrowDefinition);

        OnNewArrowDefinitionList(arrowDefinitionList);
    }

    public static void ChangeArrowMode(bool on)
    {
        arrowMode = on;
        OnArrowModeChange(arrowMode);
    }
}
