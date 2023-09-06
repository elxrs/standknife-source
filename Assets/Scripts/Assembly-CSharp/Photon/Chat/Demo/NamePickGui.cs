using UnityEngine;
using UnityEngine.UI;

namespace Photon.Chat.Demo
{
	[RequireComponent(typeof(ChatGui))]
	public class NamePickGui : MonoBehaviour
	{
		private const string UserNamePlayerPref = "NamePickUserName";

		public ChatGui chatNewComponent;

		public InputField idInput;

		public void Start()
		{
			chatNewComponent = Object.FindObjectOfType<ChatGui>();
			string @string = PlayerPrefs.GetString("NamePickUserName");
			if (!string.IsNullOrEmpty(@string))
			{
				idInput.text = @string;
			}
		}

		public void EndEditOnEnter()
		{
			if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
			{
				StartChat();
			}
		}

		public void StartChat()
		{
			ChatGui chatGui = Object.FindObjectOfType<ChatGui>();
			chatGui.UserName = idInput.text.Trim();
			chatGui.Connect();
			base.enabled = false;
			PlayerPrefs.SetString("NamePickUserName", chatGui.UserName);
		}
	}
}
