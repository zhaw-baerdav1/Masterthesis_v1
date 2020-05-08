using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class WorkplaceListItem : MonoBehaviour
{
    private WorkspaceNetworkInfo workspaceNetworkInfo;

    public void Initialize(WorkspaceNetworkInfo _workspaceNetworkInfo, Transform parentPlane, int workspaceListCount)
    {
        this.workspaceNetworkInfo = _workspaceNetworkInfo;

        transform.SetParent(parentPlane.transform);
        transform.localScale = new Vector3(1, 1, 0.2f);
        transform.localRotation = Quaternion.identity;

        float orderPosition = (-3.74f) + (2.48f * workspaceListCount);
        transform.localPosition = new Vector3(0, 0, orderPosition);

        string workspaceName = (workspaceListCount + 1) + ". " + workspaceNetworkInfo.GetName();

        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = workspaceName;
        
        textMesh.gameObject.transform.localScale = new Vector3(0.2f, 1, 1);
        textMesh.gameObject.transform.localRotation = Quaternion.Euler(90, 180, 0);
        textMesh.gameObject.transform.localPosition = new Vector3(3.445f, 0, 0);
    }

    public void MarkAsSelected()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.fontStyle = FontStyle.Bold;
        textMesh.color = Color.yellow;
    }

    public void RemoveMarkAsSelected()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.fontStyle = FontStyle.Normal;
        textMesh.color = Color.white;
    }

    public WorkspaceNetworkInfo GetWorkspaceNetworkInfo()
    {
        return workspaceNetworkInfo;
    }
}
