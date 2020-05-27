using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

//data transfer object to handle internet and lan workspace
public class WorkspaceNetworkInfo
{
    public MatchInfoSnapshot internetMatch;
    public LanConnectionInfo localMatch;
    
    //returns if workspace should be online
    public bool IsOnline()
    {
        return internetMatch != null;
    }

    //returns name of workspace
    public string GetName()
    {
        //if online, return official name and amount of participants
        if (IsOnline())
        {
            string workspaceName = internetMatch.name;
            workspaceName += " (" + internetMatch.currentSize + "/" + internetMatch.maxSize + ")";

            return workspaceName;
        }

        //if local, return dedicated name of local match
        return localMatch.name;
    }
}

//represents internal struct for LAN connections
public struct LanConnectionInfo
{
    public string ipAddress;
    public int port;
    public string name;

    public LanConnectionInfo(string fromAddress, string data)
    {
        //identify ipaddress, port and name out of network information
        ipAddress = fromAddress.Substring(fromAddress.LastIndexOf(":") + 1, fromAddress.Length - (fromAddress.LastIndexOf(":") + 1));
        string portText = data.Substring(data.LastIndexOf(":") + 1, data.Length - (data.LastIndexOf(":") + 1));
        port = 7777;
        int.TryParse(portText, out port);
        name = "WS: " + ipAddress;
    }
}