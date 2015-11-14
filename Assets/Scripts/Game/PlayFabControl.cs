using UnityEngine;
using System;
using System.Collections;
using PlayFab.ClientModels;
using PlayFab;

/// <summary>
/// Singleton Class, handles all events for the Playfab multiplayer service.
/// </summary>
public class PlayFabControl : MonoBehaviour {

    public static PlayFabControl pObject;

    public bool isConnected { get; private set; }
    public Action<PlayFabError, string> onLoginResult;
    public Action<PlayFabError, string> onRegisterResult;
    public Action<PlayFabError, string> onAddUsernamePasswordResult;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Awake, Checks if GameControl is already present, if false, makes it persistent, if true destroys the new object and keeps the old persistent one.
    /// </summary>
    void Awake()
    {
        if (pObject == null)
        {
            DontDestroyOnLoad(gameObject);
            pObject = this;
        }
        else if (pObject != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// On Monobehaviour Start, login with device ID (Currently disabled).
    /// </summary>
    public void Start()
    {
        //LoginWithDeviceID();
    } 
    #endregion

    #region Public Methods
    /// <summary>
    /// Uses DeviceID to login with PlayFab.
    /// </summary>
    public void LoginWithDeviceID()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                print("Android");
                LoginWithAndroidDeviceIDRequest request = new LoginWithAndroidDeviceIDRequest();
                request.AndroidDeviceId = SystemInfo.deviceUniqueIdentifier;
                request.CreateAccount = true;
                request.TitleId = PlayFabData.TitleId;
                PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginResult, OnPlayFabError);
                break;
            case RuntimePlatform.IPhonePlayer:
                print("iOS");
                break;
            default:
                print("Other platforms");
                break;
        }
    }

    /// <summary>
    /// Sends a loginrequest to playfab using username and password.
    /// </summary>
    /// <param name="username">The username of the account that is logging in.</param>
    /// <param name="password">The password of the account that is logging in.</param>
    public void LoginWithPlayfab(string username, string password)
    {
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
        request.Username = username;
        request.Password = password;
        request.TitleId = PlayFabData.TitleId;
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginResult, OnPlayFabError);
    }

    /// <summary>
    /// Sends a request to make a new Playfab account.
    /// </summary>
    /// <param name="username">The username of the account that is being created.</param>
    /// <param name="email">E-mail for the new account.</param>
    /// <param name="password">The password of the account that is being created.</param>
    public void RegisterWithPlayfab(string username, string email, string password)
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.TitleId = PlayFabData.TitleId;
        request.Username = username;
        request.Email = email;
        request.Password = password;
        Debug.Log("TitleId : " + request.TitleId);
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterResult, OnPlayFabError);
    }

    /// <summary>
    /// Sends a request to add a Username and Password to an existing deviceID login.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public void AddUsernamePassword(string username, string password)
    {
        AddUsernamePasswordRequest request = new AddUsernamePasswordRequest();
        request.Username = username;
        request.Password = password;
        PlayFabClientAPI.AddUsernamePassword(request, OnAddUserNamePasswordResult, OnPlayFabError);
    }
    #endregion

    #region Playfab Event Handlers
    /// <summary>
    /// Called if player login is successful. Sends a request for the playerdata to the Playfab server, connects to Photon.
    /// </summary>
    /// <param name="result">LoginResult provided by the Playfab server.</param>
    public void OnLoginResult(LoginResult result)
    {
        if (onLoginResult != null) {
            onLoginResult(null, "Login succesful!");
        }
        PlayFabData.AuthKey = PlayFabClientAPI.AuthKey;
        PhotonControl.pObject.ConnectToPhoton();
        GetUserCombinedInfo();
    }

    /// <summary>
    /// Called if adding Username and Password is succesful. Sends a request for the playerdata to the Playfab server, connects to Photon.
    /// </summary>
    /// <param name="result">AddUsernamePasswordResult provided by the Playfab server.</param>
    public void OnAddUserNamePasswordResult(AddUsernamePasswordResult result)
    {
        if (onAddUsernamePasswordResult != null)
        {
            onAddUsernamePasswordResult(null, "Login succesful!");
        }
        PlayFabData.AuthKey = PlayFabClientAPI.AuthKey;
        PhotonControl.pObject.ConnectToPhoton();
        GetUserCombinedInfo();
    }

    /// <summary>
    /// Called if registering an account is succesful.
    /// </summary>
    /// <param name="result">RegisterPlayFabUserResult provided by the Playfab server.</param>
    public void OnRegisterResult(RegisterPlayFabUserResult result)
    {
        if (onRegisterResult != null)
        {
            onRegisterResult(null, "Registration succesful!");
        }
        PlayFabData.AuthKey = PlayFabClientAPI.AuthKey;
        PhotonControl.pObject.ConnectToPhoton();
        GetUserCombinedInfo();
    }

    /// <summary>
    /// Called if playerdata is succesfully retrieved. Saves the player data and loads the MainMenu scene.
    /// </summary>
    /// <param name="result">GetUserCombinedInfoResult provided by the Playfab server.</param>
    public void OnGetUserInfoResult(GetUserCombinedInfoResult result)
    {
        PhotonControl.pObject.SetPlayerData(result.AccountInfo, result.Data);
        isConnected = true;
    }

    /// <summary>
    /// Called if there is an error, display appropriate error message.
    /// </summary>
    /// <param name="error">PlayFabError provided by the Playfab server.</param>
    void OnPlayFabError(PlayFabError error)
    {
        if (onLoginResult != null)
        {
            onLoginResult(error, "Login error");
        }
        if (onRegisterResult != null)
        {
            onRegisterResult(error, "Registration error");
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Sends a request to the Playfab server to retrieve user account info.
    /// </summary>
    private void GetUserCombinedInfo()
    {
        GetUserCombinedInfoRequest request = new GetUserCombinedInfoRequest();
        PlayFabClientAPI.GetUserCombinedInfo(request, OnGetUserInfoResult, OnPlayFabError);
    }
    #endregion
}
