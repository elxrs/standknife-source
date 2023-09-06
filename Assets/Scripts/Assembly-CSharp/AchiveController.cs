using UnityEngine;

public class AchiveController : MonoBehaviour
{
	public Achive[] Achives;

	public GameObject AchiveFinishText;

	public GameObject AchiveCloseText;

	private int _countAchives;

	public static AchiveController _obj;

	private void Awake()
	{
		_obj = this;
		_countAchives = Achives.Length;
		if (PlayerPrefs.GetInt("GameMode") == 2)
		{
			for (int i = 0; i < Achives.Length; i++)
			{
				Achives[i].gameObject.SetActive(value: false);
			}
			AchiveFinishText.SetActive(value: false);
			AchiveCloseText.SetActive(value: true);
		}
	}

	public static void AchiveDestroyCheck()
	{
		_obj._countAchives--;
		if (_obj._countAchives <= 0)
		{
			_obj.AchiveFinishText.SetActive(value: true);
		}
	}
}
