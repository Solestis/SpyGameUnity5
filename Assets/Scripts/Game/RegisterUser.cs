using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Currently unused, to be combined with LoginUser.
/// </summary>
public class RegisterUser : UIWindow
{

    #region Linked in Editor
    public InputField userNameField;
	public InputField emailField;
	public InputField passwordField;
	public InputField passwordRepeatField;
    public Button registerButton;
    public Button backButton;
    #endregion

    public LoginUser loginUser;

    #region Monobehaviour Event Handlers
    new public void Start()
    {
        base.Start();
        PlayFabControl.pObject.onRegisterResult += OnRegisterResult;
    }
    #endregion

    #region Private Methods
    new private void AddButtonListeners()
    {
        base.AddButtonListeners();
        registerButton.onClick.AddListener(() => { RegisterWithPlayfab(); });
    }

    private void OnRegisterResult(PlayFabError error, string textMessage)
    {
        Debug.Log(error.ErrorMessage);
    } 

    public void RegisterWithPlayfab()
    {
        if (passwordField.text == passwordRepeatField.text)
        {
            PlayFabControl.pObject.RegisterWithPlayfab(userNameField.text, emailField.text, passwordField.text);
        }
        else
        {
            Debug.Log("Passwords not the same!");
        }
    }
    #endregion
}
