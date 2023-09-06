using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviourPunCallbacks
{
	public Image Progress;

	public GameObject[] LoadMenus;

	private void Start()
	{
		if (!ServerManager._inRoom)
		{
			StartCoroutine(LoadLevel());
		}
		else
		{
			StartCoroutine(LoadServer());
		}
	}

	private IEnumerator LoadLevel()
	{
		LoadMenus[ServerManager._levelLoad - 3].SetActive(value: true);
		yield return new WaitForSeconds(1f);
		AsyncOperation operation = SceneManager.LoadSceneAsync(ServerManager._levelLoad);
		while (!operation.isDone)
		{
			float fillAmount = operation.progress / 0.9f;
			Progress.fillAmount = fillAmount;
			yield return null;
		}
	}

	private IEnumerator LoadServer()
	{
		LoadMenus[ServerManager._levelLoad - 3].SetActive(value: true);
		yield return new WaitForSeconds(1f);
		PhotonNetwork.LoadLevel(ServerManager._levelLoad);
		while (PhotonNetwork.LevelLoadingProgress < 1f)
		{
			Progress.fillAmount = PhotonNetwork.LevelLoadingProgress / 0.9f;
			yield return null;
		}
	}
}
