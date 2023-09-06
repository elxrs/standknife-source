using UnityEngine;

namespace Photon.Chat.Demo
{
	public class IgnoreUiRaycastWhenInactive : MonoBehaviour, ICanvasRaycastFilter
	{
		public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			return base.gameObject.activeInHierarchy;
		}
	}
}
