using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// User Interface logic for the RoomView UIElement.
/// </summary>
public class UIRoomView : UIElement {

    #region Linked in Editor
    public Transform playerContainer;
    public Button makeMissionButton;
    public Button newMissionButton;
    public UIWindow newMissionWindow; 
    #endregion

    private Dictionary<PhotonPlayer, GameObject> playerPortraits;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, adds PhotonControl event actions, adds a player portrait for each player in the PhotonNetwork room, adds button listeners.
    /// </summary>
    void Start()
    {
        PhotonControl.pObject.onPlayerJoined += OnPlayerJoined;
        PhotonControl.pObject.onPlayerLeft += OnPlayerLeft;
        PhotonControl.pObject.onMissionRecieved += OnMissionRecieved;
        PhotonControl.pObject.onMissionSent += OnMissionSent;
        PhotonControl.pObject.onMakeNewMission += OnMakeNewMission;
        playerPortraits = new Dictionary<PhotonPlayer, GameObject>();
        AddButtonListeners();
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// On PhotonNetwork PlayerConnected, adds a portrait for the new player.
    /// </summary>
    /// <param name="newPlayer">The new PhotonPlayer that connected.</param>
    public void OnPlayerJoined(PhotonPlayer newPlayer)
    {
        print("Someone else joined!");
        AddPlayerPortrait(newPlayer);
    }

    /// <summary>
    /// On PhotonNetwork PlayerDisconnected, remove the portrait of the player that left.
    /// </summary>
    /// <param name="leftPlayer">The PhotonPlayer that left the room.</param>
    public void OnPlayerLeft(PhotonPlayer leftPlayer)
    {
        RemovePlayerPortrait(leftPlayer);
        Debug.Log(PhotonNetwork.masterClient);
    }

    /// <summary>
    /// Show the makeMissionButton.
    /// </summary>
    private void OnMakeNewMission()
    {
        makeMissionButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// On recieving a mission through the PhotonNetwork, configure the newMissionWindow and activate the newMissionButton, both linked in Editor.
    /// </summary>
    /// <param name="missionData"></param>
    private void OnMissionRecieved(ExitGames.Client.Photon.Hashtable missionData)
    {
        object value;
        if (missionData.TryGetValue("missionTitle", out value))
        {
            newMissionWindow.titleText.text = value as string;
        }
        if (missionData.TryGetValue("missionDescription", out value))
        {
            newMissionWindow.descriptionText.text = value as string;
        }
        newMissionButton.gameObject.SetActive(true);
    }

    private void OnMissionSent()
    {
        makeMissionButton.gameObject.SetActive(false);
    }
    #endregion

    #region Button Methods
    /// <summary>
    /// Navigates to MissionMaker, sets GameState to MakingMission.
    /// </summary>
    public void MakeMission()
    {
        UICanvas.canvas.NavToUIGroup("UIMission", "UIMissionMaker");
        GameControl.pObject.GameState = GameControl.GAMESTATE.MakingMission;
        makeMissionButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Tells MissionControl to start the mission.
    /// </summary>
    public void StartMission()
    {
        MissionControl.pObject.ContinueMission();
        newMissionButton.gameObject.SetActive(false);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// On activation of the UIElement, add all portraits for the players in the room.
    /// </summary>
    override public void Activate()
    {
        base.Activate();
        if (PhotonNetwork.inRoom)
        {
            foreach (PhotonPlayer p in PhotonNetwork.playerList)
            {
                AddPlayerPortrait(p);
            }
        }
        else
        {
            Debug.Log("No room found");
        }
    }

    /// <summary>
    /// On deactivation of the UIElement, remove all player portraits.
    /// </summary>
    public override void Deactivate()
    {
        foreach (Transform child in playerContainer.transform)
        {
            Destroy(child.gameObject);
        }
        playerPortraits = new Dictionary<PhotonPlayer,GameObject>();
        base.Deactivate();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Add button listeners.
    /// </summary>
    private void AddButtonListeners()
    {
        makeMissionButton.onClick.AddListener(() => { MakeMission(); });
        newMissionButton.onClick.AddListener(() => { newMissionWindow.Activate(); });
        newMissionWindow.closeButton.onClick.AddListener(() => { StartMission(); });
    }

    /// <summary>
    /// Remove the portrait of a PhotonPlayer from the canvas.
    /// </summary>
    /// <param name="p">Photonplayer of which the portrait has to be removed.</param>
    private void RemovePlayerPortrait(PhotonPlayer p)
    {
        GameObject portraitObject;
        if (playerPortraits.TryGetValue(p, out portraitObject))
        {
            Destroy(portraitObject);
        }
    }

    /// <summary>
    /// Add a portrait of a PhotonPlayer to the canvas.
    /// </summary>
    /// <param name="p">Photonplayer of which the portrait has to be added.</param>
    private void AddPlayerPortrait(PhotonPlayer p)
    {
        GameObject portraitObject = Instantiate(Resources.Load("PlayerPortrait"), Vector3.zero, Quaternion.identity) as GameObject;
        portraitObject.GetComponent<RectTransform>().SetParent(playerContainer);
        portraitObject.transform.localScale = Vector3.one;
        portraitObject.transform.localPosition = new Vector3(0, 0, playerContainer.localPosition.z);
        PlayerPortrait portraitScript = portraitObject.GetComponent<PlayerPortrait>();
        portraitScript.SetPlayer(p);
        playerPortraits.Add(p, portraitObject);
    } 
    #endregion
}