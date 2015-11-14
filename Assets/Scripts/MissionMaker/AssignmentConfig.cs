using ExitGames.Client.Photon;
using System;
using UnityEngine;

/// <summary>
/// Base script, not used for actual assignment configs. Config script to be added to the assignmentMaker for configuring the NoneAssignment.
/// </summary>
public class AssignmentConfig : MonoBehaviour {

	protected WIAssignmentMaker assignmentMaker;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, get the WIAssignmentMaker component on this GameObject and store it.
    /// </summary>
    public void Start()
    {
        assignmentMaker = this.gameObject.GetComponent<WIAssignmentMaker>();
        SetActions();
    } 
    #endregion

    #region Public Methods
    /// <summary>
    /// Configures the assignmentmaker popup to show title and to only show relevant UI elements.
    /// </summary>
    public virtual void ConfigureAssignment()
    {
        print("configuring basic!");
        assignmentMaker.titleText.text = "Opdracht!";
        assignmentMaker.assignmentExplanation.text = "Dit is een lege test opdracht.";

    }

    /// <summary>
    /// Saves the assignment data in a hashtable and creates a new assignment and adds it to the AssignmentObject.
    /// </summary>
    public virtual void FinalizeAssignment()
    {
        Debug.Log("Finishing Assignment");
        Hashtable h = new Hashtable();
        h.Add("missionDescription", assignmentMaker.descriptionField.text + ". " + assignmentMaker.assignmentExplanation.text);
        Assignment newAssignment = new Assignment(0, "None", h);
        assignmentMaker.assignmentObject.AddAssignment(newAssignment);
    }

    /// <summary>
    /// Cancels the creation of the assignment, destroys this script.
    /// </summary>
    public virtual void CancelAssignment()
    {
        Debug.Log("Canceling");
        Destroy(this);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Sets event handlers for the Actions in assignmentMaker.
    /// </summary>
    protected virtual void SetActions()
    {
        Debug.Log("Setting Actions");
        assignmentMaker.onActivate += ConfigureAssignment;
        assignmentMaker.onDeactivate += CancelAssignment;
        assignmentMaker.onFinish += FinalizeAssignment;
    }
    #endregion
}