#pragma strict
import UnityEngine.UI;

public var cameraLeft : Camera;
public var cameraRight : Camera;
public var barrier : Image;
public var maxWidth : float = 0.70;

function Start () {
	//Initializing camera widths (to avoid rendering issues this is done in script).
	cameraLeft.rect.width = 1-maxWidth;
	cameraRight.rect.width = maxWidth;
	cameraRight.rect.x = 1-maxWidth;
	barrier.rectTransform.position.x = (1-maxWidth) * Screen.width;
}

function Update () {
	//Resize camera views on screen touch.
	if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
		//If left side of screen is touched, increase size of topdown camera.
		if(Input.GetTouch(0).position.x < Screen.width/2) {
			print("touched left!");
			cameraLeft.rect.width = maxWidth;
			cameraRight.rect.width = 1-maxWidth;
			cameraRight.rect.x = maxWidth;
			barrier.rectTransform.position.x = maxWidth * Screen.width;
		
		//If right side of screen is touched, increase size of playerview camera.
		} else if(Input.GetTouch(0).position.x > Screen.width/2) {
			print("touched right!");
			cameraLeft.rect.width = 1-maxWidth;
			cameraRight.rect.width = maxWidth;
			cameraRight.rect.x = 1-maxWidth;
			barrier.rectTransform.position.x = (1-maxWidth) * Screen.width;
		}
	}
}