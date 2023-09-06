using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ControllerServer : MonoBehaviourPunCallbacks
{
	public GameObject TryConectButton;

	public InputField RoomName;

	public Text WarningPlayText;

	public Text ErrorText;

	public GameObject[] Blocks;

	public Image[] ButtonsTypeGame;

	public Color EnableButtonColor;

	public Color DisableButtonColor;

	private void Start()
	{
		SwitchTypeGame(0);
		if (PlayerPrefs.GetInt("GameMode") == 2)
		{
			ButtonsTypeGame[1].GetComponent<Button>().interactable = false;
			WarningPlayText.text = "В открытом режиме нету онлайна!";
		}
		else if (!ServerManager._serverActive)
		{
			ButtonsTypeGame[1].GetComponent<Button>().interactable = false;
			WarningPlayText.text = "Сервера отключены!";
		}
		else if (!PhotonNetwork.IsConnected)
		{
			TryConectButton.SetActive(value: true);
			ButtonsTypeGame[1].GetComponent<Button>().interactable = false;
			WarningPlayText.text = "Оффлайн режим!";
		}
	}

	public void SwitchTypeGame(int _type)
	{
		for (int i = 0; i < ButtonsTypeGame.Length; i++)
		{
			ButtonsTypeGame[i].color = EnableButtonColor;
		}
		ButtonsTypeGame[_type].color = DisableButtonColor;
		for (int j = 0; j < Blocks.Length; j++)
		{
			Blocks[j].SetActive(value: true);
		}
		Blocks[_type].SetActive(value: false);
	}

	public void CreateRoom()
	{
		if (RoomName.text.Length > 3)
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.MaxPlayers = 1;
			PhotonNetwork.CreateRoom(RoomName.text, roomOptions);
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(LobbyError("Короткое имя!"));
		}
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		StopAllCoroutines();
		StartCoroutine(LobbyError("Имя занято!"));
	}

	public void JoinRoom()
	{
		if (RoomName.text.Length > 3)
		{
			PhotonNetwork.JoinRoom(RoomName.text);
			return;
		}
		StopAllCoroutines();
		StartCoroutine(LobbyError("Короткое имя!"));
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		StopAllCoroutines();
		StartCoroutine(LobbyError("Игра не найдена!"));
	}

	public override void OnJoinedRoom()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LeaveRoom();
		}
		ServerManager._inRoom = true;
		ServerManager._nameRoom = RoomName.text;
		StopAllCoroutines();
		PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("AccName");
		Controller._obj.StartCoroutine(Controller._obj.PlayLoade(4));
	}

	public void JoinRandomRoom()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public void JoinRandomOrCreateRoom()
	{
		PhotonNetwork.JoinRandomOrCreateRoom(null, 0);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		StopAllCoroutines();
		StartCoroutine(LobbyError("Нет игр!"));
	}

	public void TryConect()
	{
		TryConectButton.SetActive(value: false);
		WarningPlayText.text = "Подключение...";
		PhotonNetwork.GameVersion = Application.version;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		if (PlayerPrefs.GetInt("GameMode") != 2)
		{
			SwitchTypeGame(0);
			TryConectButton.SetActive(value: true);
			ButtonsTypeGame[1].GetComponent<Button>().interactable = false;
			WarningPlayText.text = "Ошибка подключения к серверу!";
		}
	}

	public override void OnConnectedToMaster()
	{
		if (PlayerPrefs.GetInt("GameMode") != 2)
		{
			TryConectButton.SetActive(value: false);
			ButtonsTypeGame[1].GetComponent<Button>().interactable = true;
			PhotonNetwork.OfflineMode = false;
			WarningPlayText.text = "";
		}
	}

	private IEnumerator LobbyError(string _errorStr)
	{
		RoomName.placeholder.gameObject.SetActive(value: false);
		RoomName.textComponent.gameObject.SetActive(value: false);
		RoomName.interactable = false;
		ErrorText.text = _errorStr;
		ErrorText.gameObject.SetActive(value: true);
		yield return new WaitForSeconds(1f);
		RoomName.placeholder.gameObject.SetActive(value: true);
		RoomName.textComponent.gameObject.SetActive(value: true);
		RoomName.interactable = true;
		ErrorText.text = "";
		ErrorText.gameObject.SetActive(value: false);
	}
}
