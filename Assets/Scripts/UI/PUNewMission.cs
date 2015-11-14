public class PUNewMission : UIPopUp {

    #region Button Methods
    /// <summary>
    /// Close this popup, and tell MissionControl to load the mission.
    /// </summary>
    public void LoadMissionScene()
    {
        Deactivate();
        MissionControl.pObject.ContinueMission();
    } 
    #endregion
}