using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Realtime.Demo
{
	public class ConnectAndJoinRandomLb : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
	{
		[SerializeField]
		private AppSettings appSettings = new AppSettings();

		private LoadBalancingClient lbc;

		private ConnectionHandler ch;

		public Text StateUiText;

		public void Start()
		{
			lbc = new LoadBalancingClient();
			lbc.AddCallbackTarget(this);
			if (!lbc.ConnectUsingSettings(appSettings))
			{
				Debug.LogError("Error while connecting");
			}
			ch = base.gameObject.GetComponent<ConnectionHandler>();
			if (ch != null)
			{
				ch.Client = lbc;
				ch.StartFallbackSendAckThread();
			}
		}

		public void Update()
		{
			LoadBalancingClient loadBalancingClient = lbc;
			if (loadBalancingClient != null)
			{
				loadBalancingClient.Service();
				Text stateUiText = StateUiText;
				string text = loadBalancingClient.State.ToString();
				if (stateUiText != null && !stateUiText.text.Equals(text))
				{
					stateUiText.text = "State: " + text;
				}
			}
		}

		public void OnConnected()
		{
		}

		public void OnConnectedToMaster()
		{
			Debug.Log("OnConnectedToMaster");
			lbc.OpJoinRandomRoom();
		}

		public void OnDisconnected(DisconnectCause cause)
		{
			Debug.Log(string.Concat("OnDisconnected(", cause, ")"));
		}

		public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
		}

		public void OnCustomAuthenticationFailed(string debugMessage)
		{
		}

		public void OnRegionListReceived(RegionHandler regionHandler)
		{
			Debug.Log("OnRegionListReceived");
			regionHandler.PingMinimumOfRegions(OnRegionPingCompleted, null);
		}

		public void OnRoomListUpdate(List<RoomInfo> roomList)
		{
		}

		public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
		{
		}

		public void OnJoinedLobby()
		{
		}

		public void OnLeftLobby()
		{
		}

		public void OnFriendListUpdate(List<FriendInfo> friendList)
		{
		}

		public void OnCreatedRoom()
		{
		}

		public void OnCreateRoomFailed(short returnCode, string message)
		{
		}

		public void OnJoinedRoom()
		{
			Debug.Log("OnJoinedRoom");
		}

		public void OnJoinRoomFailed(short returnCode, string message)
		{
		}

		public void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.Log("OnJoinRandomFailed");
			lbc.OpCreateRoom(new EnterRoomParams());
		}

		public void OnLeftRoom()
		{
		}

		private void OnRegionPingCompleted(RegionHandler regionHandler)
		{
			Debug.Log("OnRegionPingCompleted " + regionHandler.BestRegion);
			Debug.Log("RegionPingSummary: " + regionHandler.SummaryToCache);
			lbc.ConnectToRegionMaster(regionHandler.BestRegion.Code);
		}
	}
}
