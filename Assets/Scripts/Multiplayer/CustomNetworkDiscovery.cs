using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class CustomNetworkDiscovery : NetworkDiscovery
{
    private float timeout = 5f;

    private Dictionary<LanConnectionInfo, float> lanAddresses = new Dictionary<LanConnectionInfo, float>();

    public void Awake()
    {
        StartCoroutine(CleanupExpiredEntries());
    }

    public void StartBroadcast()
    {
        if (running) { 
            StopBroadcast();
        }

        FindObjectOfType<CustomNetworkManager>().StartHost();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        LanConnectionInfo info = new LanConnectionInfo(fromAddress, data);

        if (lanAddresses.ContainsKey(info) == false)
        {
            lanAddresses.Add(info, Time.time + timeout);
            WorkspaceList.HandleLocalWorspaceList(new List<LanConnectionInfo>(lanAddresses.Keys));
        }
        else
        {
            lanAddresses[info] = Time.time + timeout;
        }
    }

    private IEnumerator CleanupExpiredEntries()
    {
        while (true)
        {
            bool changed = false;

            List<LanConnectionInfo> lanConnectionInfoList = new List<LanConnectionInfo>(lanAddresses.Keys);
            foreach (LanConnectionInfo lanConnectionInfo in lanConnectionInfoList)
            {
                if(lanAddresses[lanConnectionInfo] <= Time.time)
                {
                    lanAddresses.Remove(lanConnectionInfo);
                    changed = true;
                }
            }

            if (changed)
            {
                WorkspaceList.HandleLocalWorspaceList(lanConnectionInfoList);
            }

            yield return new WaitForSeconds(timeout);
        }
    }
}