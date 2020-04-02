﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking.Match;

public class CustomNetworkManager : NetworkManager
{
    private float nextRefreshTime;
    private int playerCount = 0;
    
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        foreach(CustomPlayer customPlayer in FindObjectsOfType<CustomPlayer>())
        {
            if (customPlayer.name.Equals("OfflinePlayer"))
            {
                customPlayer.gameObject.SetActive(false);
            }
        }

        Transform spawnPoint = this.startPositions[playerCount];

        GameObject newPlayer = (GameObject)Instantiate(this.playerPrefab, spawnPoint.position, spawnPoint.rotation);
        newPlayer.SetActive(true);
        
        NetworkServer.AddPlayerForConnection(conn, newPlayer, playerControllerId);
        playerCount++;
    }
    
    public override void OnServerRemovePlayer(NetworkConnection conn, UnityEngine.Networking.PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        playerCount--;
    }

    public void StartHosting()
    {
        StartMatchMaker();
        matchMaker.CreateMatch("Test-Workspace", 6, true, "", "", "", 0, 0, OnWorkspaceCreate);
    }

    private void OnWorkspaceCreate(bool success, string extendedInfo, MatchInfo responseData)
    {
        base.StartHost(responseData);
    }

    private void Update()
    {
        if (Time.time >= nextRefreshTime)
        {
            RefreshWorkspaceList();
        }
    }

    private void RefreshWorkspaceList()
    {
        nextRefreshTime = Time.time + 5f;

        if (matchMaker == null)
        {
            StartMatchMaker();
        }

        matchMaker.ListMatches(0, 10, "", true, 0, 0, HandleListWorkspacesComplete);
    }

    private void HandleListWorkspacesComplete(bool success, string extendedinfo, List<MatchInfoSnapshot> responseData)
    {
        WorkspaceList.HandleWorspaceList(responseData);
    }

    public void JoinWorkspace(MatchInfoSnapshot workspace)
    {
        if (matchMaker == null)
        {
            StartMatchMaker();
        }

        matchMaker.JoinMatch(workspace.networkId, "", "", "", 0, 0, HandleJoinWorkspace);
    }

    private void HandleJoinWorkspace(bool success, string extendedInfo, MatchInfo responseData)
    {
        StartClient(responseData);
    }
}