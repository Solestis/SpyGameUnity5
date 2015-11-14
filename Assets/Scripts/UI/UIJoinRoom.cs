using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// User Interface logic for the JoinRoom UIElement.
/// </summary>
public class UIJoinRoom : UIElement {

    public Button cancelButton;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, add onActivate action and add Button listeners.
    /// </summary>
    void Start()
    {
        onActivate += JoinRoom;
        AddButtonListeners();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Leave PhotonNetwork room and navigate to main menu.
    /// </summary>
    private void CancelRoomJoin()
    {
        PhotonNetwork.LeaveRoom();
        UICanvas.canvas.NavToMainMenu();
    }

    /// <summary>
    /// Join a random PhotonNetwork room.
    /// </summary>
    private void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// Add button listeners.
    /// </summary>
    private void AddButtonListeners()
    {
        cancelButton.onClick.AddListener(() => { CancelRoomJoin(); });
    }
    #endregion
}
