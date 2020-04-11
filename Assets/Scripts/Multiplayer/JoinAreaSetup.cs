using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinAreaSetup : MonoBehaviour
{
    void Start()
    {
        WorkspaceList.HandleWorkspaceActivate(true);
        CharacterList.HandleCharacterActivate(false);
    }
}
