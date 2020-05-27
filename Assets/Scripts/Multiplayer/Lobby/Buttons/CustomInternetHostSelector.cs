using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for button collider when hosting internet workspace
public class CustomInternetHostSelector : MonoBehaviour
{
    private bool isHosting = false;

    private void OnTriggerEnter(Collider other)
    {
        //do not host twice
        if (isHosting)
        {
            return;
        }

        //use network manager to start hosting
        FindObjectOfType<CustomNetworkManager>().StartInternetHosting();
        isHosting = true;
    }
}
