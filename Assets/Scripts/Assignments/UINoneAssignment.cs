using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a class used for testing only, when an assignment without any content is needed.
/// </summary>
public class UINoneAssignment : UIElement
{
    #region Linked in Editor
    public Button finishButton;
    #endregion

    private Assignment assignment;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, registers action events, adds button listeners.
    /// </summary>
    public void Start()
    {
        onActivate += InitAssignment;
        finishButton.onClick.AddListener(() => { FinishAssignment(); });
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Fetches active assignment from MissionControl, starts assignment if assignmentState = STATE_NEW.
    /// </summary>
    private void InitAssignment()
    {
        if (MissionControl.pObject.activeMission != null && MissionControl.pObject.activeAssignment != null)
        {
            assignment = MissionControl.pObject.activeMission.activeAssignment;
        }
        else
        {
            assignment = new Assignment(0, "None");
        }
        if (assignment.assignmentState == Assignment.STATE_NEW)
        {

            assignment.Start();
        }
    }

    /// <summary>
    /// Calls FinishAssignment on the assignment in MissionControl. Continues to the next assignment.
    /// </summary>
    public void FinishAssignment()
    {
        assignment.FinishAssignment();
        Deactivate();
        MissionControl.pObject.ContinueMission();
    }
    #endregion
}