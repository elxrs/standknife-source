using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
	protected override void Start()
	{
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
}
