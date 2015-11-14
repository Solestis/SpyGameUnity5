using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// User Interface logic for the MainMenu UIElement.
/// </summary>
public class UIMainMenu : UIElement {

	public PlayerPortrait mainPlayerPortrait;
    public Button roomButton;
    public Button leaveRoomButton;
    public Button missionButton;
    public Button editCharacterButton;
    public Button settingsButton;
    public UIWindow settingsWindow;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, loads the portrait of the main player, set event Actions and adds Button listeners.
    /// </summary>
    public void Start()
    {
        mainPlayerPortrait.SetPlayer(PhotonNetwork.player, true);
        PhotonControl.pObject.onLeftRoom += OnLeftRoom;
        onActivate += OnActivate;
        onDeactivate += OnDeactivate;
        AddButtonListeners();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// On activation of the UIElement, show the player portrait (used for 3D portraits). Configure the interactability of the buttons on the main menu.
    /// </summary>
    public void OnActivate()
    {
        mainPlayerPortrait.gameObject.SetActive(true);
        mainPlayerPortrait.BroadcastMessage("GameObjectFadeIn", SendMessageOptions.DontRequireReceiver);

        switch (GameControl.pObject.GameState)
        {
            case GameControl.GAMESTATE.Idle:
                roomButton.interactable = true;
                leaveRoomButton.interactable = false;
                missionButton.interactable = false;
                break;
            case GameControl.GAMESTATE.MakingMission:
                roomButton.interactable = true;
                leaveRoomButton.interactable = false;
                missionButton.interactable = true;
                break;
            case GameControl.GAMESTATE.OnMission:
                roomButton.interactable = false;
                leaveRoomButton.interactable = false;
                missionButton.interactable = true;
                break;
            case GameControl.GAMESTATE.WaitingInGroup:
                roomButton.interactable = true;
                leaveRoomButton.interactable = true;
                missionButton.interactable = false;
                break;
            case GameControl.GAMESTATE.MissionCompleted:
                roomButton.interactable = true;
                leaveRoomButton.interactable = true;
                missionButton.interactable = false;
                break;
            default:
                Console.WriteLine("Gamestate invalid");
                break;
        }
    }

    /// <summary>
    /// On deactivation of the UIElement, hide the player portrait (used for 3D portraits).
    /// </summary>
    public void OnDeactivate()
    {
        mainPlayerPortrait.BroadcastMessage("GameObjectFadeOut", SendMessageOptions.DontRequireReceiver);
        mainPlayerPortrait.gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds button listeners.
    /// </summary>
    private void AddButtonListeners()
    {
        roomButton.onClick.AddListener(() => { UICanvas.canvas.NavToUIGroup("UIRoom"); });
        leaveRoomButton.onClick.AddListener(() => { LeaveRoom(); });
        missionButton.onClick.AddListener(() => { UICanvas.canvas.NavToUIGroup("UIMission"); });
        settingsButton.onClick.AddListener(() => { settingsWindow.Activate(); });
        roomButton.interactable = true;
        leaveRoomButton.interactable = false;
        missionButton.interactable = false;
    }

    /// <summary>
    /// On PhotonNetwork OnLeftRoom event, make leaveroombutton uninteractable.
    /// </summary>
    private void OnLeftRoom()
    {
        leaveRoomButton.interactable = false;
    }

    /// <summary>
    /// Leaves the Photon room, joins the lobby and navigates to the Main Menu.
    /// </summary>
    private void LeaveRoom()
    {
        UICanvas.canvas.SetActiveUI("UIRoom", "UIJoinRoom");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
    } 
    #endregion
}
