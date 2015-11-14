using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that controls the player object in the WalkAssignment.
/// </summary>
public class PlayerController : MonoBehaviour {

    #region Set in Editor
    public float accuracy = 1f;
    public float updateDistance = 1f;
    public Text debugtext;
    public LineRenderer lineRenderer;
    public GameObject headingIndicator;
    public GameObject target;
    public GameObject targetIndicator; 
    #endregion
	
	private float startLatitudeRad;
	private Vector3 startLocation;
	private Vector3 previousLinePoint;
	private int lineCount = 1;
	private float nextActionTime = 0.0f;
	private float period = 0.1f;
	private float radius = 6371000;
	private float xAnglePrev;


    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, check if user has location service enabled, if true, start the service.
    /// </summary>
    public void Start()
    {
        debugtext = UICanvas.canvas.debugText;

        if (!Input.location.isEnabledByUser) return;

        Input.location.Start(accuracy, updateDistance);
        Input.compass.enabled = true;
        debugtext.text = "Starting GPS";

        StartCoroutine(StartLocationService());
    }

    /// <summary>
    /// On Monobehaviour Update, update player position, heading and targetindicator.
    /// </summary>
    public void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            UpdatePlayerPosition();
            UpdatePlayerHeading();
            UpdateTargetIndicator();
        }
    } 
    #endregion

    #region Coroutines
    /// <summary>
    /// Delayed coroutine for starting the location service.
    /// </summary>
    IEnumerator StartLocationService()
    {
        int maxWait = 20;

        // Wait until service initializes
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            print("Waiting...");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            debugtext.text = "Timed out";
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            debugtext.text = "Unable to determine device location";
        }

        // Access granted and location value could be retrieved
        else
        {
            debugtext.text = "Fetching GPS data";

            //Waiting 10 seconds to stabilize gps data.
            yield return new WaitForSeconds(10);

            //Calculating startposition and projecting GPS coordinates on 3D space.
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            float latitudeRad = latitude * Mathf.Deg2Rad;
            float longitudeRad = longitude * Mathf.Deg2Rad;
            float x = radius * longitudeRad * Mathf.Cos(latitudeRad);
            float z = radius * latitudeRad;
            startLatitudeRad = latitudeRad;
            startLocation = new Vector3(x, 0, z);

            //Setting initial line point
            lineRenderer.SetVertexCount(lineCount);
            lineCount++;
            lineRenderer.SetPosition(0, new Vector3(0, 0, 0));

            debugtext.text = "Go!";
        }
    }

    /// <summary>
    /// Coroutine to smooth player movement.
    /// </summary>
    /// <param name="newPosition"></param>
    protected IEnumerator LerpPlayerPosition(Vector3 newPosition)
    {
        float t = 0;
        Vector3 oldPosition = transform.position;
        while (t < 5)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(oldPosition, newPosition, t / 5);
            yield return null;
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Calculate the difference between the current and previous position of the player, move the player to the new position, draw a line between both positions.
    /// </summary>
    private void UpdatePlayerPosition()
    {
        //Every period (default 1 second) update player location on the map and draw a line from previous location to new location.
        if (startLocation != Vector3.zero)
        {
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            float latitudeRad = latitude * Mathf.Deg2Rad;
            float longitudeRad = longitude * Mathf.Deg2Rad;
            float x = radius * longitudeRad * Mathf.Cos(startLatitudeRad);
            float z = radius * latitudeRad;
            Vector3 position = new Vector3(x, 0, z) - startLocation;
            if (position != transform.position)
            {
                StopCoroutine("LerpPlayerPosition");
                StartCoroutine(LerpPlayerPosition(position));
            }

            //If position changed, draw a connecting line.
            if (position != previousLinePoint)
            {
                lineRenderer.SetVertexCount(lineCount);
                lineRenderer.SetPosition(lineCount - 1, position);
                previousLinePoint = position;
                lineCount++;
            }

            debugtext.text = position.ToString();
        }
    }

    /// <summary>
    /// Rotate the camera to correspond with the player heading.
    /// </summary>
    private void UpdatePlayerHeading()
    {
        if (Input.compass.trueHeading < (headingIndicator.transform.eulerAngles.y - 5) || Input.compass.trueHeading > (headingIndicator.transform.eulerAngles.y + 5))
        {
            headingIndicator.transform.eulerAngles = new Vector3(headingIndicator.transform.eulerAngles.x, Input.compass.trueHeading, 0);
        }
    }

    /// <summary>
    /// Update the target arrow to point at the target object.
    /// </summary>
    private void UpdateTargetIndicator()
    {
        targetIndicator.transform.LookAt(target.transform.position);
    } 
    #endregion
}
