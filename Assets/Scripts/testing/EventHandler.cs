using UnityEngine;
using UnityEngine.EventSystems;
public class EventHandler : MonoBehaviour, IPointerClickHandler {

	public void OnPointerClick (PointerEventData eventData)
	{
		print (eventData);
		print (eventData.pointerCurrentRaycast.gameObject);
	}

}