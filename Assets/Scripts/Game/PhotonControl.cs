using ExitGames.Client.Photon;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton Class, handles all events for the Photon multiplayer service.
/// </summary>
public class PhotonControl : Photon.MonoBehaviour {

    public Action onJoinedRoom;
    public Action onLeftRoom;
    public Action<PhotonPlayer> onPlayerJoined;
    public Action<PhotonPlayer> onPlayerLeft;
    public Action<PhotonPlayer> onLocalPlayerDataSet;
    public Action<Hashtable> onMissionRecieved;
    public Action<PhotonPlayer, GameControl.GAMESTATE> onPlayerStateRecieved;
    public Action onMissionSent;
    public Action onMakeNewMission;

	public static PhotonControl pObject;

	public int maxPlayersInRoom = 2;

	private MissionControl missionControl;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Awake, Checks if PhotonControl is already present, if false, makes it persistent, if true destroys the new object and keeps the old persistent one.
    /// </summary>
    void Awake()
    {
        if (pObject == null)
        {
            DontDestroyOnLoad(gameObject);
            pObject = this;
        }
        else if (pObject != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// On Monobehaviour Start, sets autoJoinLobby to true in PhotonNetwork, and registers a new event handlers to PhotonNetwork.
    /// </summary>
    public void Start()
    {
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.OnEventCall += OnCustomEventHandler;
    } 
    #endregion

    #region Public Methods
    /// <summary>
    /// Connects to the Photon service using default settings.
    /// </summary>
    public void ConnectToPhoton()
    {
        Debug.Log("Connecting to Photon");
        //PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.SelfHosted;
        //PhotonNetwork.PhotonServerSettings.ServerAddress = "app-eu.exitgamescloud.com";
        //PhotonNetwork.PhotonServerSettings.ServerPort = 5055;
        PhotonNetwork.ConnectUsingSettings(GameControl.GameVersion);
    }

    /// <summary>
    /// Converts the player data from the Playfab service to a Hashtable to be saved in the Photon service.
    /// </summary>
    /// <param name="accountInfo">UserAccountInfo retrieved from the Playfab service.</param>
    /// <param name="playerData">Dictionary of UserDataRecord retrieved from the Playfab service.</param>
    public void SetPlayerData(UserAccountInfo accountInfo, Dictionary<string, UserDataRecord> playerData)
    {
        PhotonNetwork.player.name = accountInfo.Username;
        Hashtable customProperties = new Hashtable();
        customProperties.Add("playFabID", accountInfo.PlayFabId);
        foreach (KeyValuePair<string, UserDataRecord> entry in playerData)
        {
            customProperties.Add(entry.Key, entry.Value.Value);
        }
        PhotonNetwork.SetPlayerCustomProperties(customProperties);
        if (onLocalPlayerDataSet != null)
        {
            onLocalPlayerDataSet(PhotonNetwork.player);
        }
    }

    /// <summary>
    /// Sends a hashtable with mission data to all players in the room.
    /// </summary>
    /// <param name="newMission">The hashtable containing mission data</param>
    public void SendMission(Mission newMission)
    {
        Hashtable missionTable = newMission.ToHashtable();
        print("missionTable: " + missionTable);
        GameControl.pObject.GameState = GameControl.GAMESTATE.WaitingInGroup;
        if (PhotonNetwork.RaiseEvent(1, missionTable, true, null))
        {
            if (onMissionSent != null)
            {
                onMissionSent();
            }
        }

    }

    /// <summary>
    /// Send the current local gamestate to all players in the room.
    /// </summary>
    public void SendGameState()
    {
        if (PhotonNetwork.RaiseEvent(2, GameControl.pObject.GameState, true, null))
        {
            Debug.Log("gameState sent");
        }
    }
    #endregion

    #region PhotonNetwork Event Handlers
    /// <summary>
    /// On recieving a custom event from the Photon service, checks the eventCode for the type of event and acts accordingly.
    /// </summary>
    /// <param name="eventCode">The eventcode for the recieved event.</param>
    /// <param name="content">Optional content being sent by the event.</param>
    /// <param name="senderID">ID of the PhotonPlayer that sent the event.</param>
    public void OnCustomEventHandler(byte eventCode, object content, int senderID)
    {
        switch (eventCode)
        {
            case 1:
                Debug.Log("new mission recieved from Photon: " + content);
                if (onMissionRecieved != null)
                {
                    onMissionRecieved(content as Hashtable);
                }
                break;
            case 2:
                PhotonPlayer senderPlayer = null;
                foreach (PhotonPlayer p in PhotonNetwork.otherPlayers)
                {
                    if (p.ID == senderID)
                    {
                        senderPlayer = p;
                        break;
                    }
                }
                Debug.Log("GAMESTATE recieved from Photon from player: " + senderPlayer + " sending: " + content);
                if (onPlayerStateRecieved != null)
                {
                    onPlayerStateRecieved(senderPlayer, (GameControl.GAMESTATE) content);
                }
                break;
            default:
                Debug.Log("Unknown event code!");
                break;
        }
    }

    /// <summary>
    /// On Photon Event RandomJoinFailed, create a new room and join that one.
    /// </summary>
    public void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = maxPlayersInRoom }, null);
    }

    /// <summary>
    /// Debug handler to show connection status.
    /// </summary>
    public void OnConnectedToPhoton()
    {
        Debug.Log("Connected to Photon");
    }

    /// <summary>
    /// On Photon Event FailedToConnectToPhoton, print the cause of the failure.
    /// </summary>
    /// <param name="cause">DisconnectCause provided by Photon.</param>
    public void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    /// <summary>
    /// On Photon Event Joinroom, navigate to UIRoomView.
    /// </summary>
    public void OnJoinedRoom()
    {
        if (onJoinedRoom != null)
        {
            onJoinedRoom();
        }
        GameControl.pObject.GameState = GameControl.GAMESTATE.WaitingInGroup;
        UICanvas.canvas.NavToUIGroup("UIRoom", "UIRoomView");
    }

    /// <summary>
    /// Calls local onLeftRoom Event.
    /// </summary>
    public void OnLeftRoom()
    {
        if (onLeftRoom != null)
        {
            onLeftRoom();
        }
    }

    /// <summary>
    /// Calls local onPlayerJoined event, calls MasterMakeNewMission().
    /// </summary>
    /// <param name="newPlayer">The PhotonPlayer that joined the room.</param>
    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        MasterMakeNewMission();
        if (onPlayerJoined != null)
        {
            onPlayerJoined(newPlayer);
        }
    }

    /// <summary>
    /// Calls local onPlayerLeft event.
    /// </summary>
    /// <param name="leftPlayer">The PhotonPlayer that left the room.</param>
    public void OnPhotonPlayerDisconnected(PhotonPlayer leftPlayer)
    {
        if (onPlayerLeft != null)
        {
            onPlayerLeft(leftPlayer);
        }
    }

    /// <summary>
    /// Calls MasterMakeNewMission().
    /// </summary>
    /// <param name="newMasterClient">PhotonPlayer of the new master client</param>
    public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        MasterMakeNewMission();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Checks if the room is full and if the local player is the master client. If true, calls onMakeMission Event.
    /// </summary>
    private void MasterMakeNewMission()
    {
        if (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers && PhotonNetwork.player == PhotonNetwork.masterClient)
        {
            if (onMakeNewMission != null)
            {
                onMakeNewMission();
            }
        }
    }
    #endregion
}
