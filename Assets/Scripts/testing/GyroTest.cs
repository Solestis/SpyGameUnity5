using UnityEngine;
using System.Collections;

public class GyroTest : MonoBehaviour {

	void Awake () {
		Input.gyro.enabled = true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		print (Input.gyro.userAcceleration);
	}
}
