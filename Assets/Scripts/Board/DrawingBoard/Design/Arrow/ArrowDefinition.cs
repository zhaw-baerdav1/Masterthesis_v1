using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//data transfer object of arrow
public class ArrowDefinition
{
    public long id;
    public long startCubeDefinitionId;
    public long endCubeDefinitionId;

    public ArrowDefinition(long _id, long _startCubeDefinitionId, long _endCubeDefinitionId)
    {
        id = _id;
        startCubeDefinitionId = _startCubeDefinitionId;
        endCubeDefinitionId = _endCubeDefinitionId;
    }
}
