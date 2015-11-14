using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Extension of UIElement that allows for windows overlaying other UIElements.
/// </summary>
public class UIWindow : UIElement {

    #region Linked in Editor
    public Text titleText;
    public Text descriptionText;
    public Button closeButton;
    #endregion

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, set inDictionary to false.
    /// </summary>
    public virtual void Awake()
    {
        inDictionary = false;
    }

    /// <summary>
    /// On Monobehaviour Start, add button listeners and set fadeDuration to 0 to avoid issues when navigating to other UIElements.
    /// </summary>
    public virtual void Start()
    {
        AddButtonListeners();
        fadeDuration = 0.0f;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Set the title and description of the popup.
    /// </summary>
    /// <param name="title">Title of the popup.</param>
    /// <param name="description">Description of the popup.</param>
    public void SetContents(string title, string description = "")
    {
        titleText.text = title;
        if (descriptionText != null)
        {
            descriptionText.text = description;
        }
    }
    #endregion

    #region Protected Methods
    /// <summary>
    /// Adds button listeners.
    /// </summary>
    protected virtual void AddButtonListeners()
    {
        closeButton.onClick.AddListener(() => { Deactivate(); });
    }
    #endregion
}
