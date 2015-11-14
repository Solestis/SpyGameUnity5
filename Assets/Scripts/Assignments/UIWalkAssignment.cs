using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles the logic for the interface of the WalkAssignment.
/// </summary>
public class UIWalkAssignment : UIElement {

    #region Linked in Editor
    public Text distanceText;
    public GameObject backgroundGrid;
    public GameObject playerObject;
    public GameObject targetObject;
    public GameObject background;
    #endregion

	private float period = 1f;
	private float nextActionTime = 0.0f;
    private float distance;
    private Assignment walkAssignment;
    private Vector3 targetLocation;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, registers action events, adds button listeners, sets this as a variable in the AssignmentTarget and if active intializes the assignment.
    /// </summary>
    public void Start()
    {
        onActivate += InitAssignment;
        onDeactivate += OnDeactivate;
        targetObject.GetComponent<AssignmentTarget>().assignmentUI = this;
        if (isActive)
        {
            InitAssignment();
        }
    }

    /// <summary>
    /// On Monobehaviour Update, updates the Time and Distance text on the UI every second.
    /// </summary>
    void Update()
    {
        if (Time.time > nextActionTime && this.isActive)
        {
            //Update the distance to target
            distance = GetTargetDistance();
            distanceText.text = "Afstand: " + distance.ToString("F1") + "m";

            //Add 1 second to new nextActionTime;
            nextActionTime = Time.time + period;
        }
    } 
    #endregion

    #region Public Methods
    /// <summary>
    /// Calls FinishAssignment in the assignment itself, deactivates the background for this assignment and continues to the next assignment.
    /// </summary>
	public void FinishAssignment () {
        walkAssignment.FinishAssignment();
        backgroundGrid.SetActive(false);
        Deactivate();
        MissionControl.pObject.ContinueMission();
	}
    #endregion

    #region Private Methods
    /// <summary>
    /// Initializes the assignment by retrieving the active Assignment from MissionControl, then configuring the scene with Assignment data if assignmentState = STATE_NEW.
    /// </summary>
    private void InitAssignment()
    {
        if (MissionControl.pObject.activeMission != null && MissionControl.pObject.activeAssignment != null)
        {
            walkAssignment = MissionControl.pObject.activeMission.activeAssignment;
        }
        else
        {
            print("Creating test assignment");
            walkAssignment = new Assignment(0, "Walk");
            Vector2 newPosition = UnityEngine.Random.insideUnitCircle * 300;
            targetLocation = new Vector3(newPosition.x, 0, newPosition.y);
        }
        if (walkAssignment.assignmentState == Assignment.STATE_NEW)
        {
            object value;
            if (walkAssignment.typeData.TryGetValue("targetLocation", out value))
            {
                targetLocation = (Vector3)value;
                Debug.Log("targetLocation: " + targetLocation);
            }
            else
            {
                Debug.Log("No target location found!");
            }

            walkAssignment.Start();
        }
        targetObject.transform.position = targetLocation;
        background.SetActive(false);
        backgroundGrid.SetActive(true);
    }

    /// <summary>
    /// Reactivates the standard background, deactivates the background for this assignment.
    /// </summary>
    private void OnDeactivate()
    {
        background.SetActive(true);
        backgroundGrid.SetActive(false);
    }

    /// <summary>
    /// Calculates the distance between the player and the mission target of the current active assignment in the mission and returns this as a float.
    /// </summary>
    /// <returns>Distance between playerObject and targetObject.</returns>
    private float GetTargetDistance()
    {
        float distance = Vector3.Distance(playerObject.transform.position, targetObject.transform.position);
        return distance;
    }
    #endregion
}
