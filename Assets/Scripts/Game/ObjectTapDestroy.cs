using UnityEngine;
using System.Collections;

/// <summary>
/// Helper script that destroys a gameObject when touched.
/// </summary>
public class ObjectTapDestroy : MonoBehaviour {

    public UIScanAssignment scanAssignment;
    public GameObject destroyObject;

    /// <summary>
    /// On Monobehaviour OnMouseDown, destroy this gameObject and calls TargetScanned to scanAssignment.
    /// </summary>
    void OnMouseDown()
    {
        print("touched the sphere!");
        if (scanAssignment != null)
        {
            scanAssignment.TargetScanned(destroyObject);
        }
        Destroy(destroyObject);
    }
}
