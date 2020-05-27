using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for handling events via unity event system
public class ArrowList : MonoBehaviour
{
    public static event Action<ArrowDefinition> OnNewArrowDefinition = delegate { };

    public static event Action<List<ArrowDefinition>> OnNewArrowDefinitionList = delegate { };
    private static List<ArrowDefinition> arrowDefinitionList = new List<ArrowDefinition>();

    public static event Action<bool> OnArrowModeChange = delegate { };
    public static bool arrowMode = false;
    
    //triggers if a new arrow is being created
    public static void TriggerNewArrowDefinition(long startId, long endId)
    {
        //execute only if arrowmode is on
        if (!arrowMode)
        {
            return;
        }

        //assign new id
        int newArrowDefinitionId = arrowDefinitionList.Count + 1;
        ArrowDefinition arrowDefinition = new ArrowDefinition(newArrowDefinitionId, startId, endId);
        
        //trigger all listeners
        OnNewArrowDefinition(arrowDefinition);
    }

    //triggers if new list of arrows should be apply
    public static void AddNewArrowDefinitionList(ArrowDefinition arrowDefinition)
    {
        //add new item to list
        arrowDefinitionList.Add(arrowDefinition);

        //trigger all listeners
        OnNewArrowDefinitionList(arrowDefinitionList);
    }

    //triggers the update of the arrow definitions (e.g. if cube is moved)
    public static void RefreshArrowDefinitionList()
    {
        //trigger all listeners
        OnNewArrowDefinitionList(arrowDefinitionList);
    }

    //triggers if the arrow mode should be active
    public static void ChangeArrowMode(bool on)
    {
        arrowMode = on;

        //trigger all listeners
        OnArrowModeChange(arrowMode);
    }
}
