using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConectServer : MonoBehaviourPunCallbacks
{
	public GameObject DisconectWindow;

	public Text DisconectText;

	public GameObject ModeWindow;

	public GameObject ServerWindow;

	public Text LoadText;

	public string[] _regions;

	public bool _serverActive;

	private void Start()
	{
		ServerManager._serverActive = _serverActive;
		if (ServerManager._onDisconnected)
		{
			DisconectWindow.SetActive(value: true);
			DisconectText.text = ServerManager._cause.ToString();
		}
		else if (!PhotonNetwork.IsConnected & !ServerManager._offlineMode)
		{
			Conect();
		}
		else
		{
			SceneManager.LoadScene(1);
		}
	}

	public void Conect()
	{
		StartCoroutine(TryConect());
	}

	private IEnumerator TryConect()
	{
		yield return new WaitForSeconds(1f);
		if (_serverActive)
		{
			PhotonNetwork.GameVersion = Application.version;
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.ConnectToRegion(_regions[PlayerPrefs.GetInt("RegionNumber")]);
		}
		else
		{
			ServerWindow.SetActive(value: true);
		}
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		if (cause == DisconnectCause.DnsExceptionOnConnect)
		{
			ModeWindow.SetActive(value: true);
		}
	}

	public void OfflineMode()
	{
		ServerManager._offlineMode = true;
		StartCoroutine(LoadMenu());
	}

	public override void OnConnectedToMaster()
	{
		ModeWindow.SetActive(value: false);
		ServerManager._offlineMode = false;
		StartCoroutine(LoadMenu());
	}

	private IEnumerator LoadMenu()
	{
		yield return new WaitForSeconds(0.5f);
		LoadText.text = "Загрузка данных пользователя";
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(1);
	}
}
