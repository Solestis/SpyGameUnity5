using UnityEngine;
using UnityEngine.UI;

public class NoneAssignment : UIElement
{
    #region Linked in Editor
    public Button finishButton;
    #endregion

    private Assignment assignment;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, fetches the active assignment from MissionControl, starts the assignment.
    /// </summary>
    public void Start()
    {
        onActivate += InitAssignment;
        finishButton.onClick.AddListener(() => { FinishAssignment(); });
    }
    #endregion

    #region Private Methods
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
    /// Calls FinishAssignment on the assignment in MissionControl.
    /// </summary>
    public void FinishAssignment()
    {
        assignment.FinishAssignment();
        Deactivate();
        MissionControl.pObject.ContinueMission();
    }
    #endregion
}