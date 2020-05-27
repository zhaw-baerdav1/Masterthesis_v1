using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

//represents workplacelist item visualisation
public class WorkplaceListItem : MonoBehaviour
{
    private WorkspaceNetworkInfo workspaceNetworkInfo;

    //initialize object based on input
    public void Initialize(WorkspaceNetworkInfo _workspaceNetworkInfo, Transform parentPlane, int workspaceListCount)
    {
        this.workspaceNetworkInfo = _workspaceNetworkInfo;

        //ensure transform matches UI
        transform.SetParent(parentPlane.transform);
        transform.localScale = new Vector3(1, 1, 0.2f);
        transform.localRotation = Quaternion.identity;

        //set position based on order
        float orderPosition = (-3.74f) + (2.48f * workspaceListCount);
        transform.localPosition = new Vector3(0, 0, orderPosition);

        //update list with number indicator
        string workspaceName = (workspaceListCount + 1) + ". " + workspaceNetworkInfo.GetName();

        //ensure text is shown
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = workspaceName;
        
        //ensure scale matches screen
        textMesh.gameObject.transform.localScale = new Vector3(0.2f, 1, 1);
        textMesh.gameObject.transform.localRotation = Quaternion.Euler(90, 180, 0);
        textMesh.gameObject.transform.localPosition = new Vector3(3.445f, 0, 0);
    }

    //mark the entry as selected if chosen by steamvr input
    public void MarkAsSelected()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.fontStyle = FontStyle.Bold;
        textMesh.color = Color.yellow;
    }

    //mark the entry as deselected if chosen by steamvr input
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
