using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Chat.Demo
{
	[ExecuteInEditMode]
	public class ChatAppIdCheckerUI : MonoBehaviour
	{
		public Text Description;

		public void Update()
		{
			if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
			{
				if (Description != null)
				{
					Description.text = "<Color=Red>WARNING:</Color>\nPlease setup a Chat AppId in the PhotonServerSettings file.";
				}
			}
			else if (Description != null)
			{
				Description.text = string.Empty;
			}
		}
	}
}
