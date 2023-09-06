using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class ControllerPolygon : MonoBehaviour
{
	[Range(15f, 60f)]
	public int MinTime_Mod1;

	[Range(60f, 120f)]
	public int MaxTime_Mod1;

	[Range(5f, 10f)]
	public int MinTime_Mod2;

	[Range(15f, 30f)]
	public int MaxTime_Mod2;

	public int ExpKill_Head;

	public int ExpKill_Body;

	public int MoneyKill_Head;

	public int MoneyKill_Body;

	public Animation GamePerecl;

	public Animation ModePriz;

	public Animation ModeGame;

	public Animation DurationTimer;

	public Animation DurationTimer2;

	public GameObject InfoView;

	public GameObject StartView;

	public GameObject TimeInfo;

	public GameObject RecordInfo;

	public GameObject PrizInfo;

	public GameObject SuperPrizInfo;

	public GameObject OtherInfo;

	public Text KillCountText;

	public Text TimeCountText;

	public Text StartText;

	public Text RecordCountText;

	public Text RecordText;

	public Text PrizCountText;

	public Text PrizText;

	public Text TimerControllText;

	public GameObject[] SuperPrizes;

	public int[] _idSuperPrizes;

	public GameObject[] SpawnersEnemy;

	public GameObject EnemyPrefab;

	public GameObject Enemy2Prefab;

	public GameObject diedRadarPrefab;

	public GameObject diedRadar2Prefab;

	public bool _spawnEnemy2;

	private Int TimeAll_Mod1;

	private Int TimeAll_Mod2;

	private GameObject TimerUI;

	private Text TimerUIText;

	private GameObject BossHPUI;

	private Image BossHealth;

	private int PrizCount;

	private bool _prizSuperTake;

	private bool playing;

	private Int CountDead;

	private Int CountDeadDummy;

	private Int CountDeadDummyHead;

	private Int CountDeadOther;

	private Int CountDeadWeapon;

	private Int Time_Ost;

	private Int Time_BossRespawn;

	private Int HpEnemy;

	private Int IDEnemy;

	private Int _modePriz;

	private Int _modeGame;

	private bool _zadSpawnEnemy2;

	public static GameObject _enemy;

	public static ControllerPolygon _obj;

	private void Start()
	{
		_obj = this;
		TimerUI = GameObject.Find("TimerUI");
		TimerUIText = GameObject.Find("TimerUIText").GetComponent<Text>();
		BossHPUI = GameObject.Find("BossHPUI");
		BossHealth = GameObject.Find("BossHealth").GetComponent<Image>();
		_modePriz = 0;
		TimeAll_Mod1 = 60;
		TimeAll_Mod2 = 15;
		_prizSuperTake = false;
	}

	public static void PereclGame()
	{
		_obj.playing = !_obj.playing;
		if (_obj.playing)
		{
			_obj.StartCoroutine(_obj.StartTheGame());
		}
		else
		{
			StopPlay(0);
		}
	}

	public static void ModePrizSet(int _typeMode)
	{
		if (_obj.playing)
		{
			StopPlay(0);
		}
		switch (_typeMode)
		{
		case 0:
			_obj._modePriz = 0;
			_obj.ModePriz.Play("ButtonClick_Left");
			break;
		case 1:
			_obj._modePriz = 1;
			_obj.ModePriz.Play("ButtonClick_Right");
			break;
		}
	}

	public static void ModeGameSet(int _typeMode)
	{
		if (_obj.playing)
		{
			StopPlay(0);
		}
		switch (_typeMode)
		{
		case 0:
		{
			_obj._modeGame = 0;
			_obj.DurationTimer.gameObject.SetActive(value: true);
			_obj.DurationTimer2.gameObject.SetActive(value: false);
			_obj.ModeGame.Play("ButtonClick_Left");
			int num = (int)((float)(int)_obj.TimeAll_Mod1 / 60f);
			_obj.TimerControllText.text = num.ToString("D2") + ":" + ((int)_obj.TimeAll_Mod1 - 60 * num).ToString("D2");
			_obj.TimerControllText.transform.parent.gameObject.SetActive(value: true);
			break;
		}
		case 1:
		{
			_obj._modeGame = 1;
			_obj.DurationTimer.gameObject.SetActive(value: false);
			_obj.DurationTimer2.gameObject.SetActive(value: true);
			_obj.ModeGame.Play("ButtonClick_Right");
			int num = (int)((float)(int)_obj.TimeAll_Mod2 / 60f);
			_obj.TimerControllText.text = num.ToString("D2") + ":" + ((int)_obj.TimeAll_Mod2 - 60 * num).ToString("D2");
			_obj.TimerControllText.transform.parent.gameObject.SetActive(value: true);
			break;
		}
		case 2:
			_obj._modeGame = 2;
			_obj.DurationTimer.gameObject.SetActive(value: false);
			_obj.DurationTimer2.gameObject.SetActive(value: false);
			_obj.ModeGame.Play("ButtonClick_Right2");
			_obj.TimerControllText.transform.parent.gameObject.SetActive(value: false);
			break;
		}
	}

	public static void DurationTimerAdd(int _typeMode, int _typeClick, int _countAdd)
	{
		if (_obj.playing)
		{
			StopPlay(0);
		}
		switch (_typeMode)
		{
		case 0:
			if (((int)_obj.TimeAll_Mod1 + _countAdd >= _obj.MinTime_Mod1) & ((int)_obj.TimeAll_Mod1 + _countAdd <= _obj.MaxTime_Mod1))
			{
				ControllerPolygon obj2 = _obj;
				obj2.TimeAll_Mod1 = (int)obj2.TimeAll_Mod1 + _countAdd;
				int num2 = (int)((float)(int)_obj.TimeAll_Mod1 / 60f);
				_obj.TimerControllText.text = num2.ToString("D2") + ":" + ((int)_obj.TimeAll_Mod1 - 60 * num2).ToString("D2");
			}
			switch (_typeClick)
			{
			case 0:
				_obj.DurationTimer.Play("ButtonClick2_Left");
				break;
			case 1:
				_obj.DurationTimer.Play("ButtonClick2_Right");
				break;
			}
			break;
		case 1:
			if (((int)_obj.TimeAll_Mod2 + _countAdd >= _obj.MinTime_Mod2) & ((int)_obj.TimeAll_Mod2 + _countAdd <= _obj.MaxTime_Mod2))
			{
				ControllerPolygon obj = _obj;
				obj.TimeAll_Mod2 = (int)obj.TimeAll_Mod2 + _countAdd;
				int num = (int)((float)(int)_obj.TimeAll_Mod2 / 60f);
				_obj.TimerControllText.text = num.ToString("D2") + ":" + ((int)_obj.TimeAll_Mod2 - 60 * num).ToString("D2");
			}
			switch (_typeClick)
			{
			case 0:
				_obj.DurationTimer2.Play("ButtonClick2_Left");
				break;
			case 1:
				_obj.DurationTimer2.Play("ButtonClick2_Right");
				break;
			}
			break;
		}
	}

	public static void StopPlay(int _typeStop)
	{
		RuntimeManager.PlayOneShot("event:/Ambience/Polygon/Horn 2");
		if (_enemy != null)
		{
			Object.Destroy(_enemy);
		}
		if ((int)_obj.IDEnemy == 2)
		{
			RuntimeManager.PlayOneShot("event:/Ingame Events/New Year 2022/santa_kill_cam");
			_obj.BossHPUI.transform.localScale = new Vector3(0f, 0f, 0f);
			_obj.BossHealth.fillAmount = 1f;
		}
		_obj.GamePerecl.Play("ButtonClick_Stop");
		_obj.playing = false;
		_obj.TimeCountText.text = "00:00";
		_obj.TimerUIText.text = _obj.TimeCountText.text;
		_obj.TimerUI.transform.localScale = new Vector3(0f, 0f, 0f);
		if ((_typeStop == 0) & ((int)_obj.CountDead == 0 || (int)_obj._modeGame == 0))
		{
			_obj.InfoView.SetActive(value: false);
			_obj.StartView.SetActive(value: true);
			_obj.StartText.text = "Прервано";
		}
		else
		{
			_obj.TimeInfo.SetActive(value: false);
			_obj.OtherInfo.SetActive(value: true);
			if (PlayerPrefs.GetInt("GameMode") == 1)
			{
				if ((Random.Range(0, 100) < 5 + PlayerPrefsSafe.GetInt("BoostLuck")) & !_obj._prizSuperTake & ((int)_obj._modeGame == 0))
				{
					_obj.PrizInfo.SetActive(value: false);
					_obj.SuperPrizInfo.SetActive(value: true);
					int num = Random.Range(0, _obj.SuperPrizes.Length);
					_obj.SuperPrizes[num].SetActive(value: true);
					PlayerPrefsSafe.SetInt("SuperPrizeID", _obj._idSuperPrizes[num]);
					_obj._prizSuperTake = true;
				}
				else
				{
					_obj.PrizInfo.SetActive(value: true);
				}
				if ((int)_obj._modeGame == 0)
				{
					if ((int)_obj.TimeAll_Mod1 == 30)
					{
						PlayerPrefsSafe.SetInt("CountDeadEnemyTimer", (int)_obj.CountDead - (int)_obj.CountDeadWeapon);
					}
					else if ((int)_obj.TimeAll_Mod1 >= 60)
					{
						PlayerPrefsSafe.SetInt("CountFinishRounds", 1 + PlayerPrefsSafe.GetInt("CountFinishRounds"));
					}
				}
				else if ((int)_obj._modeGame == 1)
				{
					switch (_obj.TimeAll_Mod2)
					{
					case 15:
					{
						float num2 = (float)_obj.PrizCount * 0.9f;
						_obj.PrizCount = (int)num2;
						break;
					}
					case 20:
					{
						float num2 = (float)_obj.PrizCount * 0.8f;
						_obj.PrizCount = (int)num2;
						break;
					}
					}
					PlayerPrefsSafe.SetInt("CountDeadDummyHead", (int)_obj.CountDeadDummyHead + PlayerPrefsSafe.GetInt("CountDeadDummyHead"));
					if (PlayerPrefsSafe.GetInt("RecordMod2_" + _obj.TimeAll_Mod2.ToString()) < (int)_obj.CountDead)
					{
						PlayerPrefsSafe.SetInt("RecordMod2_" + _obj.TimeAll_Mod2.ToString(), _obj.CountDead);
					}
				}
				else if ((int)_obj._modeGame == 2)
				{
					float num3 = (float)_obj.PrizCount * 0.5f;
					_obj.PrizCount = (int)num3;
					if (PlayerPrefsSafe.GetInt("RecordMod3") < (int)_obj.CountDead)
					{
						PlayerPrefsSafe.SetInt("RecordMod3", _obj.CountDead);
					}
				}
				PlayerPrefsSafe.SetInt("CountDeadDummy", (int)_obj.CountDeadDummy - (int)_obj.CountDeadWeapon + PlayerPrefsSafe.GetInt("CountDeadDummy"));
				PlayerPrefsSafe.SetInt("CountDeadDummyWeapon", (int)_obj.CountDeadWeapon + PlayerPrefsSafe.GetInt("CountDeadDummyWeapon"));
				PlayerPrefsSafe.SetInt("CountDeadOther", (int)_obj.CountDeadOther + PlayerPrefsSafe.GetInt("CountDeadOther"));
				switch (_obj._modePriz)
				{
				case 0:
					PlayerPrefsSafe.SetInt("ExpAddGame", _obj.PrizCount + PlayerPrefsSafe.GetInt("ExpAddGame"));
					break;
				case 1:
					PlayerPrefsSafe.SetInt("MoneyAddGame", _obj.PrizCount + PlayerPrefsSafe.GetInt("MoneyAddGame"));
					break;
				}
				_obj.PrizCountText.text = _obj.PrizCount.ToString();
				PlayerPrefs.Save();
			}
			else
			{
				_obj.PrizCount = 0;
				_obj.PrizInfo.SetActive(value: true);
				_obj.PrizCountText.text = _obj.PrizCount.ToString();
			}
		}
		_obj.StopAllCoroutines();
	}

	private IEnumerator StartTheGame()
	{
		_obj.GamePerecl.Play("ButtonClick_Start");
		HpEnemy = 0;
		PrizCount = 0;
		CountDead = 0;
		CountDeadDummy = 0;
		CountDeadDummyHead = 0;
		CountDeadOther = 0;
		KillCountText.text = "0";
		InfoView.SetActive(value: false);
		StartView.SetActive(value: true);
		TimeInfo.SetActive(value: true);
		RecordInfo.SetActive(value: false);
		PrizInfo.SetActive(value: false);
		SuperPrizInfo.SetActive(value: false);
		OtherInfo.SetActive(value: false);
		RuntimeManager.PlayOneShot("event:/Ambience/Polygon/Horn");
		StartText.text = "3";
		yield return new WaitForSeconds(1f);
		RuntimeManager.PlayOneShot("event:/Ambience/Polygon/Horn");
		StartText.text = "2";
		yield return new WaitForSeconds(1f);
		RuntimeManager.PlayOneShot("event:/Ambience/Polygon/Horn");
		StartText.text = "1";
		yield return new WaitForSeconds(1f);
		RuntimeManager.PlayOneShot("event:/Ambience/Polygon/Horn 2");
		switch (_obj._modePriz)
		{
		case 0:
			_obj.PrizText.text = "Exp+";
			break;
		case 1:
			_obj.PrizText.text = "Gold+";
			break;
		}
		switch (_obj._modeGame)
		{
		case 0:
			PrizInfo.SetActive(value: true);
			Time_Ost = TimeAll_Mod1;
			break;
		case 1:
			RecordText.text = string.Concat("Рекорд на ", TimeAll_Mod2, " сек.");
			RecordCountText.text = PlayerPrefsSafe.GetInt("RecordMod2_" + _obj.TimeAll_Mod2.ToString()).ToString();
			Time_Ost = TimeAll_Mod2;
			_obj.TimeInfo.SetActive(value: false);
			RecordInfo.SetActive(value: true);
			_obj.OtherInfo.SetActive(value: true);
			break;
		case 2:
			RecordText.text = "Рекорд";
			RecordCountText.text = PlayerPrefsSafe.GetInt("RecordMod3").ToString();
			_obj.TimeInfo.SetActive(value: false);
			RecordInfo.SetActive(value: true);
			_obj.OtherInfo.SetActive(value: true);
			break;
		}
		StartView.SetActive(value: false);
		InfoView.SetActive(value: true);
		if ((int)_obj._modeGame != 2)
		{
			TimerUI.transform.localScale = new Vector3(1f, 1f, 1f);
			StartCoroutine(TimerStart());
		}
		StartCoroutine(EnemySpawn(0f));
	}

	private IEnumerator TimerStart()
	{
		Time_BossRespawn = 0;
		while ((int)Time_Ost > 1)
		{
			if ((int)Time_Ost <= 4)
			{
				RuntimeManager.PlayOneShot("event:/Ambience/Polygon/Horn");
			}
			Time_Ost = (int)Time_Ost - 1;
			if (((int)IDEnemy == 2) & ((int)_modeGame == 1))
			{
				Time_BossRespawn = (int)Time_BossRespawn + 1;
				if ((int)Time_BossRespawn >= 10)
				{
					Object.Destroy(_enemy);
				}
			}
			int num = (int)((float)(int)Time_Ost / 60f);
			TimeCountText.text = num.ToString("D2") + ":" + ((int)Time_Ost - 60 * num).ToString("D2");
			TimerUIText.text = TimeCountText.text;
			yield return new WaitForSeconds(1f);
			if ((int)Time_BossRespawn >= 10)
			{
				int num2 = Random.Range(0, SpawnersEnemy.Length);
				_enemy = Object.Instantiate(Enemy2Prefab, SpawnersEnemy[num2].transform.position, SpawnersEnemy[num2].transform.rotation);
				Time_BossRespawn = 0;
			}
		}
		StopPlay(1);
	}

	public static void HitEnemy(int _damage, float _strongHit, int _isKnife)
	{
		float obj = _strongHit;
		if (!1f.Equals(obj))
		{
			if (!2f.Equals(obj))
			{
				if (3f.Equals(obj))
				{
					_strongHit = 2.5f;
				}
			}
			else
			{
				_strongHit = 1f;
			}
		}
		else
		{
			_strongHit = 0.75f;
		}
		_strongHit *= (float)Random.Range(92, 100) / 100f;
		if ((int)_obj.IDEnemy == 1)
		{
			ControllerPolygon obj2 = _obj;
			obj2.HpEnemy = (int)obj2.HpEnemy - (int)((float)_damage * _strongHit);
		}
		else if ((int)_obj.IDEnemy == 2 && _isKnife == 1)
		{
			ControllerPolygon obj3 = _obj;
			obj3.HpEnemy = (int)obj3.HpEnemy - (int)((float)_damage * _strongHit);
			_obj.BossHealth.fillAmount = (float)(int)_obj.HpEnemy / 1000f;
		}
		float num = 1f;
		if (_isKnife == 0)
		{
			num = 0.2f;
		}
		if ((int)_obj.HpEnemy <= 0)
		{
			if ((int)_obj.IDEnemy == 1)
			{
				if (_strongHit == 1f)
				{
					switch (_obj._modePriz)
					{
					case 0:
						_obj.PrizCount += (int)((float)_obj.ExpKill_Body * num);
						break;
					case 1:
						_obj.PrizCount += (int)((float)_obj.MoneyKill_Body * num);
						break;
					}
				}
				else if (_strongHit > 1f)
				{
					switch (_obj._modePriz)
					{
					case 0:
						_obj.PrizCount += (int)((float)_obj.ExpKill_Head * num);
						break;
					case 1:
						_obj.PrizCount += (int)((float)_obj.MoneyKill_Head * num);
						break;
					}
					ControllerPolygon obj4 = _obj;
					obj4.CountDeadDummyHead = (int)obj4.CountDeadDummyHead + 1;
				}
				ControllerPolygon obj5 = _obj;
				obj5.CountDeadDummy = (int)obj5.CountDeadDummy + 1;
				if ((int)_obj._modeGame == 1)
				{
					_obj.Time_Ost = (int)((float)(int)_obj.TimeAll_Mod2 * num) + 1;
					_obj.StopAllCoroutines();
					_obj.StartCoroutine(_obj.TimerStart());
				}
			}
			else
			{
				switch (_obj._modePriz)
				{
				case 0:
					_obj.PrizCount += 1000;
					break;
				case 1:
					_obj.PrizCount += 750;
					break;
				}
				ControllerPolygon obj6 = _obj;
				obj6.CountDeadOther = (int)obj6.CountDeadOther + 1;
				_obj.Time_Ost = (int)_obj.TimeAll_Mod2 + 3;
				_obj.StopAllCoroutines();
				_obj.StartCoroutine(_obj.TimerStart());
			}
			if (_isKnife == 0)
			{
				ControllerPolygon obj7 = _obj;
				obj7.CountDeadWeapon = (int)obj7.CountDeadWeapon + 1;
			}
			ControllerPolygon obj8 = _obj;
			obj8.CountDead = (int)obj8.CountDead + 1;
			DeadEnemy();
		}
		else
		{
			Controller._obj.Kitmarker.Play("Marker_Hit");
		}
	}

	public static void DeadEnemy()
	{
		Controller._obj.Kitmarker.Play("Marker_Kill");
		GameObject gameObject = GameObject.Find("ballonEnemy");
		if ((int)_obj.IDEnemy == 2)
		{
			RuntimeManager.PlayOneShot("event:/Ingame Events/New Year 2022/santa_kill_cam");
			Object.Instantiate(_obj.diedRadar2Prefab, gameObject.transform.position, gameObject.transform.rotation);
			_obj.BossHPUI.transform.localScale = new Vector3(0f, 0f, 0f);
			_obj.BossHealth.fillAmount = 1f;
			_obj.StartCoroutine(_obj.EnemySpawn(3f));
		}
		else
		{
			Object.Instantiate(_obj.diedRadarPrefab, gameObject.transform.position, gameObject.transform.rotation);
			_obj.StartCoroutine(_obj.EnemySpawn(1f));
		}
		Object.Destroy(_enemy);
		_obj.KillCountText.text = _obj.CountDead.ToString();
	}

	private IEnumerator EnemySpawn(float _zad)
	{
		yield return new WaitForSeconds(_zad);
		int num = Random.Range(0, SpawnersEnemy.Length);
		if ((Random.Range(0, 100) < 3 + PlayerPrefsSafe.GetInt("BoostLuck")) & _spawnEnemy2 & ((int)_obj._modeGame == 1) & !_zadSpawnEnemy2)
		{
			_enemy = Object.Instantiate(Enemy2Prefab, SpawnersEnemy[num].transform.position, SpawnersEnemy[num].transform.rotation);
			BossHPUI.transform.localScale = new Vector3(1f, 1f, 1f);
			BossHealth.fillAmount = 1f;
			HpEnemy = 1000;
			IDEnemy = 2;
			_zadSpawnEnemy2 = true;
			_obj.Time_Ost = (int)_obj.TimeAll_Mod2 * 2;
			_obj.StopAllCoroutines();
			_obj.StartCoroutine(_obj.TimerStart());
		}
		else
		{
			_enemy = Object.Instantiate(EnemyPrefab, SpawnersEnemy[num].transform.position, SpawnersEnemy[num].transform.rotation);
			if (PlayerPrefsSafe.GetInt("WhActive") == 1)
			{
				_enemy.GetComponent<Dummy>().DummyWH.SetActive(value: true);
			}
			HpEnemy = 100;
			IDEnemy = 1;
			_zadSpawnEnemy2 = false;
		}
	}
}
