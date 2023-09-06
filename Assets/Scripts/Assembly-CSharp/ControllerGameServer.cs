using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerGameServer : MonoBehaviourPunCallbacks
{
	public Controller Canvas;

	public GameObject PlayerPrefab;

	public GameObject[] Spawners;

	public bool _spawnPlayer = true;

	public static PlayerController _player;

	public static ControllerGameServer _obj;

	public static int sceneIndex;

	private void Awake()
	{
		if (ServerManager._inRoom & !PhotonNetwork.InRoom)
		{
			PhotonNetwork.JoinRoom(ServerManager._nameRoom);
			return;
		}
		_obj = this;
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
		SpawnPlayer();
	}

	public override void OnJoinedRoom()
	{
		_obj = this;
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
		SpawnPlayer();
	}

	public void SpawnPlayer()
	{
		if (_spawnPlayer)
		{
			int num = Random.Range(0, Spawners.Length);
			if (ServerManager._inRoom)
			{
				_player = PhotonNetwork.Instantiate(PlayerPrefab.name, Spawners[num].transform.position, Spawners[num].transform.localRotation, 0).GetComponentInChildren<PlayerController>();
			}
			else
			{
				_player = Object.Instantiate(PlayerPrefab, Spawners[num].transform.position, Spawners[num].transform.localRotation).GetComponentInChildren<PlayerController>();
			}
		}
		else
		{
			_player = GameObject.Find("Player").GetComponent<PlayerController>();
		}
	}
}
