using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLocalHostSelector : MonoBehaviour
{
    private bool isHosting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isHosting)
        {
            return;
        }

        FindObjectOfType<CustomNetworkDiscovery>().StartBroadcast();
        isHosting = true;
    }
}
