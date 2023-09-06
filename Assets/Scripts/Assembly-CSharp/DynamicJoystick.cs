using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
	[SerializeField]
	private float moveThreshold = 1f;

	public float MoveThreshold
	{
		get
		{
			return moveThreshold;
		}
		set
		{
			moveThreshold = Mathf.Abs(value);
		}
	}

	protected override void Start()
	{
		MoveThreshold = moveThreshold;
		base.Start();
		background.gameObject.SetActive(value: false);
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
		background.gameObject.SetActive(value: true);
		base.OnPointerDown(eventData);
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		background.gameObject.SetActive(value: false);
		base.OnPointerUp(eventData);
	}

	protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
	{
		if (magnitude > moveThreshold)
		{
			Vector2 vector = normalised * (magnitude - moveThreshold) * radius;
			background.anchoredPosition += vector;
		}
		base.HandleInput(magnitude, normalised, radius, cam);
	}
}
