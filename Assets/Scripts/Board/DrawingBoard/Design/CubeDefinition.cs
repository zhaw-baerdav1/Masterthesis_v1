using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeDefinition : MonoBehaviour
{
    private long id;
    private string naming;
    private Vector3 position;


    public long getId()
    {
        return this.id;
    }

    public void setId(long id)
    {
        this.id = id;
    }

    public string getNaming()
    {
        return this.naming;
    }

    public void setNaming(string naming)
    {
        this.naming = naming;
    }

    public Vector3 getPosition()
    {
        return this.position;
    }

    public void setPosition(Vector3 position)
    {
        this.position = position;
    }
}
