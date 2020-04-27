using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeDefinition
{
    public long id;
    public string naming;
    public Vector3 position;

    public CubeDefinition(long _id, string _naming, Vector3 _position)
    {
        id = _id;
        naming= _naming;
        position = _position;
    }
}
