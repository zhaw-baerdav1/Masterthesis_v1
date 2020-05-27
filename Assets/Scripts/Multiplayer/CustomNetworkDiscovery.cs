using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

//responsible for broadcasting and receiving workspace in LAN
public class CustomNetworkDiscovery : NetworkDiscovery
{
    private float timeout = 5f;

    private Dictionary<LanConnectionInfo, float> lanAddresses = new Dictionary<LanConnectionInfo, float>();

    //ensure list kept up to date
    public void Awake()
    {
        StartCoroutine(CleanupExpiredEntries());
    }

    //start broadcasting
    public void StartBroadcast()
    {
        if (running) { 
            StopBroadcast();
        }

        //use networkmanager to start host
        FindObjectOfType<CustomNetworkManager>().StartHost();
    }

    //triggered when new broadcast has been found
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        LanConnectionInfo info = new LanConnectionInfo(fromAddress, data);

        //only add to list of not yet existing
        if (lanAddresses.ContainsKey(info) == false)
        {
            lanAddresses.Add(info, Time.time + timeout);

            //trigger event to update workspace list
            WorkspaceList.HandleLocalWorspaceList(new List<LanConnectionInfo>(lanAddresses.Keys));
        }
        else
        {
            //update time for expiry
            lanAddresses[info] = Time.time + timeout;
        }
    }

    //runs constantly to keep list up to date and clean up expired entries
    private IEnumerator CleanupExpiredEntries()
    {
        while (true)
        {
            bool changed = false;

            List<LanConnectionInfo> lanConnectionInfoList = new List<LanConnectionInfo>(lanAddresses.Keys);
            foreach (LanConnectionInfo lanConnectionInfo in lanConnectionInfoList)
            {
                //if expiry time has been reached
                if(lanAddresses[lanConnectionInfo] <= Time.time)
                {
                    lanAddresses.Remove(lanConnectionInfo);
                    changed = true;
                }
            }

            if (changed)
            {
                //if entries have changed, update workspace list
                WorkspaceList.HandleLocalWorspaceList(lanConnectionInfoList);
            }

            yield return new WaitForSeconds(timeout);
        }
    }
}