using ExitGames.Client.Photon;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Config script to be added to the assignmentMaker for configuring the WalkAssignment.
/// </summary>
public class WalkAssignmentConfig : AssignmentConfig {

    /// <summary>
    /// Configures the assignmentmaker popup to show title and to only show relevant UI elements.
    /// </summary>
    public override void ConfigureAssignment()
    {
        print("configuring walk!");
        assignmentMaker.titleText.text = "Loop Opdracht!";
        assignmentMaker.assignmentExplanation.text = "Volg de pijl en loop naar de aangegeven locatie toe.";
        assignmentMaker.typeInputField.transform.parent.gameObject.SetActive(true);
        assignmentMaker.typeInputFieldDescription.text = "Loop radius?";
        assignmentMaker.typeInputField.placeholder.GetComponent<Text>().text = "Getal tussen 200 en 300";
        assignmentMaker.typeInputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Integer;
    }

    /// <summary>
    /// Saves the assignment data in a hashtable and creates a new assignment and adds it to the AssignmentObject.
    /// </summary>
    public override void FinalizeAssignment()
    {
        Hashtable h = new Hashtable();
        Vector2 newPosition = UnityEngine.Random.insideUnitCircle * Int32.Parse(assignmentMaker.typeInputField.text);
        Vector3 target = new Vector3(newPosition.x, 0, newPosition.y);
        h.Add("targetLocation", target);
        h.Add("missionDescription", assignmentMaker.descriptionField.text + ". " + assignmentMaker.assignmentExplanation.text);
        Assignment newWalkAssignment = new Assignment(0, "Walk", h);
        assignmentMaker.assignmentObject.AddAssignment(newWalkAssignment);
    }
}
