using UnityEngine;
using System.Collections;

/// <summary>
/// Helper script to make gameobjects orient to another gameobject when active.
/// </summary>
public class LookAt : MonoBehaviour {

    public Transform lookTarget;

	// Update is called once per frame
	void Update () {
	    if (this.gameObject.activeInHierarchy)
        {
            this.transform.LookAt(lookTarget);
        }
	}
}