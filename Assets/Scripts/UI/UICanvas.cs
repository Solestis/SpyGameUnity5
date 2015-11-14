using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller class for managing the UIGroups on the UnityUI Canvas.
/// </summary>
public class UICanvas : MonoBehaviour {

    #region Linked in Editor
    public Text debugText;
    public UIElement uiMainMenu;
    public Button mainMenuButton;
    #endregion

    public static UICanvas canvas;

    private Dictionary<string, UIGroup> uiGroupDictionary;
    private UIGroup activeGroup;
    private UIGroup previousGroup;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Awake, save a reference of this instance in a static variable.
    /// </summary>
    public void Awake()
    {
        UICanvas.canvas = this;
    }

    /// <summary>
    /// On Monobehaviour Start, register this UICanvas on GameControl.
    /// </summary>
    void Start()
    {
        UIGroup[] uiGroups = gameObject.GetComponentsInChildren<UIGroup>();
        uiGroupDictionary = new Dictionary<string, UIGroup>();
        foreach (UIGroup group in uiGroups)
        {
            uiGroupDictionary.Add(group.name, group);
            Debug.Log(group.name);
        }
        mainMenuButton.onClick.AddListener(() => { NavToMainMenu(); });
    } 
    #endregion

    #region Public Methods
    /// <summary>
    /// Switch to the main menu UIElement linked in Editor.
    /// </summary>
    public void NavToMainMenu()
    {
        activeGroup.Deactivate();
        activeGroup = null;
        uiMainMenu.Activate();
        mainMenuButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Switch to a UIGroup linked in editor by using a direct object reference.
    /// </summary>
    /// <param name="nextUIGroup">Reference to the UIGroup to swtich to.</param>
    /// <param name="clearPreviousGroup">Boolean to determine of the active UIElement in the previous UIGroup should be cleared.</param>
    public void NavToUIGroup(UIGroup nextUIGroup, bool clearPreviousGroup = false)
    {
        if (nextUIGroup != activeGroup)
        {
            previousGroup = activeGroup;
            activeGroup = nextUIGroup;
            Debug.Log("PreviousGroup: " + previousGroup);
            Debug.Log("ActiveGroup: " + nextUIGroup);
            if (previousGroup == null)
            {
                uiMainMenu.Deactivate();
            }
            else
            {
                previousGroup.Deactivate();
                if (clearPreviousGroup)
                {
                    previousGroup.ClearActiveUI();
                }
            }
            activeGroup.Activate();
            mainMenuButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Switch to a UIGroup linked in editor by using a string name, optionally activating a UIElement in this group as well.
    /// </summary>
    /// <param name="nextUIGroupName">String name of the next UIGroup to switch to.</param>
    /// <param name="nextUI">String of the next UIElement in the next UIGroup to navigate to.</param>
    /// <param name="clearPreviousGroup">Boolean to determine of the active UIElement in the previous UIGroup should be cleared.</param>
    public void NavToUIGroup(string nextUIGroupName, string nextUI = null, bool clearPreviousGroup = false)
    {
        UIGroup nextGroup;
        if (uiGroupDictionary.TryGetValue(nextUIGroupName, out nextGroup))
        {
            NavToUIGroup(nextGroup, clearPreviousGroup);
            if (nextUI != null)
            {
                nextGroup.SetActiveUI(nextUI);
            }
        }
    }

    /// <summary>
    /// Sets a UIElement in a UIGroup as the active UIElement.
    /// </summary>
    /// <param name="uiGroupName">String name of the UIGroup</param>
    /// <param name="uiName">String name of the UIElement to make active.</param>
    public void SetActiveUI(string uiGroupName, string uiName)
    {
        UIGroup nextGroup;
        if (uiGroupDictionary.TryGetValue(uiGroupName, out nextGroup))
        {
                nextGroup.SetActiveUI(uiName, false);
        }
    }

    /// <summary>
    /// Instantiate a new generic UIPopup.
    /// </summary>
    /// <param name="title">Title string to show on the Popup.</param>
    /// <param name="description">Description string to show on the Popup.</param>
    /// <param name="onClose">Action method to call when the Popup is closed.</param>
    public void NewPopUp(string title, string description = "", Action onClose = null)
    {
        GameObject popUpObject = Instantiate(Resources.Load("UIPopUp")) as GameObject;
        RectTransform rectTransform = popUpObject.GetComponent<RectTransform>();
        rectTransform.SetParent(this.gameObject.transform);
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        UIPopUp uiPopUp = popUpObject.GetComponent<UIPopUp>();
        uiPopUp.SetContents(title, description);
        if (onClose != null)
        {
            uiPopUp.AddDeactivateAction(onClose);
        }
    }
    #endregion
}
