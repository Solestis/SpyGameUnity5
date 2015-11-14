using UnityEngine;

public class PUAssignment : UIElement {

    /// <summary>
    /// Tells MissionControl to progress to the next assignment.
    /// </summary>
    #region Button Methods
    public void CompleteAssignment()
    {
        MissionControl.pObject.activeMission.NextAssignment();
        Destroy(this.gameObject);
    } 
    #endregion
}
