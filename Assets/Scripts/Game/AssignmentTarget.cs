using UnityEngine;

/// <summary>
/// Script used on the target object of the WalkAssignment.
/// </summary>
public class AssignmentTarget : MonoBehaviour {

    public UIWalkAssignment assignmentUI;

	/// <summary>
	/// On Collision with another GameObject with a collider, tells MissionControl to load the scene of the assignment currently active.
	/// </summary>
	/// <param name="other">The collider of the GameObject that this GameObject collided with, provided by the Unity physics engine.</param>
	void OnTriggerEnter (Collider other) {
		print ("Target reached!");
        assignmentUI.FinishAssignment();
	}
}
