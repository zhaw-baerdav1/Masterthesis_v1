using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

//represents character list item in visualisation
public class CharacterListItem : MonoBehaviour
{
    private GameObject character;

    //initiate object based  on input
    public void Initialize(GameObject character, Transform parentPlane, int workspaceListCount)
    {
        this.character = character;

        //add element to ui
        transform.SetParent(parentPlane.transform);
        transform.localScale = new Vector3(0.5f, 0.5f, 0.1f);
        transform.localRotation = Quaternion.identity;

        //ensure order
        float orderPosition = (-1.87f) + (1.24f * workspaceListCount);
        transform.localPosition = new Vector3(0, 0, orderPosition);

        //update text
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = (workspaceListCount + 1) + ". " + character.name;
        
        //ensure text is shown on UI
        textMesh.gameObject.transform.localScale = new Vector3(0.2f, 1, 1);
        textMesh.gameObject.transform.localRotation = Quaternion.Euler(90, 180, 0);
        textMesh.gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    //mark entry of character as selected
    public void MarkAsSelected()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.fontStyle = FontStyle.Bold;
        textMesh.color = Color.yellow;
    }

    //mark entry of character as deselected
    public void RemoveMarkAsSelected()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.fontStyle = FontStyle.Normal;
        textMesh.color = Color.white;
    }

    public GameObject GetCharacter()
    {
        return character;
    }
}
