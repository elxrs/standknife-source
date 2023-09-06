using UnityEngine;
using UnityEngine.EventSystems;

namespace Photon.Chat.UtilityScripts
{
	public class EventSystemSpawner : MonoBehaviour
	{
		private void OnEnable()
		{
			if (Object.FindObjectOfType<EventSystem>() == null)
			{
				GameObject obj = new GameObject("EventSystem");
				obj.AddComponent<EventSystem>();
				obj.AddComponent<StandaloneInputModule>();
			}
		}
	}
}
