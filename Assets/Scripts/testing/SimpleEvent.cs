using UnityEngine;
using UnityEngine.Events;
public class SimpleEvent : MonoBehaviour {
	//My UnityEvent Manager
	public UnityEvent myUnityEvent = new UnityEvent();
	void Start () {
		//Subscribe my delegate to my UnityEvent Manager
		myUnityEvent.AddListener(MyAwesomeDelegate);
		//Execute all registered delegates
		myUnityEvent.Invoke();
	}
	//My Delegate function
	private void MyAwesomeDelegate()
	{
		Debug.Log("My Awesome UnityEvent lives");
	}

	public void RunMeFromTheInspector()
	{
		Debug.Log("Look, I was configured in the inspector");
	}
}