using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// User Interface logic for the NewCharacter UIElement (Unfinished).
/// </summary>
public class UINewCharacter : MonoBehaviour {

    public Button femaleCharacterChoice;
    public Button maleCharacterChoice;
    public Button loginButton;
    public UIWindow loginWindow;

    private string gender;

	// Use this for initialization
	void Start () {
        AddButtonListeners();
	}

    private void AddButtonListeners()
    {
        femaleCharacterChoice.onClick.AddListener(() => { newCharacterWindow("female"); });
        maleCharacterChoice.onClick.AddListener(() => { newCharacterWindow("male"); });
        loginButton.onClick.AddListener(() => { ShowLoginWindow(); });
    }

    private void newCharacterWindow(string characterGender)
    {
        gender = characterGender;
        print("gender: " + gender);
        print("Show new character window");
    }

    private void ShowLoginWindow()
    {
        print("Show login window");
    }
}
