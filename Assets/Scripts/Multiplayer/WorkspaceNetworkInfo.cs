using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class WorkspaceNetworkInfo
{
    public MatchInfoSnapshot internetMatch;
    public LanConnectionInfo localMatch;
    
    public bool IsOnline()
    {
        return internetMatch != null;
    }

    public string GetName()
    {
        if (IsOnline())
        {
            string workspaceName = internetMatch.name;
            workspaceName += " (" + internetMatch.currentSize + "/" + internetMatch.maxSize + ")";

            return workspaceName;
        }

        return localMatch.name;
    }
}

public struct LanConnectionInfo
{
    public string ipAddress;
    public int port;
    public string name;

    public LanConnectionInfo(string fromAddress, string data)
    {
        ipAddress = fromAddress.Substring(fromAddress.LastIndexOf(":") + 1, fromAddress.Length - (fromAddress.LastIndexOf(":") + 1));
        string portText = data.Substring(data.LastIndexOf(":") + 1, data.Length - (data.LastIndexOf(":") + 1));
        port = 7777;
        int.TryParse(portText, out port);
        name = "WS: " + ipAddress;
    }
}