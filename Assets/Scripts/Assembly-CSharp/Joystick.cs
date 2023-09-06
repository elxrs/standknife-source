using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
{
	[SerializeField]
	private float handleRange = 1f;

	[SerializeField]
	private float deadZone;

	[SerializeField]
	private AxisOptions axisOptions;

	[SerializeField]
	private bool snapX;

	[SerializeField]
	private bool snapY;

	[SerializeField]
	protected RectTransform background;

	[SerializeField]
	private RectTransform handle;

	private RectTransform baseRect;

	private Canvas canvas;

	private Camera cam;

	private Vector2 input = Vector2.zero;

	public float Horizontal
	{
		get
		{
			if (!snapX)
			{
				return input.x;
			}
			return SnapFloat(input.x, AxisOptions.Horizontal);
		}
	}

	public float Vertical
	{
		get
		{
			if (!snapY)
			{
				return input.y;
			}
			return SnapFloat(input.y, AxisOptions.Vertical);
		}
	}

	public Vector2 Direction => new Vector2(Horizontal, Vertical);

	public float HandleRange
	{
		get
		{
			return handleRange;
		}
		set
		{
			handleRange = Mathf.Abs(value);
		}
	}

	public float DeadZone
	{
		get
		{
			return deadZone;
		}
		set
		{
			deadZone = Mathf.Abs(value);
		}
	}

	public AxisOptions AxisOptions
	{
		get
		{
			return AxisOptions;
		}
		set
		{
			axisOptions = value;
		}
	}

	public bool SnapX
	{
		get
		{
			return snapX;
		}
		set
		{
			snapX = value;
		}
	}

	public bool SnapY
	{
		get
		{
			return snapY;
		}
		set
		{
			snapY = value;
		}
	}

	protected virtual void Start()
	{
		HandleRange = handleRange;
		DeadZone = deadZone;
		baseRect = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>();
		if (canvas == null)
		{
			Debug.LogError("The Joystick is not placed inside a canvas");
		}
		Vector2 vector = new Vector2(0.5f, 0.5f);
		background.pivot = vector;
		handle.anchorMin = vector;
		handle.anchorMax = vector;
		handle.pivot = vector;
		handle.anchoredPosition = Vector2.zero;
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		OnDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		cam = null;
		if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
		{
			cam = canvas.worldCamera;
		}
		Vector2 vector = RectTransformUtility.WorldToScreenPoint(cam, background.position);
		Vector2 vector2 = background.sizeDelta / 2f;
		input = (eventData.position - vector) / (vector2 * canvas.scaleFactor);
		FormatInput();
		HandleInput(input.magnitude, input.normalized, vector2, cam);
		handle.anchoredPosition = input * vector2 * handleRange;
	}

	protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
	{
		if (magnitude > deadZone)
		{
			if (magnitude > 1f)
			{
				input = normalised;
			}
		}
		else
		{
			input = Vector2.zero;
		}
	}

	private void FormatInput()
	{
		if (axisOptions == AxisOptions.Horizontal)
		{
			input = new Vector2(input.x, 0f);
		}
		else if (axisOptions == AxisOptions.Vertical)
		{
			input = new Vector2(0f, input.y);
		}
	}

	private float SnapFloat(float value, AxisOptions snapAxis)
	{
		if (value == 0f)
		{
			return value;
		}
		if (axisOptions == AxisOptions.Both)
		{
			float num = Vector2.Angle(input, Vector2.up);
			switch (snapAxis)
			{
			case AxisOptions.Horizontal:
				if (num < 22.5f || num > 157.5f)
				{
					return 0f;
				}
				return (value > 0f) ? 1 : (-1);
			case AxisOptions.Vertical:
				if (num > 67.5f && num < 112.5f)
				{
					return 0f;
				}
				return (value > 0f) ? 1 : (-1);
			default:
				return value;
			}
		}
		if (value > 0f)
		{
			return 1f;
		}
		if (value < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		input = Vector2.zero;
		handle.anchoredPosition = Vector2.zero;
	}

	protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
	{
		Vector2 localPoint = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
		{
			Vector2 vector = baseRect.pivot * baseRect.sizeDelta;
			return localPoint - background.anchorMax * baseRect.sizeDelta + vector;
		}
		return Vector2.zero;
	}
}
