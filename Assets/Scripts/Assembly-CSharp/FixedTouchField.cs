using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public Vector2 TouchDist;

	private Vector2 PointerOld;

	private int PointerId;

	public bool Pressed;

	public void OnPointerDown(PointerEventData eventData)
	{
		Pressed = true;
		PointerId = eventData.pointerId;
		PointerOld = eventData.position;
	}

	private void Update()
	{
		if (Pressed)
		{
			if (PointerId >= 0 && PointerId < Input.touches.Length)
			{
				TouchDist = Input.touches[PointerId].position - PointerOld;
				PointerOld = Input.touches[PointerId].position;
			}
			else
			{
				TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
				PointerOld = Input.mousePosition;
			}
		}
		else
		{
			TouchDist = default(Vector2);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Pressed = false;
	}
}
