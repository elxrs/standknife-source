using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Chat.Demo
{
	public class ChatGui : MonoBehaviour, IChatClientListener
	{
		public string[] ChannelsToJoinOnConnect;

		public string[] FriendsList;

		public int HistoryLengthToFetch;

		private string selectedChannelName;

		public ChatClient chatClient;

		protected internal ChatAppSettings chatAppSettings;

		public GameObject missingAppIdErrorPanel;

		public GameObject ConnectingLabel;

		public RectTransform ChatPanel;

		public GameObject UserIdFormPanel;

		public InputField InputFieldChat;

		public Text CurrentChannelText;

		public Toggle ChannelToggleToInstantiate;

		public GameObject FriendListUiItemtoInstantiate;

		private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();

		private readonly Dictionary<string, FriendItem> friendListItemLUT = new Dictionary<string, FriendItem>();

		public bool ShowState = true;

		public GameObject Title;

		public Text StateText;

		public Text UserIdText;

		private static string HelpText = "\n    -- HELP --\nTo subscribe to channel(s) (channelnames are case sensitive) :  \n\t<color=#E07B00>\\subscribe</color> <color=green><list of channelnames></color>\n\tor\n\t<color=#E07B00>\\s</color> <color=green><list of channelnames></color>\n\nTo leave channel(s):\n\t<color=#E07B00>\\unsubscribe</color> <color=green><list of channelnames></color>\n\tor\n\t<color=#E07B00>\\u</color> <color=green><list of channelnames></color>\n\nTo switch the active channel\n\t<color=#E07B00>\\join</color> <color=green><channelname></color>\n\tor\n\t<color=#E07B00>\\j</color> <color=green><channelname></color>\n\nTo send a private message: (username are case sensitive)\n\t\\<color=#E07B00>msg</color> <color=green><username></color> <color=green><message></color>\n\nTo change status:\n\t\\<color=#E07B00>state</color> <color=green><stateIndex></color> <color=green><message></color>\n<color=green>0</color> = Offline <color=green>1</color> = Invisible <color=green>2</color> = Online <color=green>3</color> = Away \n<color=green>4</color> = Do not disturb <color=green>5</color> = Looking For Group <color=green>6</color> = Playing\n\nTo clear the current chat tab (private chats get closed):\n\t<color=#E07B00>\\clear</color>";

		public int TestLength = 2048;

		private byte[] testBytes = new byte[2048];

		public string UserName { get; set; }

		public void Start()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			UserIdText.text = "";
			StateText.text = "";
			StateText.gameObject.SetActive(value: true);
			UserIdText.gameObject.SetActive(value: true);
			Title.SetActive(value: true);
			ChatPanel.gameObject.SetActive(value: false);
			ConnectingLabel.SetActive(value: false);
			if (string.IsNullOrEmpty(UserName))
			{
				UserName = "user" + Environment.TickCount % 99;
			}
			chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
			bool flag = !string.IsNullOrEmpty(chatAppSettings.AppIdChat);
			missingAppIdErrorPanel.SetActive(!flag);
			UserIdFormPanel.gameObject.SetActive(flag);
			if (!flag)
			{
				Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
			}
		}

		public void Connect()
		{
			UserIdFormPanel.gameObject.SetActive(value: false);
			chatClient = new ChatClient(this);
			chatClient.UseBackgroundWorkerForSending = true;
			chatClient.AuthValues = new AuthenticationValues(UserName);
			chatClient.ConnectUsingSettings(chatAppSettings);
			ChannelToggleToInstantiate.gameObject.SetActive(value: false);
			Debug.Log("Connecting as: " + UserName);
			ConnectingLabel.SetActive(value: true);
		}

		public void OnDestroy()
		{
			if (chatClient != null)
			{
				chatClient.Disconnect();
			}
		}

		public void OnApplicationQuit()
		{
			if (chatClient != null)
			{
				chatClient.Disconnect();
			}
		}

		public void Update()
		{
			if (chatClient != null)
			{
				chatClient.Service();
			}
			if (StateText == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				StateText.gameObject.SetActive(ShowState);
			}
		}

		public void OnEnterSend()
		{
			if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
			{
				SendChatMessage(InputFieldChat.text);
				InputFieldChat.text = "";
			}
		}

		public void OnClickSend()
		{
			if (InputFieldChat != null)
			{
				SendChatMessage(InputFieldChat.text);
				InputFieldChat.text = "";
			}
		}

		private void SendChatMessage(string inputLine)
		{
			if (string.IsNullOrEmpty(inputLine))
			{
				return;
			}
			if ("test".Equals(inputLine))
			{
				if (TestLength != testBytes.Length)
				{
					testBytes = new byte[TestLength];
				}
				chatClient.SendPrivateMessage(chatClient.AuthValues.UserId, testBytes, forwardAsWebhook: true);
			}
			bool flag = chatClient.PrivateChannels.ContainsKey(selectedChannelName);
			string target = string.Empty;
			if (flag)
			{
				target = selectedChannelName.Split(':')[1];
			}
			if (inputLine[0].Equals('\\'))
			{
				string[] array = inputLine.Split(new char[1] { ' ' }, 2);
				if (array[0].Equals("\\help"))
				{
					PostHelpToCurrentChannel();
				}
				if (array[0].Equals("\\state"))
				{
					int num = 0;
					List<string> list = new List<string>();
					list.Add("i am state " + num);
					string[] array2 = array[1].Split(' ', ',');
					if (array2.Length != 0)
					{
						num = int.Parse(array2[0]);
					}
					if (array2.Length > 1)
					{
						list.Add(array2[1]);
					}
					chatClient.SetOnlineStatus(num, list.ToArray());
				}
				else if ((array[0].Equals("\\subscribe") || array[0].Equals("\\s")) && !string.IsNullOrEmpty(array[1]))
				{
					chatClient.Subscribe(array[1].Split(' ', ','));
				}
				else if ((array[0].Equals("\\unsubscribe") || array[0].Equals("\\u")) && !string.IsNullOrEmpty(array[1]))
				{
					chatClient.Unsubscribe(array[1].Split(' ', ','));
				}
				else if (array[0].Equals("\\clear"))
				{
					ChatChannel channel;
					if (flag)
					{
						chatClient.PrivateChannels.Remove(selectedChannelName);
					}
					else if (chatClient.TryGetChannel(selectedChannelName, flag, out channel))
					{
						channel.ClearMessages();
					}
				}
				else if (array[0].Equals("\\msg") && !string.IsNullOrEmpty(array[1]))
				{
					string[] array3 = array[1].Split(new char[2] { ' ', ',' }, 2);
					if (array3.Length >= 2)
					{
						string target2 = array3[0];
						string message = array3[1];
						chatClient.SendPrivateMessage(target2, message);
					}
				}
				else if ((array[0].Equals("\\join") || array[0].Equals("\\j")) && !string.IsNullOrEmpty(array[1]))
				{
					string[] array4 = array[1].Split(new char[2] { ' ', ',' }, 2);
					if (channelToggles.ContainsKey(array4[0]))
					{
						ShowChannel(array4[0]);
						return;
					}
					chatClient.Subscribe(new string[1] { array4[0] });
				}
				else
				{
					Debug.Log("The command '" + array[0] + "' is invalid.");
				}
			}
			else if (flag)
			{
				chatClient.SendPrivateMessage(target, inputLine);
			}
			else
			{
				chatClient.PublishMessage(selectedChannelName, inputLine);
			}
		}

		public void PostHelpToCurrentChannel()
		{
			CurrentChannelText.text += HelpText;
		}

		public void DebugReturn(DebugLevel level, string message)
		{
			switch (level)
			{
			case DebugLevel.ERROR:
				Debug.LogError(message);
				break;
			case DebugLevel.WARNING:
				Debug.LogWarning(message);
				break;
			default:
				Debug.Log(message);
				break;
			}
		}

		public void OnConnected()
		{
			if (ChannelsToJoinOnConnect != null && ChannelsToJoinOnConnect.Length != 0)
			{
				chatClient.Subscribe(ChannelsToJoinOnConnect, HistoryLengthToFetch);
			}
			ConnectingLabel.SetActive(value: false);
			UserIdText.text = "Connected as " + UserName;
			ChatPanel.gameObject.SetActive(value: true);
			if (FriendsList != null && FriendsList.Length != 0)
			{
				chatClient.AddFriends(FriendsList);
				string[] friendsList = FriendsList;
				foreach (string text in friendsList)
				{
					if (FriendListUiItemtoInstantiate != null && text != UserName)
					{
						InstantiateFriendButton(text);
					}
				}
			}
			if (FriendListUiItemtoInstantiate != null)
			{
				FriendListUiItemtoInstantiate.SetActive(value: false);
			}
			chatClient.SetOnlineStatus(2);
		}

		public void OnDisconnected()
		{
			ConnectingLabel.SetActive(value: false);
		}

		public void OnChatStateChange(ChatState state)
		{
			StateText.text = state.ToString();
		}

		public void OnSubscribed(string[] channels, bool[] results)
		{
			foreach (string channelName in channels)
			{
				chatClient.PublishMessage(channelName, "says 'hi'.");
				if (ChannelToggleToInstantiate != null)
				{
					InstantiateChannelButton(channelName);
				}
			}
			Debug.Log("OnSubscribed: " + string.Join(", ", channels));
			ShowChannel(channels[0]);
		}

		public void OnSubscribed(string channel, string[] users, Dictionary<object, object> properties)
		{
			Debug.LogFormat("OnSubscribed: {0}, users.Count: {1} Channel-props: {2}.", channel, users.Length, properties.ToStringFull());
		}

		private void InstantiateChannelButton(string channelName)
		{
			if (channelToggles.ContainsKey(channelName))
			{
				Debug.Log("Skipping creation for an existing channel toggle.");
				return;
			}
			Toggle toggle = UnityEngine.Object.Instantiate(ChannelToggleToInstantiate);
			toggle.gameObject.SetActive(value: true);
			toggle.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
			toggle.transform.SetParent(ChannelToggleToInstantiate.transform.parent, worldPositionStays: false);
			channelToggles.Add(channelName, toggle);
		}

		private void InstantiateFriendButton(string friendId)
		{
			GameObject obj = UnityEngine.Object.Instantiate(FriendListUiItemtoInstantiate);
			obj.gameObject.SetActive(value: true);
			FriendItem component = obj.GetComponent<FriendItem>();
			component.FriendId = friendId;
			obj.transform.SetParent(FriendListUiItemtoInstantiate.transform.parent, worldPositionStays: false);
			friendListItemLUT[friendId] = component;
		}

		public void OnUnsubscribed(string[] channels)
		{
			foreach (string text in channels)
			{
				if (channelToggles.ContainsKey(text))
				{
					UnityEngine.Object.Destroy(channelToggles[text].gameObject);
					channelToggles.Remove(text);
					Debug.Log("Unsubscribed from channel '" + text + "'.");
					if (text == selectedChannelName && channelToggles.Count > 0)
					{
						IEnumerator<KeyValuePair<string, Toggle>> enumerator = channelToggles.GetEnumerator();
						enumerator.MoveNext();
						ShowChannel(enumerator.Current.Key);
						enumerator.Current.Value.isOn = true;
					}
				}
				else
				{
					Debug.Log("Can't unsubscribe from channel '" + text + "' because you are currently not subscribed to it.");
				}
			}
		}

		public void OnGetMessages(string channelName, string[] senders, object[] messages)
		{
			if (channelName.Equals(selectedChannelName))
			{
				ShowChannel(selectedChannelName);
			}
		}

		public void OnPrivateMessage(string sender, object message, string channelName)
		{
			InstantiateChannelButton(channelName);
			if (message is byte[] array)
			{
				Debug.Log("Message with byte[].Length: " + array.Length);
			}
			if (selectedChannelName.Equals(channelName))
			{
				ShowChannel(channelName);
			}
		}

		public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
		{
			Debug.LogWarning("status: " + $"{user} is {status}. Msg:{message}");
			if (friendListItemLUT.ContainsKey(user))
			{
				FriendItem friendItem = friendListItemLUT[user];
				if (friendItem != null)
				{
					friendItem.OnFriendStatusUpdate(status, gotMessage, message);
				}
			}
		}

		public void OnUserSubscribed(string channel, string user)
		{
			Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
		}

		public void OnUserUnsubscribed(string channel, string user)
		{
			Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
		}

		public void OnChannelPropertiesChanged(string channel, string userId, Dictionary<object, object> properties)
		{
			Debug.LogFormat("OnChannelPropertiesChanged: {0} by {1}. Props: {2}.", channel, userId, properties.ToStringFull());
		}

		public void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties)
		{
			Debug.LogFormat("OnUserPropertiesChanged: (channel:{0} user:{1}) by {2}. Props: {3}.", channel, targetUserId, senderUserId, properties.ToStringFull());
		}

		public void OnErrorInfo(string channel, string error, object data)
		{
			Debug.LogFormat("OnErrorInfo for channel {0}. Error: {1} Data: {2}", channel, error, data);
		}

		public void AddMessageToSelectedChannel(string msg)
		{
			ChatChannel channel = null;
			if (!chatClient.TryGetChannel(selectedChannelName, out channel))
			{
				Debug.Log("AddMessageToSelectedChannel failed to find channel: " + selectedChannelName);
			}
			else
			{
				channel?.Add("Bot", msg, 0);
			}
		}

		public void ShowChannel(string channelName)
		{
			if (string.IsNullOrEmpty(channelName))
			{
				return;
			}
			ChatChannel channel = null;
			if (!chatClient.TryGetChannel(channelName, out channel))
			{
				Debug.Log("ShowChannel failed to find channel: " + channelName);
				return;
			}
			selectedChannelName = channelName;
			CurrentChannelText.text = channel.ToStringMessages();
			Debug.Log("ShowChannel: " + selectedChannelName);
			foreach (KeyValuePair<string, Toggle> channelToggle in channelToggles)
			{
				channelToggle.Value.isOn = ((channelToggle.Key == channelName) ? true : false);
			}
		}

		public void OpenDashboard()
		{
			Application.OpenURL("https://dashboard.photonengine.com");
		}
	}
}
