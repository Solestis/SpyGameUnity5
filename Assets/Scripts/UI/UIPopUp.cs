using System;
using UnityEngine.UI;

/// <summary>
/// Generic configurable Popupwindow to be used with UICanvas.
/// </summary>
public class UIPopUp : UIWindow
{
    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, calls UIWindow.Start() and set onDeactivate Action.
    /// </summary>
    override public void Start()
    {
        base.Start();
        onDeactivate += DestroyPopUp;
    }

    /// <summary>
    /// Adds an action to be called whenever the Popup is closed.
    /// </summary>
    /// <param name="action">The action to be added to onDeactivate</param>
    public void AddDeactivateAction(Action action)
    {
        onDeactivate += action;
    }

    /// <summary>
    /// Destroys the gameObject of this Popup.
    /// </summary>
    private void DestroyPopUp()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
