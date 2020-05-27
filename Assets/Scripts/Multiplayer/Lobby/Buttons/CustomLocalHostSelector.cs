using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for handling collider on local host button
public class CustomLocalHostSelector : MonoBehaviour
{
    private bool isHosting = false;

    private void OnTriggerEnter(Collider other)
    {
        //do not host twice
        if (isHosting)
        {
            return;
        }

        //start broadcasting in networkdiscovery
        FindObjectOfType<CustomNetworkDiscovery>().StartBroadcast();
        isHosting = true;
    }
}
