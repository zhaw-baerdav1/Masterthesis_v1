using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInternetHostSelector : MonoBehaviour
{
    private bool isHosting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isHosting)
        {
            return;
        }

        FindObjectOfType<CustomNetworkManager>().StartInternetHosting();
        isHosting = true;
    }
}
