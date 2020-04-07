using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class WorkplaceListItem : MonoBehaviour
{
    private MatchInfoSnapshot matchInfoSnapshot;

    public void Initialize(MatchInfoSnapshot workspace, Transform parentPlane, int workspaceListCount)
    {
        this.matchInfoSnapshot = workspace;

        transform.SetParent(parentPlane.transform);
        transform.localScale = new Vector3(1, 1, 0.2f);
        transform.localRotation = Quaternion.identity;

        float orderPosition = (-3.74f) + (2.48f * workspaceListCount);
        transform.localPosition = new Vector3(0, 0, orderPosition);

        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = matchInfoSnapshot.name;
        
        textMesh.gameObject.transform.localScale = new Vector3(0.2f, 1, 1);
        textMesh.gameObject.transform.localRotation = Quaternion.Euler(90, 180, 0);
        textMesh.gameObject.transform.localPosition = new Vector3(3.445f, 0, 0);
    }
}
