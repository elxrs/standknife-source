using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviourPunCallbacks
{
	public static string _nameRoom;

	public static bool _inRoom;

	public static int _levelLoad;

	public static DisconnectCause _cause;

	public static bool _offlineMode;

	public static bool _onDisconnected;

	public static bool _serverActive = true;

	public static bool _onManager;

	protected void Awake()
	{
		if (!_onManager)
		{
			_onManager = true;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		if (SceneManager.GetActiveScene().buildIndex >= 3)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			_inRoom = false;
			_cause = cause;
			_onDisconnected = true;
			if (PhotonNetwork.InRoom)
			{
				PhotonNetwork.LeaveRoom();
			}
			PhotonNetwork.LoadLevel(0);
		}
	}
}
