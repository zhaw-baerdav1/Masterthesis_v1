using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//represents the cubes visualisation on UI
public class CubeRepresentation : MonoBehaviour
{
    private CubeDefinition cubeDefinition;

    public CubeDefinition GetCubeDefinition()
    {
        return cubeDefinition;
    }

    public void setCubeDefinition(CubeDefinition _cubeDefinition)
    {
        cubeDefinition = _cubeDefinition;
    }

    //attach cube to drawing board
    public void AttachToDrawingBoard(Transform panelTransform)
    {
        transform.SetParent(panelTransform);
    }

    //initiate the cube prefab
    public void Initialize()
    {
        //ensure starting position of cube
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(.08f, .08f, .08f);

        //apply position and text
        ApplyPosition();
        ApplyText();
    }

    //updates all labels on the cube
    private void ApplyText()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();

        TextMeshPro[] textMeshes = GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro textMesh in textMeshes)
        {
            textMesh.text = _cubeDefinition.naming;
        }
    }

    //update material of cube
    public void SetMaterial(Material material)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    //moves the cube up
    public void MoveUp()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        Vector3 position = _cubeDefinition.position;

        position.x = position.x + 0.05f;

        cubeDefinition.position = position;

        //update based on arrow definition
        ApplyPosition();
    }

    //moves the cube up
    public void MoveDown()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        Vector3 position = _cubeDefinition.position;

        position.x = position.x - 0.05f;

        cubeDefinition.position = position;

        //update based on arrow definition
        ApplyPosition();
    }

    //moves the cube left
    public void MoveLeft()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        Vector3 position = _cubeDefinition.position;

        position.z = position.z + 0.05f;

        cubeDefinition.position = position;

        //update based on arrow definition
        ApplyPosition();
    }

    //moves the cube right
    public void MoveRight()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        Vector3 position = _cubeDefinition.position;

        position.z = position.z - 0.05f;

        cubeDefinition.position = position;

        //update based on arrow definition
        ApplyPosition();
    }

    //apply position on UI based on cube definition
    private void ApplyPosition()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        transform.localPosition = _cubeDefinition.position;
    }
}
