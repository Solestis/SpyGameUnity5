using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameObject class to load the Character portrait of the player.
/// </summary>
public class PlayerPortrait : MonoBehaviour
{
    #region Linked in Editor
    public Text nameText;
    public RectTransform characterContainer;
    public Text recentScoreText;
    public GameObject recentScoreObject;
    #endregion

    public PhotonPlayer player { get; private set; }

    private Character character;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, add eventlistener to Photoncontrol onPlayerStateRecieved.
    /// </summary>
    void Start()
    {
        if (recentScoreObject != null && recentScoreText != null)
        {
            PhotonControl.pObject.onPlayerStateRecieved += ShowRecentScore;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Either loads the player portrait through Photoncontrol events or invokes LoadPlayerData directly.
    /// </summary>
    /// <param name="p">Player of which the portrait is shown</param>
    /// <param name="loadData">Boolean switch to change if player gets loaded through Photoncontrol events or gets loaded instantly.</param>
    public void SetPlayer(PhotonPlayer p, bool loadData = false)
    {
        player = p;
        if (p == PhotonNetwork.player && loadData)
        {
            PhotonControl.pObject.onLocalPlayerDataSet += LoadPlayerData;
        }
        else
        {
            player = p;
            Invoke("LoadPlayerData", 0.1f);
        }
    }

    /// <summary>
    /// Loads the character body from the PhotonPlayer data.
    /// </summary>
    /// <param name="playerToLoad">Photonplayer of which the character body is loaded.</param>
    public void LoadPlayerData(PhotonPlayer playerToLoad)
    {
        player = playerToLoad;
        nameText.text = player.name;
        object value;
        if (player.customProperties.TryGetValue("gameState", out value))
        {
            int stateInt = (int)value;
            if ((GameControl.GAMESTATE)stateInt == GameControl.GAMESTATE.MissionCompleted)
            {
                recentScoreObject.SetActive(true);
                recentScoreText.text = player.GetScore().ToString();
            }
        }
        character = new Character(player.customProperties);
        character.InstantiateBody(characterContainer.transform);
    }

    /// <summary>
    /// Loads the character body from the local player.
    /// </summary>
    private void LoadPlayerData()
    {
        LoadPlayerData(player);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Shows the score of the player if gamestate is MissionCompleted.
    /// </summary>
    /// <param name="recievedPlayer">Player which score is being recieved.</param>
    /// <param name="state">Gamestate of the recieved player</param>
    private void ShowRecentScore(PhotonPlayer recievedPlayer, GameControl.GAMESTATE state)
    {
        if (player == recievedPlayer && state == GameControl.GAMESTATE.MissionCompleted)
        {
            recentScoreObject.SetActive(true);
            recentScoreText.text = recievedPlayer.GetScore().ToString();
        }
    }
    #endregion
}
