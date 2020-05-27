using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using System;

//responsible for all the network traffic
public class CustomNetworkManager : NetworkManager
{
    private float nextRefreshTime;
    private static float refreshCycle = 5f;
    
    private int playerCount = 0;

    public List<string> roomNameList;

    public List<GameObject> playerList;

    private Dictionary<int, int> spawnDictionary = new Dictionary<int, int>();

    private int selectedCharacterNumber;

    //ensure networkmanager is singleton and bind events
    private void Awake()
    {
        singleton = this;
        DontDestroyOnLoad(this.gameObject);

        CharacterList.OnCharacterSelected += CharacterList_OnCharacterSelected;
    }

    //unbind events
    private void OnDestroy()
    {
        CharacterList.OnCharacterSelected -= CharacterList_OnCharacterSelected;
    }

    //initiate broadcasing availability
    private void Start()
    {
        CustomNetworkDiscovery customNetworkDiscovery = GetComponent<CustomNetworkDiscovery>();
        customNetworkDiscovery.Initialize();
        customNetworkDiscovery.StartAsClient();
    }

    //start hosting lobby
    public override void OnStartHost()
    {
        base.OnStartHost();

        CustomNetworkDiscovery customNetworkDiscovery = GetComponent<CustomNetworkDiscovery>();
        customNetworkDiscovery.Initialize();
        customNetworkDiscovery.StartAsServer();
    }

    //triggered when the selection of the character has changed
    private void CharacterList_OnCharacterSelected(int selectedNumber)
    {
        selectedCharacterNumber = selectedNumber;
    }

    //add new player to network
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        int connectionId = conn.connectionId;

        //deserialize spawn profile
        SpawnProfile spawnProfile = new SpawnProfile();
        spawnProfile.Deserialize(extraMessageReader);

        //ensure spawn position
        int spawnOrderNumber = playerCount;
        if (spawnDictionary.ContainsKey(connectionId))
        {
            spawnOrderNumber = spawnDictionary[connectionId];
        }

        spawnDictionary[connectionId] = spawnOrderNumber;

        Transform spawnPoint = this.startPositions[spawnOrderNumber];

        //setup client based on selected character
        GameObject playerToJoin = playerList[spawnProfile.characterId];
        GameObject newPlayer = (GameObject)Instantiate(playerToJoin, spawnPoint.position, spawnPoint.rotation);

        //update player value
        CustomPlayer customPlayer = newPlayer.GetComponent<CustomPlayer>();
        customPlayer.connectionId = connectionId;

        //add player to connection
        NetworkServer.AddPlayerForConnection(conn, newPlayer, playerControllerId);
        playerCount++;
    }
    
    //remove connection to player
    public override void OnServerRemovePlayer(NetworkConnection conn, UnityEngine.Networking.PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        playerCount--;
    }

    //start matchmaking and create new match
    public void StartInternetHosting()
    { 
        StartMatchMaker();
        matchMaker.CreateMatch("Test-Workspace", 6, true, "", "", "", 0, 0, OnInternetWorkspaceCreate);
    }

    //triggered when clicking host
    private void OnInternetWorkspaceCreate(bool success, string extendedInfo, MatchInfo responseData)
    {
        base.StartHost(responseData);
    }

    //triggered when room should be changed
    public void ChangeToNextRoom()
    {
        //update room index and select new room
        int currentIndex = roomNameList.IndexOf(networkSceneName);

        if (currentIndex == (roomNameList.Count - 1))
        {
            return;
        }

        //change scene on server
        ServerChangeScene(roomNameList[currentIndex + 1]);
    }

    //update scene on client
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        //ensure player is still connected
        ClientScene.Ready(conn);

        bool hasPlayerJoined = (ClientScene.localPlayers.Count != 0);

        //rejoin if scene has changed
        if (hasPlayerJoined)
        {
            ClientScene.RemovePlayer(0);
        }

        //add player to connection
        AddCustomPlayer(conn);
    }

    private void Update()
    {        
        //do not continue if already in network
        if (!String.IsNullOrEmpty(networkSceneName))
        {
            return;
        }

        //trigger update of lobby values
        if (Time.time >= nextRefreshTime)
        {
            nextRefreshTime = Time.time + refreshCycle;

            RefreshInternetWorkspaceList();
            RefreshCharacterList();
            RefreshRoomList();
        }
    }

    //refresh newest list of workspace
    private void RefreshInternetWorkspaceList()
    {
        if (matchMaker == null)
        {
            StartMatchMaker();
        }

        matchMaker.ListMatches(0, 10, "", true, 0, 0, HandleListWorkspacesComplete);
    }

    //refresh newest list of characters
    private void RefreshCharacterList()
    {
        CharacterList.HandleCharacterList(playerList);
    }

    //refresh newest list of rooms
    private void RefreshRoomList()
    {
        RoomList.HandleRoomNameList(roomNameList);
    }

    //if matches are update, trigger event system
    private void HandleListWorkspacesComplete(bool success, string extendedinfo, List<MatchInfoSnapshot> responseData)
    {
        WorkspaceList.HandleInternetWorspaceList(responseData);
    }

    //join workspace
    public void JoinWorkspace(WorkspaceNetworkInfo workspaceNetworkInfo)
    {
        //join through internet if online
        if (workspaceNetworkInfo.IsOnline())
        {
            JoinInternetWorkspace(workspaceNetworkInfo.internetMatch);
            return;
        }

        //join through LAN if locally
        JoinLocalWorkspace(workspaceNetworkInfo.localMatch);
    }

    //logic for joining internet match
    public void JoinInternetWorkspace(MatchInfoSnapshot workspace)
    {
        if (matchMaker == null)
        {
            StartMatchMaker();
        }

        matchMaker.JoinMatch(workspace.networkId, "", "", "", 0, 0, HandleJoinInternetWorkspace);
    }

    //start client if joining internet workspace
    private void HandleJoinInternetWorkspace(bool success, string extendedInfo, MatchInfo responseData)
    {
        StartClient(responseData);
    }

    //logic for joining LAN match
    public void JoinLocalWorkspace(LanConnectionInfo lanConnectionInfo)
    {
        networkPort = lanConnectionInfo.port;
        networkAddress = lanConnectionInfo.ipAddress;

        StartClient();
    }

    //if client connects
    public override void OnClientConnect(NetworkConnection conn)
    {
        if (!clientLoadedScene)
        {
            //if scene is not loaded, add player
            ClientScene.Ready(conn);
            AddCustomPlayer(conn);
        }
    }

    //dedicated logic to add player and select character
    private void AddCustomPlayer(NetworkConnection conn)
    {
        SpawnProfile spawnProfile = new SpawnProfile();
        spawnProfile.characterId = selectedCharacterNumber;

        ClientScene.AddPlayer(client.connection, 0, spawnProfile);
    }
}