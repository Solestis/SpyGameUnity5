using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helper script, counts the children of a gameObject and shows it on a UI.
/// </summary>
public class ChildCounter : MonoBehaviour {

    #region Linked in Editor
    public GameObject parentObject;
    public Text counterText; 
    #endregion
	private int childCount;

    #region Monobehaviour Event Handlers
    /// <summary>
    /// Update the counterText.
    /// </summary>
    void Update()
    {
        if (childCount != parentObject.transform.childCount)
        {
            childCount = parentObject.transform.childCount;
            counterText.text = childCount.ToString();
        }
    } 
    #endregion
}
