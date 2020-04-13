using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeSelector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CubeDefinition cubeDefinition = new CubeDefinition();
        cubeDefinition.setId(1);
        cubeDefinition.setNaming("Test1");
        cubeDefinition.setPosition(new Vector3(0,0.05f,0));

        CubeList.AddNewCubeDefinition(cubeDefinition);
    }
}
