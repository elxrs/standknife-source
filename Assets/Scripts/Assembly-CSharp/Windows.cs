using FMODUnity;
using UnityEngine;

public class Windows : MonoBehaviour
{
	public GameObject[] WindowsOpen;

	public GameObject[] WindowsClose;

	public void Open()
	{
		RuntimeManager.PlayOneShot("event:/UI/ui default");
		for (int i = 0; i < WindowsOpen.Length; i++)
		{
			WindowsOpen[i].SetActive(value: true);
		}
	}

	public void OpenScale()
	{
		RuntimeManager.PlayOneShot("event:/UI/ui default");
		for (int i = 0; i < WindowsOpen.Length; i++)
		{
			WindowsOpen[i].transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void Close()
	{
		RuntimeManager.PlayOneShot("event:/UI/ui default");
		for (int i = 0; i < WindowsClose.Length; i++)
		{
			WindowsClose[i].SetActive(value: false);
		}
	}

	public void CloseScale()
	{
		RuntimeManager.PlayOneShot("event:/UI/ui default");
		for (int i = 0; i < WindowsClose.Length; i++)
		{
			WindowsClose[i].transform.localScale = new Vector3(0f, 0f, 0f);
		}
	}
}
