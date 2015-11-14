using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;

/// <summary>
/// Singleton Class, handles all mission progress for the game.
/// </summary>
public class MissionControl : MonoBehaviour {

	public static MissionControl pObject;

    public Text timerText;
    public GameObject timerObject;

	public Mission activeMission { get; private set; }
    public Assignment activeAssignment { get; private set; }
    public Action onNextAssignment;
    public Action onMissionComplete;

    private TimeSpan timerClock;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Awake, Checks if MissionControl is already present, if false, makes it persistent, if true destroys the new object and keeps the old persistent one.
    /// </summary>
    public void Awake()
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
    /// On Monobehaviour Start, adds eventlisteners to PhotonControl events.
    /// </summary>
    public void Start()
    {
        PhotonControl.pObject.onMissionRecieved += SetMissionFromHashtable;
        PhotonControl.pObject.onPlayerStateRecieved += CheckNextRound;
    }
    #endregion

    #region Public Methods    
    /// <summary>
    /// Creates the mission from a hashtable sent over the PhotonNetwork.
    /// </summary>
    /// <param name="missionData">The hashtable containing the mission data.</param>
    public void SetMissionFromHashtable(Hashtable missionData)
    {
        print("mission created from hashtable");
        Mission newMission = new Mission(missionData);
        activeMission = newMission;
        SetMissionTimer();
    }

    /// <summary>
    /// Creates a mission from assignmentObjects like those used in the missionMaker, saves it as a Hashtable and sends it over the PhotonNetwork.
    /// </summary>
    /// <param name="assignmentObjects">Array of assignmentObjects.</param>
    /// <param name="missionDuration">De duration of the mission in hours.</param>
    public Mission CreateMissionFromAssignmentObjects(AssignmentObject[] assignmentObjects, int missionDuration)
    {
        Dictionary<int, Assignment> assignments = new Dictionary<int, Assignment>();
        foreach (AssignmentObject ao in assignmentObjects)
        {
            AssignmentObject assignmentObject = ao.gameObject.GetComponent<AssignmentObject>();
            assignmentObject.assignment.sequenceNumber = ao.gameObject.transform.GetSiblingIndex() + 1;
            assignments.Add(assignmentObject.assignment.sequenceNumber, assignmentObject.assignment);
        }
        Mission newMission = new Mission(DateTime.Now.AddDays(1), missionDuration, assignments);
        print("newMission: " + newMission);
        return newMission;
    }

    /// <summary>
    /// Checks the missionstate and calls NextAssignment.
    /// </summary>
    public void ContinueMission()
    {
        switch (activeMission.missionState)
        {
            case Mission.STATE_NEW:
                NextAssignment();
                InvokeRepeating("RunMissionTimer", 1f, 1f);
                GameControl.pObject.GameState = GameControl.GAMESTATE.OnMission;
                break;
            case Mission.STATE_ONGOING:
                if (activeAssignment.assignmentState == Assignment.STATE_FINISHED)
                {
                    NextAssignment();
                }
                break;
            case Mission.STATE_FINISHED:
                //UICanvas.canvas.NewPopUp("Missie voltooid!", "Ga nu terug naar het groepscherm", CompleteMission);
                //StopMissionTimer();
                //if (onMissionComplete != null)
                //{
                //    onMissionComplete();
                //}
                //GameControl.pObject.GameState = GameControl.GAMESTATE.WaitingInGroup;
                break;
            default:
                Debug.Log("Unknown missionState");
                break;
        }
    }

    /// <summary>
    /// Calculates score, sets the gamestate to MissionCompleted in Missioncontrol, navigates back to UIRoom.
    /// </summary>
    public void CompleteMission()
    {
        double timerClockInSeconds = timerClock.TotalSeconds;
        double score = 100 * (timerClockInSeconds / (activeMission.missionDuration * 3600));
        PhotonNetwork.player.SetScore((int)score);
        GameControl.pObject.GameState = GameControl.GAMESTATE.MissionCompleted;
        UICanvas.canvas.NavToUIGroup("UIRoom");
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Checks the mission state and progresses the mission to the next assignment if applicable.
    /// </summary>
    private void NextAssignment()
    {
        activeMission.NextAssignment();
        if (activeMission.missionState == Mission.STATE_ONGOING)
        {
            activeAssignment = activeMission.activeAssignment;
            string assignmentUIName = "UI" + activeAssignment.assignmentType + "Assignment";
            UICanvas.canvas.NavToUIGroup("UIMission", assignmentUIName);
            string title = "Nieuwe opdracht: " + activeAssignment.assignmentType;
            UICanvas.canvas.NewPopUp(title, activeAssignment.description);
            UICanvas.canvas.debugText.text = "";
            if (onNextAssignment != null)
            {
                onNextAssignment();
            }
        }
        else if (activeMission.missionState == Mission.STATE_FINISHED)
        {
            UICanvas.canvas.NewPopUp("Missie voltooid!", "Ga nu terug naar het groepscherm", CompleteMission);
            UICanvas.canvas.debugText.text = "";
            StopMissionTimer();
            if (onMissionComplete != null)
            {
                onMissionComplete();
            }
        }
        
    }

    /// <summary>
    /// Initiates the mission timer.
    /// </summary>
    private void SetMissionTimer()
    {
        timerObject.SetActive(true);
        timerClock = new TimeSpan(activeMission.missionDuration, 0, 0);
        timerText.text = timerClock.ToString();
    }

    /// <summary>
    /// Starts the mission timer.
    /// </summary>
    private void RunMissionTimer()
    {
        TimeSpan second = new TimeSpan(0,0,1);
        timerClock = timerClock.Subtract(second);
        timerText.text = timerClock.ToString();
    }

    /// <summary>
    /// Stops the mission timer.
    /// </summary>
    private void StopMissionTimer()
    {
        CancelInvoke("RunMissionTimer");
        print(timerClock);
        timerObject.SetActive(false);
    }

    /// <summary>
    /// If local player is the master client for the group, checks if all players are done with the mission, if true, pass master client off to the next player.
    /// </summary>
    /// <param name="player">Player sending the event.</param>
    /// <param name="gameState">Gamestate of the player sending the event.</param>
    private void CheckNextRound(PhotonPlayer player, GameControl.GAMESTATE gameState)
    {
        if (PhotonNetwork.isMasterClient) {
            foreach (PhotonPlayer p in PhotonNetwork.otherPlayers)
            {
                object value;
                if (p.customProperties.TryGetValue("gameState", out value))
                {
                    int stateInt = (int) value;
                    GameControl.GAMESTATE state = (GameControl.GAMESTATE)stateInt;
                    if (state != GameControl.GAMESTATE.MissionCompleted)
                    {
                        return;
                    }
                }
            }
            print("next player has to become groupmaster");
            int masterIndex = Array.IndexOf(PhotonNetwork.playerList, PhotonNetwork.player);
            if (masterIndex + 1 < PhotonNetwork.playerList.Length)
            {
                PhotonPlayer newMasterClient = PhotonNetwork.playerList[masterIndex + 1];
                PhotonNetwork.SetMasterClient(newMasterClient);
                GameControl.pObject.GameState = GameControl.GAMESTATE.WaitingInGroup;
            }
            else
            {
                print("everyone has been masterclient");
            }
        }
    }
    #endregion
}
