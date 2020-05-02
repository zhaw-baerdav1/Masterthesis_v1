﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using System;

public class CustomNetworkManager : NetworkManager
{
    private float nextRefreshTime;
    private static float refreshCycle = 5f;
    
    private int playerCount = 0;

    public List<string> roomNameList;

    public List<GameObject> playerList;

    private Dictionary<int, int> spawnDictionary = new Dictionary<int, int>();

    private int selectedCharacterNumber;

    private void Awake()
    {
        CharacterList.OnCharacterSelected += CharacterList_OnCharacterSelected;
    }

    private void OnDestroy()
    {
        CharacterList.OnCharacterSelected -= CharacterList_OnCharacterSelected;
    }

    private void CharacterList_OnCharacterSelected(int selectedNumber)
    {
        selectedCharacterNumber = selectedNumber;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        int connectionId = conn.connectionId;

        SpawnProfile spawnProfile = new SpawnProfile();
        spawnProfile.Deserialize(extraMessageReader);

        int spawnOrderNumber = playerCount;
        if (spawnDictionary.ContainsKey(connectionId))
        {
            spawnOrderNumber = spawnDictionary[connectionId];
        }

        spawnDictionary[connectionId] = spawnOrderNumber;

        Transform spawnPoint = this.startPositions[spawnOrderNumber];

        GameObject playerToJoin = playerList[spawnProfile.characterId];
        GameObject newPlayer = (GameObject)Instantiate(playerToJoin, spawnPoint.position, spawnPoint.rotation);

        CustomPlayer customPlayer = newPlayer.GetComponent<CustomPlayer>();
        customPlayer.connectionId = connectionId;


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

    public void ChangeToNextRoom()
    {
        int currentIndex = roomNameList.IndexOf(networkSceneName);

        if (currentIndex == (roomNameList.Count - 1))
        {
            return;
        }

        ServerChangeScene(roomNameList[currentIndex + 1]);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        ClientScene.Ready(conn);

        bool hasPlayerJoined = (ClientScene.localPlayers.Count != 0);

        if (hasPlayerJoined)
        {
            ClientScene.RemovePlayer(0);
        }

        AddCustomPlayer(conn);
    }

    private void Update()
    {

        if (Time.time >= nextRefreshTime)
        {
            nextRefreshTime = Time.time + refreshCycle;

            RefreshWorkspaceList();
            RefreshCharacterList();
            RefreshRoomList();
        }
    }

    private void RefreshWorkspaceList()
    {
        if (matchMaker == null)
        {
            StartMatchMaker();
        }

        matchMaker.ListMatches(0, 10, "", true, 0, 0, HandleListWorkspacesComplete);
    }

    private void RefreshCharacterList()
    {
        CharacterList.HandleCharacterList(playerList);
    }

    private void RefreshRoomList()
    {
        RoomList.HandleRoomNameList(roomNameList);
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

    public override void OnClientConnect(NetworkConnection conn)
    {
        if (!clientLoadedScene)
        {
            ClientScene.Ready(conn);
            AddCustomPlayer(conn);
        }
    }

    private void AddCustomPlayer(NetworkConnection conn)
    {
        SpawnProfile spawnProfile = new SpawnProfile();
        spawnProfile.characterId = selectedCharacterNumber;

        ClientScene.AddPlayer(client.connection, 0, spawnProfile);
    }
}