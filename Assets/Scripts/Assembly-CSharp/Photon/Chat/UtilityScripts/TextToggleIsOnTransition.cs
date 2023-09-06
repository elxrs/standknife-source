using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Photon.Chat.UtilityScripts
{
	[RequireComponent(typeof(Text))]
	public class TextToggleIsOnTransition : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		public Toggle toggle;

		private Text _text;

		public Color NormalOnColor = Color.white;

		public Color NormalOffColor = Color.black;

		public Color HoverOnColor = Color.black;

		public Color HoverOffColor = Color.black;

		private bool isHover;

		public void OnEnable()
		{
			_text = GetComponent<Text>();
			OnValueChanged(toggle.isOn);
			toggle.onValueChanged.AddListener(OnValueChanged);
		}

		public void OnDisable()
		{
			toggle.onValueChanged.RemoveListener(OnValueChanged);
		}

		public void OnValueChanged(bool isOn)
		{
			_text.color = ((!isOn) ? (isHover ? NormalOffColor : NormalOffColor) : (isHover ? HoverOnColor : HoverOnColor));
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			isHover = true;
			_text.color = (toggle.isOn ? HoverOnColor : HoverOffColor);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			isHover = false;
			_text.color = (toggle.isOn ? NormalOnColor : NormalOffColor);
		}
	}
}
