using ExitGames.Client.Photon;
using System;
using UnityEngine;

/// <summary>
///  Config script to be added to the assignmentMaker for configuring the ScanAssignment.
/// </summary>
public class ScanAssignmentConfig : AssignmentConfig {

    #region Public Methods
    /// <summary>
    /// Configures the assignmentmaker window to show title and to only show relevant UI elements.
    /// </summary>
    public override void ConfigureAssignment()
    {
        print("configuring scan!");
        assignmentMaker.titleText.text = "Scan Opdracht!";
        assignmentMaker.assignmentExplanation.text = "Kijk om je heen en vindt alle verborgen signalen. Tap de signalen als je ze gevonden hebt om ze te verwijderen.";
    }

    /// <summary>
    /// Saves the assignment data in a hashtable and creates a new assignment and adds it to the AssignmentObject.
    /// </summary>
    public override void FinalizeAssignment()
    {
        Debug.Log("Finishing Assignment");
        Hashtable h = new Hashtable();
        h.Add("missionDescription", assignmentMaker.descriptionField.text + ". " + assignmentMaker.assignmentExplanation.text);
        Assignment newAssignment = new Assignment(0, "Scan", h);
        assignmentMaker.assignmentObject.AddAssignment(newAssignment);
    }
    #endregion
}