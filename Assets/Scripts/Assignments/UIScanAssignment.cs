using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// This class handles the logic for the interface of the ScanAssignment.
/// </summary>
public class UIScanAssignment : UIElement
{
    #region Linked in Editor
    public GameObject playerCamera;
    public GameObject targetSignal;
    public GameObject content3D;
    public GameObject cameraPlane;
    public GameObject background;
    #endregion

    private float xAnglePrev;
    private float xAngleNew;
    private float yAnglePrev;
    private float yAngleNew;

    private bool xRotateOn;
    private bool yRotateOn;

    private WebCamTexture webCamTexture;

    private int targetsLeft = 5;
    private Assignment scanAssignment;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, registers action events, adds button listeners and if active intializes the assignment.
    /// </summary>
    public void Start()
    {
        onActivate += InitAssignment;
        onDeactivate += OnDeactivate;
        if (isActive)
        {
            InitAssignment();
        }
    }

    /// <summary>
    /// If the gameObject is active, every frame update the rotation of the playercamera using the accelerometer and compass sensor.
    /// </summary>
    public void Update()
    {
        if (isActive) {
            float xAngle = Mathf.Atan2(Input.acceleration.z, Input.acceleration.y) * (180 / Mathf.PI) + 180;
            float yAngle = Input.compass.trueHeading;
            if (xAngle > (xAnglePrev + 2) || xAngle < (xAnglePrev - 2))
            {
                xAnglePrev = xAngle;
                xRotateOn = true;
            }
            else
            {
                xRotateOn = false;
            }

            if (yAngle > (yAnglePrev + 5) || yAngle < (yAnglePrev - 5))
            {
                yAnglePrev = yAngle;
                yRotateOn = true;
            }
            else
            {
                yRotateOn = false;
            }
            if (xRotateOn)
            {
                xAngleNew = xAngle;
            }
            if (yRotateOn)
            {
                yAngleNew = yAngle;
            }

            playerCamera.transform.eulerAngles = new Vector3(xAngleNew, yAngleNew, 0);
            //UICanvas.canvas.debugText.text = "xAngleNew: " + xAngleNew + ", yAngleNew: " + yAngleNew;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Counts how many targets are left to find, if no targets left, calls FinishAssignment();
    /// </summary>
    /// <param name="targetObject">The object being counted for targets scanned.</param>
    public void TargetScanned(GameObject targetObject)
    {
        if (targetsLeft > 1)
        {
            targetsLeft--;
        }
        else
        {
            FinishAssignment();
        }
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
            scanAssignment = MissionControl.pObject.activeMission.activeAssignment;
        }
        else
        {
            scanAssignment = new Assignment(0, "Scan");
        }
        if (scanAssignment.assignmentState == Assignment.STATE_NEW)
        {
            Input.compass.enabled = true;
            Input.location.Start();
            webCamTexture = new WebCamTexture();
            cameraPlane.GetComponent<Renderer>().material.mainTexture = webCamTexture;
            int radius = 100;
            for (int i = 1; i <= 5; i++)
            {
                float angle1 = (float)GameControl.random.NextDouble() * Mathf.PI;
                float angle2 = (float)GameControl.random.NextDouble() * Mathf.PI * 2;
                float x = radius * Mathf.Sin(angle1) * Mathf.Cos(angle2);
                float y = radius * Mathf.Sin(angle1) * Mathf.Sin(angle2);
                float z = radius * Mathf.Cos(angle1);
                Vector3 testVector = new Vector3(x, y, z);
                Debug.Log(testVector);
                GameObject newSphere = Instantiate(targetSignal);
                newSphere.transform.parent = content3D.transform;
                newSphere.transform.localPosition = testVector;
            }
            scanAssignment.Start();
        }
        background.SetActive(false);
        content3D.SetActive(true);
        webCamTexture.Play();  
    }

    /// <summary>
    /// Reactivates the standard background, deactivates the 3D content for this assignment and stops the webcamtexture from playing.
    /// </summary>
    private void OnDeactivate()
    {
        background.SetActive(true);
        content3D.SetActive(false);
        webCamTexture.Stop();
    }

    /// <summary>
    /// Calls FinishAssignment in the assignment itself, resets this assignment and continues to the next assignment.
    /// </summary>
    private void FinishAssignment()
    {
        scanAssignment.FinishAssignment();
        targetsLeft = 5;
        content3D.SetActive(false);
        Deactivate();
        MissionControl.pObject.ContinueMission();
    }
    #endregion
}