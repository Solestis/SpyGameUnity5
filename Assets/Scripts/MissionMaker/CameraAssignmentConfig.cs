using ExitGames.Client.Photon;
using System;
using UnityEngine.UI;

/// <summary>
///  Config script to be added to the assignmentMaker for configuring the CameraAssignment.
/// </summary>
public class CameraAssignmentConfig : AssignmentConfig {

    /// <summary>
    /// Configures the assignmentmaker window to show title and to only show relevant UI elements.
    /// </summary>
	public override void ConfigureAssignment () {
		print ("configuring camera!");
		assignmentMaker.titleText.text = "Camera Opdracht!";
        assignmentMaker.assignmentExplanation.text = "Maak foto's van de aangegeven objecten. Wanneer je een foto neemt kan je kiezen deze te houden of af te keuren.";
		assignmentMaker.typeInputField.transform.parent.gameObject.SetActive(true);
		assignmentMaker.typeInputFieldDescription.text = "Hoeveel foto's?";
		assignmentMaker.typeInputField.placeholder.GetComponent<Text> ().text = "Getal tussen 1 en 10";
		assignmentMaker.typeInputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Integer;
	}

    /// <summary>
    /// Saves the assignment data in a hashtable and creates a new assignment and adds it to the AssignmentObject.
    /// </summary>
	public override void FinalizeAssignment () {
		Hashtable h = new Hashtable ();
		h.Add ("photosNeeded", Int32.Parse(assignmentMaker.typeInputField.text));
		h.Add ("missionDescription", assignmentMaker.descriptionField.text + ". " + assignmentMaker.assignmentExplanation.text);
		Assignment newCameraAssignment = new Assignment (0,"Camera", h);
		assignmentMaker.assignmentObject.AddAssignment(newCameraAssignment);
	}
}
