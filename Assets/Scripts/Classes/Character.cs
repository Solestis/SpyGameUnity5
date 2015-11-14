using ExitGames.Client.Photon;
using UnityEngine;

/// <summary>
/// Container class for the player character object. Holds the gameobject for the body of the character and all its properties.
/// </summary>
public class Character {

	public GameObject body { get; private set; }
	public Vector3 position { get { return body.transform.position; } set { body.transform.position = value; } }

    private string gender;
	private Hashtable playerProperties;
	private string genderModelName;

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="Character"/> class. Retrieves the gender and other properties of the character from the Hashtable.
    /// </summary>
    /// <param name="properties">Properties of the character, stored in a Hashtable</param>
    public Character(Hashtable properties)
    {
        playerProperties = properties;

        // Retrieving gender string from playerProperties
        object genderObject;
        playerProperties.TryGetValue("gender", out genderObject);
        gender = genderObject as string;

        // Determine the model to load for the character when instantiated.
        switch (gender)
        {
            case "male":
                genderModelName = "Characters/CharacterBodyMale";
                break;
            case "female":
                genderModelName = "Characters/CharacterBodyFemale";
                break;
            default:
                Debug.Log("No Gender found for character!");
                break;
        }
    } 
    #endregion

    #region Public Methods
    /// <summary>
    /// Places the body of the character in the scene, and returns it as a GameObject.
    /// </summary>
    /// <param name="parent">Transform of the partent this GameObject is placed on.</param>
    /// <returns></returns>
    public GameObject InstantiateBody(Transform parent)
    {
        body = GameObject.Instantiate(Resources.Load(genderModelName)) as GameObject;
        RectTransform bodyRectTransform = body.GetComponent<RectTransform>();
        bodyRectTransform.SetParent(parent);
        body.transform.localPosition = new Vector3(0, 0, parent.localPosition.z);
        bodyRectTransform.localScale = Vector3.one;
        //float scale = parent.GetComponent<RectTransform>().rect.height;
        //body.transform.FindChild("3DModel").localScale = Vector3.one * scale;
        return body;
    } 
    #endregion
}
