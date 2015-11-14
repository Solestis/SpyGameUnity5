using System.Collections;
using UnityEngine;
public class LoadingAnimation : MonoBehaviour {
	Animator loadingAnimation;
	// Get the Animator component on Awake
	void Awake () {
		loadingAnimation = GetComponent<Animator>();
	}
	// Start the loading animation by setting the animation
	// Loading property to true
	void Start () {
		loadingAnimation.SetBool("Loading", true);
		StartCoroutine(LoadLevel());
	}
	// A simple coroutine to wait 3 seconds and load the level
	IEnumerator LoadLevel()
	{
		for (int i = 0; i < 3; i++)
		{
			yield return new WaitForSeconds(1);
		}
		loadingAnimation.SetBool("Loading", false);
		//Application.LoadLevel("Main Menu");
	}
}