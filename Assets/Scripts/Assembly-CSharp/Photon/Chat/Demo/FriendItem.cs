using UnityEngine;
using UnityEngine.UI;

namespace Photon.Chat.Demo
{
	public class FriendItem : MonoBehaviour
	{
		public Text NameLabel;

		public Text StatusLabel;

		public Text Health;

		[HideInInspector]
		public string FriendId
		{
			get
			{
				return NameLabel.text;
			}
			set
			{
				NameLabel.text = value;
			}
		}

		public void Awake()
		{
			Health.text = string.Empty;
		}

		public void OnFriendStatusUpdate(int status, bool gotMessage, object message)
		{
			string text;
			switch (status)
			{
			case 1:
				text = "Invisible";
				break;
			case 2:
				text = "Online";
				break;
			case 3:
				text = "Away";
				break;
			case 4:
				text = "Do not disturb";
				break;
			case 5:
				text = "Looking For Game/Group";
				break;
			case 6:
				text = "Playing";
				break;
			default:
				text = "Offline";
				break;
			}
			StatusLabel.text = text;
			if (gotMessage)
			{
				string text2 = string.Empty;
				if (message != null && message is string[] array && array.Length >= 2)
				{
					text2 = array[1] + "%";
				}
				Health.text = text2;
			}
		}
	}
}
