using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkplaceListItem : MonoBehaviour
{
    public void InitiatePlane(Transform parentPane)
    {
        transform.SetParent(parentPane.transform);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }
}
