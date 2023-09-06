using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class Achive : MonoBehaviour
{
	public enum Type
	{
		KillDummy = 0,
		KillDummyHead = 1,
		KillOther = 2,
		KillEnemyTimer = 3,
		RecordKillEnemy = 4,
		FinishRounds = 5,
		LevelUp = 6,
		KillDummyWeapon = 7
	}

	public Type modeZad;

	public int[] _countMain;

	public int[] _countPrizGold;

	public int[] _idSuperPriz;

	public int _numberAchive;

	private int _numberFinish;

	private int _levelAchive;

	private bool _takePrizZad;

	[Header("Префаб:")]
	public Text ZadanieText;

	public Text CountText;

	public Text GoldCountText;

	public GameObject PrizObject;

	public Image SuperPriz;

	public GameObject TakePrizButton;

	public GameObject PrizText;

	private void Start()
	{
		_takePrizZad = false;
		LoadAchive();
	}

	private void OnEnable()
	{
		if (_takePrizZad)
		{
			_takePrizZad = false;
			GetComponent<Animation>().Play("AchiveReloadPriz");
			LoadAchive();
		}
	}

	public void LoadAchive()
	{
		if (PlayerPrefsSafe.GetInt("AchiveDelete_" + _numberAchive) == 1)
		{
			AchiveController.AchiveDestroyCheck();
			Object.Destroy(base.gameObject);
		}
		_levelAchive = PlayerPrefsSafe.GetInt("AchiveSaveLevel_" + _numberAchive);
		switch (modeZad)
		{
		case Type.KillDummy:
			_numberFinish = PlayerPrefsSafe.GetInt("CountDeadDummy");
			ZadanieText.text = "Убить " + _countMain[_levelAchive] + " Манекенов при помощи ножа";
			break;
		case Type.KillDummyHead:
			_numberFinish = PlayerPrefsSafe.GetInt("CountDeadDummyHead");
			ZadanieText.text = "Убить " + _countMain[_levelAchive] + " Манекенов в голову в режиме «Выживание»";
			break;
		case Type.KillOther:
			_numberFinish = PlayerPrefsSafe.GetInt("CountDeadOther");
			ZadanieText.text = "Убить " + _countMain[_levelAchive] + " босса(ов) санту в режиме «Выживание»";
			break;
		case Type.KillEnemyTimer:
			_numberFinish = PlayerPrefsSafe.GetInt("CountDeadEnemyTimer");
			ZadanieText.text = "За одну сессию убить " + _countMain[_levelAchive] + " врагов в режиме «Таймер» на 30 секунд при помощи ножа";
			break;
		case Type.RecordKillEnemy:
			_numberFinish = PlayerPrefsSafe.GetInt("RecordMod2_15");
			ZadanieText.text = "Получить рекорд " + _countMain[_levelAchive] + " убийств в режиме «Выживание» на 15 секунд";
			break;
		case Type.FinishRounds:
			_numberFinish = PlayerPrefsSafe.GetInt("CountFinishRounds");
			ZadanieText.text = "Завершить " + _countMain[_levelAchive] + " сессий в режиме «Таймер» на 60 секунд и больше";
			break;
		case Type.LevelUp:
			_numberFinish = PlayerPrefsSafe.GetInt("AccMedal_Quality") - 1;
			ZadanieText.text = "Дойти до 50 уровня и получить медаль";
			break;
		case Type.KillDummyWeapon:
			_numberFinish = PlayerPrefsSafe.GetInt("CountDeadDummyWeapon");
			ZadanieText.text = "Убить " + _countMain[_levelAchive] + " Манекенов при помощи огнестрельных оружий";
			break;
		}
		CountText.text = _numberFinish + "|" + _countMain[_levelAchive];
		if (_numberFinish >= _countMain[_levelAchive])
		{
			TakePrizButton.SetActive(value: true);
			PrizText.SetActive(value: false);
		}
		else
		{
			TakePrizButton.SetActive(value: false);
			PrizText.SetActive(value: true);
		}
		if (_countPrizGold[_levelAchive] != 0)
		{
			GoldCountText.text = _countPrizGold[_levelAchive].ToString();
			PrizObject.SetActive(value: true);
			SuperPriz.gameObject.SetActive(value: false);
		}
		else
		{
			SuperPriz.sprite = ControllerInventory._obj.AllItems[_idSuperPriz[_levelAchive]].Preview;
			SuperPriz.gameObject.SetActive(value: true);
			PrizObject.SetActive(value: false);
		}
	}

	public void TakePrizAchive()
	{
		StartCoroutine(TakePriz());
	}

	private IEnumerator TakePriz()
	{
		RuntimeManager.PlayOneShot("event:/UI/ui default");
		_takePrizZad = true;
		GetComponent<Animation>().Play("AchiveTakePriz");
		if (_countPrizGold[_levelAchive] != 0)
		{
			Controller.AddMoney(_countPrizGold[_levelAchive]);
		}
		else
		{
			ControllerInventory.BuyItem(_idSuperPriz[_levelAchive]);
		}
		_levelAchive++;
		if (_levelAchive >= _countMain.Length)
		{
			PlayerPrefsSafe.SetInt("AchiveDelete_" + _numberAchive, 1);
		}
		else
		{
			PlayerPrefsSafe.SetInt("AchiveSaveLevel_" + _numberAchive, _levelAchive);
		}
		PlayerPrefs.Save();
		yield return new WaitForSeconds(0.75f);
		if (_levelAchive >= _countMain.Length)
		{
			AchiveController.AchiveDestroyCheck();
			Object.Destroy(base.gameObject);
		}
		else
		{
			_takePrizZad = false;
			GetComponent<Animation>().Play("AchiveReloadPriz");
			LoadAchive();
		}
	}
}
