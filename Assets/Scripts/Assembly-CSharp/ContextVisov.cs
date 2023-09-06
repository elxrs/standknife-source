using UnityEngine;
using UnityEngine.EventSystems;

public class ContextVisov : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public Vector2 PositionDown;

	public void OnPointerDown(PointerEventData eventData)
	{
		PositionDown = eventData.position;
	}
}
