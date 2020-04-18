using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeSelector : MonoBehaviour
{

    bool cubethere = false;
    private void OnTriggerEnter(Collider other)
    {
        if (cubethere)
        {
            return;
        }

        CubeDefinition cubeDefinition = new CubeDefinition(1, "Test1", new Vector3(0, 0.05f, 0));
        CubeList.AddNewCubeDefinition(cubeDefinition);
        cubethere = true;
    }
}
