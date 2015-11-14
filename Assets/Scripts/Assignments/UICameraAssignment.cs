using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles the logic for the interface of the CameraAssignment.
/// </summary>
public class UICameraAssignment : UIElement
{
    #region Linked in Editor
    public RawImage displayImage;
    public GameObject photoContainer;
    public GameObject confirmationPU;
    public Text photosNeededText;
    public Button finishButton;
    public Button takePhotoButton;
    public Button confirmButton;
    public Button declineButton;
    #endregion

    private WebCamTexture webCamTexture;
    private Texture2D latestPhoto;
    private Assignment cameraAssignment;
    private int photosNeeded;
    private int photoCounter = 0;
    private List<Texture2D> photos;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, registers action events, adds button listeners and if active intializes the assignment.
    /// </summary>
    public void Start()
    {
        onActivate += InitAssignment;
        onDeactivate += OnDeactivate;
        AddButtonListeners();
        if (isActive)
        {
            InitAssignment();
        }
    }
    #endregion

    #region Button Methods    
    /// <summary>
    /// Saves the pixels of the camera texture in a Texture2D, sets the latest photo on displayImage, activates the confirmation popup.
    /// </summary>
    public void TakePhoto()
    {
        latestPhoto = new Texture2D(webCamTexture.width, webCamTexture.height);
        latestPhoto.SetPixels(webCamTexture.GetPixels());
        latestPhoto.Apply();
        displayImage.texture = latestPhoto;
        confirmationPU.SetActive(true);
    }

    /// <summary>
    /// Calls FinishAssignment in the assignment itself, resets this assignment and continues to the next assignment.
    /// </summary>
    public void FinishAssignment()
    {
        cameraAssignment.FinishAssignment();
        Reset();
        Deactivate();
        MissionControl.pObject.ContinueMission();
    }

    /// <summary>
    /// Sets the camera output on displayImage, increments the photocounter, calls Addphoto, disables the confirmation popup.
    /// </summary>
    public void ConfirmPhoto()
    {
        displayImage.texture = webCamTexture;
        confirmationPU.SetActive(false);
        photoCounter++;
        AddPhoto(latestPhoto);
    }

    /// <summary>
    /// Sets the camera output on displayImage, disables the confirmation popup.
    /// </summary>
    public void DeclinePhoto()
    {
        displayImage.texture = webCamTexture;
        confirmationPU.SetActive(false);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds listeners with methods to buttons in the scene.
    /// </summary>
    private void AddButtonListeners()
    {
        finishButton.onClick.AddListener(() => { FinishAssignment(); });
        takePhotoButton.onClick.AddListener(() => { TakePhoto(); });
        confirmButton.onClick.AddListener(() => { ConfirmPhoto(); });
        declineButton.onClick.AddListener(() => { DeclinePhoto(); });
    }

    /// <summary>
    /// Initializes the assignment by retrieving the active Assignment from MissionControl, then configuring the scene with Assignment data if assignmentState = STATE_NEW.
    /// </summary>
    private void InitAssignment()
    {
        if (MissionControl.pObject.activeMission != null && MissionControl.pObject.activeAssignment != null)
        {
            cameraAssignment = MissionControl.pObject.activeMission.activeAssignment;
        }
        else
        {
            cameraAssignment = new Assignment(0, "Camera");
            photosNeeded = 2;
        }
        if (cameraAssignment.assignmentState == Assignment.STATE_NEW)
        {
            photos = new List<Texture2D>();
            object value;
            if (cameraAssignment.typeData.TryGetValue("photosNeeded", out value))
            {
                photosNeeded = (int)value;
                Debug.Log("photosNeeded: " + photosNeeded);
            }
            else
            {
                Debug.Log("No photos needed amount found!");
            }
            photosNeededText.text = "Maak " + photosNeeded + " fotos!";
            webCamTexture = new WebCamTexture();
            displayImage.texture = webCamTexture;
            cameraAssignment.Start();
        }
        webCamTexture.Play();
    }

    /// <summary>
    /// Stops the webcamtexture from playing.
    /// </summary>
    private void OnDeactivate()
    {
        webCamTexture.Stop();
    }

    /// <summary>
    /// Adds a new photo image to the UI and adds the photo to the photos list. If the amount of photos equals the amount needed, enables the "Finish" button and disables the "Take Picture" button
    /// </summary>
    /// <param name="photoTexture">The photo texture.</param>
    private void AddPhoto(Texture2D photoTexture)
    {
        GameObject imageObject = new GameObject();
        imageObject.transform.parent = photoContainer.transform;
        imageObject.transform.localScale = Vector3.one;
        RawImage newRawImage = imageObject.AddComponent<RawImage>();
        newRawImage.texture = photoTexture;

        photos.Add(photoTexture);

        Debug.Log("Photos taken: " + photos.Count + " Photos needed: " + photosNeeded);
        photosNeededText.text = "Nog " + (photosNeeded - photoCounter) + " te nemen!";

        if (photos.Count == photosNeeded)
        {
            photosNeededText.text = "Alle foto's klaar om te verzenden.";
            takePhotoButton.gameObject.SetActive(false);
            finishButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Resets the scene of the assignment to its initial state, ready for the next assignment.
    /// </summary>
    private void Reset()
    {
        foreach (Transform child in photoContainer.transform)
        {
            Destroy(child.gameObject);
        }
        takePhotoButton.gameObject.SetActive(true);
        finishButton.gameObject.SetActive(false);
        photoCounter = 0;
    }
    #endregion
}