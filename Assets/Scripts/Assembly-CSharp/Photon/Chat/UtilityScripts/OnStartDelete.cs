using UnityEngine;

namespace Photon.Chat.UtilityScripts
{
	public class OnStartDelete : MonoBehaviour
	{
		private void Start()
		{
			Object.Destroy(base.gameObject);
		}
	}
}
