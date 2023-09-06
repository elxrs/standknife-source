using UnityEngine;
using UnityEngine.UI;

public class ScrollNormaliz : MonoBehaviour
{
	private ScrollRect _scrollRect;

	private void Awake()
	{
		_scrollRect = GetComponent<ScrollRect>();
	}

	private void OnEnable()
	{
		ResetRect();
	}

	public void ResetRect()
	{
		_scrollRect.normalizedPosition = new Vector2(_scrollRect.normalizedPosition.x, 1f);
	}
}
