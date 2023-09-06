using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerLose : MonoBehaviour
{
	public int _sceneLoad;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (_sceneLoad == 0)
			{
				SceneManager.LoadScene(Controller.sceneIndex);
			}
			else
			{
				SceneManager.LoadScene(2);
			}
			ServerManager._levelLoad = _sceneLoad;
		}
	}
}
