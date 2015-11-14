using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Group of UIElements that manages its own active UIElement.
/// </summary>
public class UIGroup : MonoBehaviour
{
    public string startUIName = "";

    private Dictionary<string, UIElement> uiElementDictionary;
    private UIElement activeUI = null;
    private UIElement previousUI;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, collect all the UIElements in children.
    /// </summary>
    public void Start()
    {
        UIElement[] uiElements = gameObject.GetComponentsInChildren<UIElement>();
        uiElementDictionary = new Dictionary<string, UIElement>();
        foreach (UIElement ui in uiElements)
        {
            if (ui.inDictionary)
            {
                uiElementDictionary.Add(ui.name, ui);
                Debug.Log(ui.name + " is in child dictionary of: " + this.name);
                if (startUIName == ui.name)
                {
                    activeUI = ui;
                }
            }
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Activates this UIGroup by activating the currently active UIElement.
    /// </summary>
    public void Activate()
    {
        activeUI.Activate();
    }

    /// <summary>
    /// Deactivates this UIGroup by deactivating the currently active UIElement.
    /// </summary>
    public void Deactivate()
    {
        if (activeUI != null)
        {
            activeUI.Deactivate();
        }
    }

    /// <summary>
    /// Checks if the UIGroup has an active UIElement.
    /// </summary>
    /// <returns>Boolean if the UIGroup has an active UIElement or not.</returns>
    public bool HasActiveUI()
    {
        if (activeUI == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Clears the active UIElement of this UIGroup.
    /// </summary>
    public void ClearActiveUI()
    {
        activeUI = null;
    }

    /// <summary>
    /// Set a specific UIElement to be the active UIElement in the UIGroup, optionally making it visible as well.
    /// </summary>
    /// <param name="uiName">String name of the UIElemen to be active.</param>
    /// <param name="activate">Boolean to determine if the UIElement should be made visible as well.</param>
    public void SetActiveUI(string uiName, bool activate = true)
    {
        UIElement uiElement;
        if (uiElementDictionary.TryGetValue(uiName, out uiElement) && uiElement != activeUI)
        {
            if (activate)
            {
                foreach (KeyValuePair<string, UIElement> ui in uiElementDictionary)
                {
                    ui.Value.uiContent.SetActive(false); ;
                }
                uiElement.Activate();
            }
            activeUI = uiElement;
        } 
    }

    /// <summary>
    /// Switch to a UIElement by using its object reference.
    /// </summary>
    /// <param name="nextUI">The UI Element to navigate to.</param>
    public void NavToUI(UIElement nextUI)
    {
        if (nextUI != activeUI)
        {
            previousUI = activeUI;
            activeUI = nextUI;
            Debug.Log("PreviousUI: " + previousUI);
            Debug.Log("ActiveUI: " + nextUI);
            previousUI.Deactivate();
            activeUI.Activate();
        }
    }

    /// <summary>
    /// Switch to a UI Element by using its string name.
    /// </summary>
    /// <param name="nextUIName">The UI Element to navigate to.</param>
    public void NavToUI(string nextUIName)
    {
        UIElement nextUI;
        if (uiElementDictionary.TryGetValue(nextUIName, out nextUI))
        {
            NavToUI(nextUI);
        }
    }

    /// <summary>
    /// Switches to the previous UI element.
    /// </summary>
    public void NavBack()
    {
        NavToUI(previousUI);
    }
    #endregion
}
