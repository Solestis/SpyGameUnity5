using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI logic for the MissionMaker.
/// </summary>
public class MissionMaker : UIElement {

    #region Linked in Editor
    public GameObject assignmentContainer;
    public UIWindow missionDataWindow;
    public Button finishMissionButton;
    public Button editMissionButton;
    #endregion

    public int missionDuration = 1;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, add button listeners and add event listeners.
    /// </summary>
    public void Start()
    {
        AddButtonListeners();
        missionDataWindow.onDeactivate += ChangeMissionTitle;
    }
    #endregion

    #region Button Methods
    /// <summary>
    /// Collects all assignments of the placed assignmentObjects in the assignmentContainer. Generates a new mission with these assignments, saves it as a Hashtable and sends it over the PhotonNetwork.
    /// </summary>
    public void FinalizeMission()
    {
        if (assignmentContainer.transform.childCount > 0)
        {
            if (missionDataWindow.titleText.text == "")
            {
                ToggleMissionDataWindow();
                missionDataWindow.onDeactivate -= FinalizeMission;
                missionDataWindow.onDeactivate += FinalizeMission;
            }
            else
            {
                missionDataWindow.onDeactivate -= FinalizeMission;
                AssignmentObject[] assignmentObjects = assignmentContainer.transform.GetComponentsInChildren<AssignmentObject>();
                Mission newMission = MissionControl.pObject.CreateMissionFromAssignmentObjects(assignmentObjects, missionDuration);
                newMission.SetMissionData(missionDataWindow.titleText.text, missionDataWindow.descriptionText.text, missionDuration);
                PhotonControl.pObject.SendMission(newMission);
                Reset();
                UICanvas.canvas.NavToUIGroup("UIRoom");
            }
        }
        else
        {
            print("No assignments set!");
        }
    }

    /// <summary>
    /// Toggles the visibility of the Mission data window.
    /// </summary>
    public void ToggleMissionDataWindow()
    {
        if (missionDataWindow.isActive)
        {
            missionDataWindow.Deactivate();
        }
        else
        {
            missionDataWindow.Activate();
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Add button listeners.
    /// </summary>
    private void AddButtonListeners()
    {
        editMissionButton.onClick.AddListener(() => { ToggleMissionDataWindow(); });
        finishMissionButton.onClick.AddListener(() => { FinalizeMission(); });
    }

    /// <summary>
    /// Change the text in the UI for the mission title that has been set in the mission data window.
    /// </summary>
    private void ChangeMissionTitle()
    {
        if (missionDataWindow.titleText.text != "")
        {
            editMissionButton.transform.FindChild("Text").GetComponent<Text>().text = missionDataWindow.titleText.text;
        }
    }

    /// <summary>
    /// Reset the MissionMaker to its default state.
    /// </summary>
    private void Reset()
    {
        InputField titleField = missionDataWindow.titleText.transform.parent.GetComponent<InputField>();
        titleField.text = "";
        InputField descriptionField = missionDataWindow.descriptionText.transform.parent.GetComponent<InputField>();
        descriptionField.text = "";
        editMissionButton.transform.FindChild("Text").GetComponent<Text>().text = "Geef je missie een naam";
        foreach (Transform child in assignmentContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion
}

