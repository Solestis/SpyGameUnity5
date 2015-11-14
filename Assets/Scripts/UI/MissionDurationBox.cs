using Kender.uGUI;
using UnityEngine;

/// <summary>
/// Helper script to setup the combobox for missionduration in the MissionMaker.
/// </summary>
public class MissionDurationBox : MonoBehaviour {

    #region Linked in Editor
    public ComboBox comboBox;
    public MissionMaker missionMaker;
    #endregion

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, configure the items in the combobox and their effects when selected.
    /// </summary>
    public void Start()
    {
        ComboBoxItem oneHourItem = new ComboBoxItem("1 uur");
        ComboBoxItem twoHourItem = new ComboBoxItem("2 uur");
        ComboBoxItem threeHourItem = new ComboBoxItem("3 uur!");
        oneHourItem.OnSelect += () =>
        {
            missionMaker.missionDuration = 1;
        };
        twoHourItem.OnSelect += () =>
        {
            missionMaker.missionDuration = 2;
        };
        threeHourItem.OnSelect += () =>
        {
            missionMaker.missionDuration = 3;
        };
        comboBox.AddItems(oneHourItem, twoHourItem, threeHourItem);
        comboBox.SelectedIndex = 0;
        comboBox.ItemsToDisplay = 3;
    } 
    #endregion
}
