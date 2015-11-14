public class PUMissionComplete : UIPopUp
{
    #region Button Methods
    /// <summary>
    /// Tells MissionControl to complete the mission.
    /// </summary>
    public void CompleteMission()
    {
        Deactivate();
        MissionControl.pObject.CompleteMission();
    } 
    #endregion
}