using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void AttachToDrawingBoard(Transform panelTransform)
    {
        transform.SetParent(panelTransform);
    }

    public void Initialize()
    {
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(.08f, .08f, .08f);

        ApplyPosition();
        ApplyText();
    }

    private void ApplyText()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();

        TextMesh[] textMeshes = GetComponentsInChildren<TextMesh>();
        foreach (TextMesh textMesh in textMeshes)
        {
            textMesh.text = _cubeDefinition.naming;
        }
    }

    public void SetMaterial(Material material)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    public void MoveUp()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        Vector3 position = _cubeDefinition.position;

        position.x = position.x + 0.05f;

        cubeDefinition.position = position;

        ApplyPosition();
    }

    public void MoveDown()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        Vector3 position = _cubeDefinition.position;

        position.x = position.x - 0.05f;

        cubeDefinition.position = position;

        ApplyPosition();
    }

    public void MoveLeft()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        Vector3 position = _cubeDefinition.position;

        position.z = position.z + 0.05f;

        cubeDefinition.position = position;

        ApplyPosition();
    }

    public void MoveRight()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        Vector3 position = _cubeDefinition.position;

        position.z = position.z - 0.05f;

        cubeDefinition.position = position;

        ApplyPosition();
    }

    private void ApplyPosition()
    {
        CubeDefinition _cubeDefinition = GetCubeDefinition();
        transform.localPosition = _cubeDefinition.position;
    }
}
