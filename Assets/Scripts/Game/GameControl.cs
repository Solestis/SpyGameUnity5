using UnityEngine;
using ExitGames.Client.Photon;

/// <summary>
/// Singleton Class, stores global data for the game and provides general global functions that can be accessed from anywhere.
/// </summary>
public class GameControl : MonoBehaviour {

	public static GameControl pObject;
	public static System.Random random;
	public static string GameVersion = "0.1";

    public enum GAMESTATE
    {
        Idle,
        WaitingInGroup,
        MakingMission,
        OnMission,
        MissionCompleted,
    };

    public GAMESTATE GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
            Hashtable h = new Hashtable();
            h.Add("gameState", (int)value);
            PhotonNetwork.player.SetCustomProperties(h);
            if (PhotonNetwork.inRoom)
            {
                PhotonControl.pObject.SendGameState();
            }
        }

    }

    private GAMESTATE gameState = GAMESTATE.Idle;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Awake, Checks if GameControl is already present, if false, makes it persistent, if true destroys the new object and keeps the old persistent one.
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
        random = new System.Random();
    } 
    #endregion
}
