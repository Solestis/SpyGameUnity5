using Kender.uGUI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI logic for the AssignmentMaker window.
/// </summary>
public class WIAssignmentMaker : UIWindow {

    #region Linked in Editor
    public InputField descriptionField;
    public InputField typeInputField;
    public Text typeInputFieldDescription;
    public Text assignmentExplanation;
    public ComboBox dropDownMenu;
    public Text dropDownMenuDescription;
    public Button doneButton;
    #endregion

    public AssignmentObject assignmentObject { get; private set; }
    public Action onFinish;
    public Action onEdit;

	private string typeScriptName;
    private Button editButton;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, add button listeners.
    /// </summary>
    public override void Start()
    {
        AddButtonListeners();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds the script for the assignment type to this gameObject and calls ConfigureAssignment on the script with a delay.
    /// </summary>
    /// <param name="type">The type of the assignment.</param>
    /// <param name="aObject">The AssignmentObject that called this method.</param>
    public void ConfigureNewAssignment(string type, AssignmentObject aObject)
    {
        assignmentObject = aObject;
        editButton = assignmentObject.GetComponent<Button>();
        editButton.onClick.AddListener(() => { ChangeAssignment(); });
        typeScriptName = type + "AssignmentConfig";
        if (type == null) type = "None";
        print("script added: " + typeScriptName);
        this.gameObject.AddComponent(Type.GetType(typeScriptName));
        Invoke("Activate", 0.1f);
    }

    /// <summary>
    /// Resets the state of the window to its default.
    /// </summary>
    public void Reset()
    {
        titleText.text = "Opdracht:";
        descriptionField.text = "";
        assignmentExplanation.text = "";
        typeInputField.transform.parent.gameObject.SetActive(false);
        typeInputField.text = "";
        typeInputField.placeholder.GetComponent<Text>().text = "Text...";
        typeInputFieldDescription.text = "InputTextDescription";
        dropDownMenu.transform.parent.gameObject.SetActive(false);
        dropDownMenuDescription.text = "DropDownMenuDescription";
        onActivate = null;
        onEdit = null;
        onFinish = null;
        onDeactivate = null;
    } 
    #endregion

    #region Button Methods
    /// <summary>
    /// Sends "FinalizeAssignment" to the assignment config script also present on this GameObject.
    /// </summary>
    public void CreateAssignment()
    {
        if (onFinish != null)
        {
            onFinish();
        }
        Deactivate();
        Reset();
    }

    /// <summary>
    /// Sends "EditAssignment" to the assignment config script also present on this GameObject.
    /// </summary>
    public void ChangeAssignment()
    {
        Debug.Log("Trying to edit the assignment!");
        if (onEdit != null)
        {
            onEdit();
        }
    }

    /// <summary>
    /// Destroys the assignment config script also present on this GameObject, destroys the assignmentObject, calls Reset() and closes the Popup.
    /// </summary>
    public void CancelAssignment()
    {
        Destroy(assignmentObject.gameObject);
        Deactivate();
        Reset();
    }
    #endregion

    #region Protected Methods
    /// <summary>
    /// Add button listeners.
    /// </summary>
    protected override void AddButtonListeners()
    {
        doneButton.onClick.AddListener(() => { CreateAssignment(); });
        closeButton.onClick.AddListener(() => { CancelAssignment(); });
    }
    #endregion
}
