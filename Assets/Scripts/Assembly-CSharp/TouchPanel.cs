using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public Vector2 TouchDist;

	public bool pressed;

	private int fingerId;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject == base.gameObject)
		{
			pressed = true;
			fingerId = eventData.pointerId;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		pressed = false;
		TouchDist.x = 0f;
		TouchDist.y = 0f;
	}

	private void Update()
	{
		if (!pressed)
		{
			return;
		}
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			if (touch.fingerId == fingerId)
			{
				if (touch.phase == TouchPhase.Moved)
				{
					TouchDist.x = touch.deltaPosition.x;
					TouchDist.y = touch.deltaPosition.y;
				}
				if (touch.phase == TouchPhase.Stationary)
				{
					TouchDist.x = 0f;
					TouchDist.y = 0f;
				}
			}
		}
	}
}
