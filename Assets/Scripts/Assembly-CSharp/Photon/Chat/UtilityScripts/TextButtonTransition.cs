using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Photon.Chat.UtilityScripts
{
	[RequireComponent(typeof(Text))]
	public class TextButtonTransition : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		private Text _text;

		public Selectable Selectable;

		public Color NormalColor = Color.white;

		public Color HoverColor = Color.black;

		public void Awake()
		{
			_text = GetComponent<Text>();
		}

		public void OnEnable()
		{
			_text.color = NormalColor;
		}

		public void OnDisable()
		{
			_text.color = NormalColor;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (Selectable == null || Selectable.IsInteractable())
			{
				_text.color = HoverColor;
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (Selectable == null || Selectable.IsInteractable())
			{
				_text.color = NormalColor;
			}
		}
	}
}
