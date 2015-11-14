using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoginUser : UIWindow {

    #region Linked in Editor
    public InputField userNameField;
    public InputField passwordField;
    public Text errorText;
    public GameObject loginInProgress;
    public Text loginProgressText;
    public Button loginButton;
    #endregion

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, checks if login data is stored. If true, login automatically.
    /// </summary>
    override public void Start()
    {
        base.Start();

        loginButton.onClick.AddListener(() => { LoginWithPlayfab(); });

        PlayFabControl.pObject.onLoginResult += OnLoginResult;

        if (PlayFabData.SkipLogin && PlayFabData.AuthKey != null)
        {
            loginInProgress.SetActive(true);
        }
        if (PlayFabData.AuthKey != null) {
            loginInProgress.SetActive(true);
            loginProgressText.text = "Logged in";
        }
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles the result of the login message.
    /// </summary>
    /// <param name="error">Content of the error, if any.</param>
    /// <param name="textMessage">Optional text message.</param>
    private void OnLoginResult(PlayFabError error, string textMessage)
    {
        loginProgressText.text = textMessage;
        if (error != null)
        {
            loginInProgress.SetActive(false);
            errorText.gameObject.SetActive(true);
            errorText.text = error.ErrorMessage;
        }
        else
        {
            this.Deactivate();
        }
    } 
    #endregion

    #region Button Methods
    /// <summary>
    /// Uses the content of the userNameField and passwordField to log in to Playfab.
    /// </summary>
    public void LoginWithPlayfab()
    {
        if (PlayFabData.AuthKey == null)
        {
            errorText.gameObject.SetActive(false);
            loginInProgress.SetActive(true);
            PlayFabControl.pObject.LoginWithPlayfab(userNameField.text, passwordField.text);
        }
    }
    #endregion
}
