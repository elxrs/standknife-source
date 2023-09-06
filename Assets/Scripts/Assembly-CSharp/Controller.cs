using System;
using System.Collections;
using FMODUnity;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviourPunCallbacks
{
	[Serializable]
	private class ControllMenu
	{
		public string Name;

		public GameObject[] NewMenu;

		public ButtonMenu[] NewButtons;

		public bool isInventMenu;
	}

	[Serializable]
	private class ButtonMenu
	{
		public Image Button;

		public bool _defaultButton;
	}

	[Serializable]
	private class Shop
	{
		public string Name;

		public GameObject[] ShopItems;

		public Image ShopButton;
	}

	[Serializable]
	private class Settings
	{
		public string Name;

		public GameObject[] SettingItems;

		public Image SettingsButton;
	}

	public GameObject[] Effects;

	public GameObject PauseMenu;

	public GameObject UIAll;

	public GameObject UIMobile;

	public GameObject MiniMapObject;

	public GameObject BigMapObject;

	public GameObject Cursore;

	public GameObject Pricele;

	public GameObject ZonaAttack;

	public GameObject ZonaShoot;

	public GameObject ButtonAttack;

	public GameObject ButtonShoot;

	public GameObject ButtonAim;

	public Image NoneUI;

	public Image NoneUI3;

	public Image WeaponUI;

	public Image WeaponUI3;

	public Sprite[] WeaponImage;

	public TouchPanel touchField;

	public Joystick joystick;

	public Animation Kitmarker;

	public Text ExpText;

	[SerializeField]
	private ControllMenu[] _menus;

	[SerializeField]
	private Shop[] AllShop;

	[SerializeField]
	private Settings[] AllSettings;

	public GameObject[] AllMenuDef;

	public GameObject[] AllMenuHor;

	public Image[] AllButtonsDef;

	public Image[] AllButtonsHor;

	public Color ButtonEnableDef;

	public Color ButtonDisableDef;

	public Color ButtonEnableHor;

	public Color ButtonDisableHor;

	public Color ButtonEnableShop;

	public Color ButtonDisableShop;

	public Sprite[] LevelIcons;

	public Sprite[] SpritesImage;

	public Sprite[] MedalIcons;

	public Text LevelText;

	public GameObject LevelProgress;

	public Text LevelProgressText;

	public Image LevelProgressCount;

	public Image LevelIcon;

	public GameObject ButtonMedal;

	public Image AccImage;

	public Image AccImageBC;

	public Image AccMedal;

	public InputField AccIDText;

	public InputField AccNameText;

	public string AccIDStringDef;

	public string AccNameStringDef;

	public string AccIDString;

	public string AccNameString;

	public float _acciddelete;

	public Button ButtonCC;

	public GameObject AvaMe;

	public GameObject PlayDevText;

	public Text PlayMapText;

	public string[] PlayMapName;

	public GameObject ButtonPlay;

	public GameObject ButtonPlayCancel;

	public Text ButtonPlayText;

	public GameObject LevelUI;

	public GameObject ButtonShop;

	public RectTransform MoneyObject;

	public Text MoneyText;

	public GameObject BlockInven;

	public GameObject ContextObject;

	public GameObject ContextMenu;

	public GameObject ContextGroup;

	public ScrollNormaliz ScrollShop;

	public GameObject ButtonExit;

	public static ShopController SaveItem;

	public static Text _moneyText;

	public static RectTransform _transform;

	public static Controller _obj;

	public static int sceneIndex;

	public static int _levelCount;

	public static int _levelProgress;

	private bool isPause;

	public static bool _attackWhile;

	public static bool _shootWhile;

	[HideInInspector]
	public int _poslMenu;

	private int _poslShop;

	private int _poslSetting;

	private void Awake()
	{
		Time.timeScale = 1f;
		isPause = false;
		_attackWhile = false;
		_shootWhile = false;
		ReloadEffects();
		_obj = GetComponent<Controller>();
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
		if (sceneIndex != 1)
		{
			return;
		}
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		_moneyText = MoneyText;
		_transform = MoneyObject;
		AccImage.sprite = SpritesImage[PlayerPrefs.GetInt("AccImageNumber")];
		AccImageBC.sprite = AccImage.sprite;
		if (PlayerPrefs.GetInt("GameMode") == 1)
		{
			MedalSet();
		}
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			ButtonExit.SetActive(value: true);
		}
		if (PlayerPrefsSafe.GetInt("AccIDName") != 0)
		{
			AccIDText.text = PlayerPrefs.GetString("AccID");
			AccNameText.text = PlayerPrefs.GetString("AccName");
		}
		else
		{
			int num = UnityEngine.Random.Range(1001, 9999);
			AccIDText.text = AccIDStringDef;
			AccNameText.text = "Guest_" + num;
			PlayerPrefsSafe.SetInt("AccIDName", num *= (int)((float)int.Parse(AccIDStringDef) * _acciddelete));
			PlayerPrefs.SetString("AccID", AccIDText.text);
			PlayerPrefs.SetString("AccName", AccNameText.text);
		}
		if (PlayerPrefs.GetInt("GameMode") == 0)
		{
			GameObject.Find("GameModeWindow").GetComponent<Windows>().OpenScale();
		}
		else if (PlayerPrefs.GetInt("GameMode") == 1)
		{
			if (PlayerPrefs.GetString("AccID") == PlayerPrefsSafe.GetInt("AccIDName").ToString())
			{
				ButtonCC.interactable = true;
			}
			else if (((PlayerPrefs.GetString("AccID") == AccIDString) & (PlayerPrefs.GetString("AccName") == AccNameString)) || Application.platform == RuntimePlatform.WindowsEditor)
			{
				ButtonCC.interactable = true;
				AvaMe.SetActive(value: true);
			}
			else
			{
				ButtonCC.interactable = false;
				AvaMe.SetActive(value: false);
			}
		}
		else if (PlayerPrefs.GetInt("GameMode") == 2)
		{
			MoneyObject.gameObject.SetActive(value: false);
			ButtonShop.transform.localScale = new Vector3(0f, 0f, 0f);
			LevelUI.SetActive(value: false);
		}
		SetMoney(PlayerPrefsSafe.GetInt("Money") + PlayerPrefsSafe.GetInt("MoneyAddGame"));
		PlayerPrefsSafe.SetInt("MoneyAddGame", 0);
		SetLevel(PlayerPrefsSafe.GetInt("LevelCount"), PlayerPrefsSafe.GetInt("LevelProgress"));
		AddLevel(PlayerPrefsSafe.GetInt("ExpAddGame"));
		PlayerPrefsSafe.SetInt("ExpAddGame", 0);
		PlayerPrefs.Save();
	}

	public void GameMode(int _mode)
	{
		PlayerPrefsSafe.SetInt("TypeWeapon3", 0);
		PlayerPrefsSafe.SetInt("TypeSkin3", 0);
		PlayerPrefsSafe.SetInt("TypeWeapon", 16);
		PlayerPrefsSafe.SetInt("TypeSkin", 0);
		PlayerPrefsSafe.SetInt("TypeGloves", 0);
		PlayerPrefsSafe.SetInt("TypeHands", 0);
		for (int i = 0; i < ControllerInventory._obj.AllItems.Length; i++)
		{
			PlayerPrefsSafe.SetInt("ItemIsActiveInt_" + i, 0);
		}
		PlayerPrefs.SetInt("GameMode", _mode);
		SceneManager.LoadScene(sceneIndex);
	}

	public void ReloadEffects()
	{
		if ((PlayerPrefs.GetInt("Effects") == 1) & (Effects.Length != 0))
		{
			for (int i = 0; i < Effects.Length; i++)
			{
				Effects[i].SetActive(value: true);
			}
		}
		else if ((PlayerPrefs.GetInt("Effects") == 0) & (Effects.Length != 0))
		{
			for (int j = 0; j < Effects.Length; j++)
			{
				Effects[j].SetActive(value: false);
			}
		}
	}

	public void ButtonAttack_Start()
	{
		_attackWhile = true;
		ControllerGameServer._player.StartCoroutine(ControllerGameServer._player.AttackWhile());
	}

	public void ButtonAttack_Stop()
	{
		_attackWhile = false;
	}

	public void ButtonShoot_Start()
	{
		_shootWhile = true;
		ControllerGameServer._player.StartCoroutine(ControllerGameServer._player.ShootWhile());
	}

	public void ButtonShoot_Stop()
	{
		_shootWhile = false;
	}

	public void ButtonJump_Start()
	{
		PlayerController.m_JumpWhile = true;
	}

	public void ButtonJump_Finish()
	{
		PlayerController.m_JumpWhile = false;
	}

	public void ButtonMap_Start()
	{
		MiniMapObject.SetActive(value: false);
		BigMapObject.SetActive(value: true);
	}

	public void ButtonMap_Finish()
	{
		MiniMapObject.SetActive(value: true);
		BigMapObject.SetActive(value: false);
	}

	public void ButtonInspect()
	{
		ControllerGameServer._player.StartCoroutine(ControllerGameServer._player.Inspect());
	}

	public void ButtonTake(int _typeTake)
	{
		ControllerGameServer._player.Take(_typeTake);
	}

	public void ButtonPricel()
	{
		ControllerGameServer._player.Pricel();
	}

	public void ButtonRestart()
	{
		SceneManager.LoadScene(sceneIndex);
	}

	public void ButtonPause()
	{
		if (!isPause)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			ControllerGameServer._player.m_controllPlayer = false;
			PauseMenu.SetActive(value: true);
			UIAll.SetActive(value: false);
			if (!PhotonNetwork.InRoom)
			{
				Time.timeScale = 0f;
			}
		}
		else
		{
			if (!PhotonNetwork.InRoom)
			{
				Time.timeScale = 1f;
			}
			UIAll.SetActive(value: true);
			PauseMenu.SetActive(value: false);
			if (!ControllerGameServer._player.m_animTest)
			{
				ControllerGameServer._player.m_controllPlayer = true;
			}
			if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) & !ControllerGameServer._player.m_mobileTest)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
		isPause = !isPause;
	}

	public void LoadLevel(int _levelIndex)
	{
		SceneManager.LoadScene(_levelIndex);
	}

	public void PlayLoad(int _levelIndex)
	{
		StartCoroutine(PlayLoade(_levelIndex));
	}

	public void PlayCancel()
	{
		StopAllCoroutines();
		BankPlay("event:/UI/ui default");
		ButtonPlayText.text = "00:00";
		ButtonPlayCancel.SetActive(value: false);
		ButtonPlay.SetActive(value: true);
		PlayDevText.SetActive(value: true);
		PlayMapText.text = "";
		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();
		}
	}

	public IEnumerator PlayLoade(int _levelIndex)
	{
		int Time = 0;
		SwitchMenu(0);
		BankPlay("event:/UI/ui play");
		PlayDevText.SetActive(value: false);
		PlayMapText.text = PlayMapName[_levelIndex - 3];
		ButtonPlay.SetActive(value: false);
		ButtonPlayCancel.SetActive(value: true);
		while (true)
		{
			int num = (int)((float)Time / 60f);
			ButtonPlayText.text = num.ToString("D2") + ":" + (Time - num * 60).ToString("D2");
			yield return new WaitForSeconds(1f);
			Time++;
			if (Time == 3)
			{
				ServerManager._levelLoad = _levelIndex;
				if (ServerManager._inRoom)
				{
					PhotonNetwork.LoadLevel(2);
				}
				else
				{
					SceneManager.LoadScene(2);
				}
			}
		}
	}

	public void AccImageSet(int _numberImage)
	{
		PlayerPrefs.SetInt("AccImageNumber", _numberImage);
		PlayerPrefs.Save();
		AccImage.sprite = SpritesImage[_numberImage];
		AccImageBC.sprite = AccImage.sprite;
	}

	public void InputFieldSet(int _typeInputField)
	{
		switch (_typeInputField)
		{
		case 1:
			if (AccIDText.text == "")
			{
				AccIDText.text = AccIDStringDef;
			}
			PlayerPrefs.SetString("AccID", AccIDText.text);
			break;
		case 2:
			if (AccNameText.text == "")
			{
				AccNameText.text = AccNameStringDef;
			}
			PlayerPrefs.SetString("AccName", AccNameText.text);
			break;
		}
		if (PlayerPrefs.GetInt("GameMode") == 1)
		{
			if (PlayerPrefs.GetString("AccID") == PlayerPrefsSafe.GetInt("AccIDName").ToString())
			{
				ButtonCC.interactable = true;
			}
			else if (((PlayerPrefs.GetString("AccID") == AccIDString) & (PlayerPrefs.GetString("AccName") == AccNameString)) || Application.platform == RuntimePlatform.WindowsEditor)
			{
				ButtonCC.interactable = true;
				AvaMe.SetActive(value: true);
			}
			else
			{
				ButtonCC.interactable = false;
				AvaMe.SetActive(value: false);
			}
		}
		PlayerPrefs.Save();
	}

	public void SwitchMenu(int _typeMenu)
	{
		if (_poslMenu == _typeMenu)
		{
			return;
		}
		if (_typeMenu < _menus.Length)
		{
			_poslMenu = _typeMenu;
			for (int i = 0; i < AllMenuDef.Length; i++)
			{
				AllMenuDef[i].SetActive(value: false);
				AllButtonsDef[i].color = ButtonEnableDef;
			}
			for (int j = 0; j < AllMenuHor.Length; j++)
			{
				AllMenuHor[j].SetActive(value: false);
				AllButtonsHor[j].color = ButtonEnableHor;
			}
			for (int k = 0; k < _menus[_typeMenu].NewMenu.Length; k++)
			{
				_menus[_typeMenu].NewMenu[k].SetActive(value: true);
				if (_menus[_typeMenu].NewButtons[k]._defaultButton)
				{
					_menus[_typeMenu].NewButtons[k].Button.color = ButtonDisableDef;
				}
				else
				{
					_menus[_typeMenu].NewButtons[k].Button.color = ButtonDisableHor;
				}
			}
			if (_menus[_typeMenu].isInventMenu & (ControllerInventory.CountBuildParent > 0))
			{
				ControllerInventory.BuildParents();
			}
		}
		else
		{
			if (_poslSetting == _typeMenu - _menus.Length)
			{
				return;
			}
			_typeMenu -= _menus.Length;
			_poslSetting = _typeMenu;
			for (int l = 0; l < AllSettings.Length; l++)
			{
				for (int m = 0; m < AllSettings[l].SettingItems.Length; m++)
				{
					AllSettings[l].SettingItems[m].SetActive(value: false);
				}
				AllSettings[l].SettingsButton.color = ButtonEnableHor;
			}
			for (int n = 0; n < AllSettings[_typeMenu].SettingItems.Length; n++)
			{
				AllSettings[_typeMenu].SettingItems[n].SetActive(value: true);
			}
			AllSettings[_typeMenu].SettingsButton.color = ButtonDisableHor;
		}
	}

	public void SwitchShop(int _typeShop)
	{
		if (_poslShop != _typeShop)
		{
			_poslShop = _typeShop;
			_typeShop--;
			for (int i = 0; i < AllShop.Length; i++)
			{
				for (int j = 0; j < AllShop[i].ShopItems.Length; j++)
				{
					AllShop[i].ShopItems[j].SetActive(value: false);
				}
				AllShop[i].ShopButton.color = ButtonEnableShop;
			}
			for (int k = 0; k < AllShop[_typeShop].ShopItems.Length; k++)
			{
				AllShop[_typeShop].ShopItems[k].SetActive(value: true);
			}
			AllShop[_typeShop].ShopButton.color = ButtonDisableShop;
		}
		else
		{
			for (int l = 0; l < AllShop.Length; l++)
			{
				for (int m = 0; m < AllShop[l].ShopItems.Length; m++)
				{
					AllShop[l].ShopItems[m].SetActive(value: true);
				}
				AllShop[l].ShopButton.color = ButtonEnableShop;
			}
			_poslShop = 0;
		}
		ScrollShop.ResetRect();
	}

	public void AddMoneyButton(int _money)
	{
		AddMoney(_money);
	}

	public void GetMedalLevel()
	{
		SetLevel(0, 0);
		_obj.AccMedal.sprite = _obj.MedalIcons[0];
		_obj.AccMedal.transform.localScale = new Vector3(0f, 0f, 0f);
		PlayerPrefsSafe.SetInt("AccMedalNumber", 0);
		PlayerPrefs.Save();
	}

	public void ButtonAddLevel(int _progressAdd)
	{
		AddLevel(_progressAdd);
	}

	public static void SetLevel(int _levelSet, int _progressSet)
	{
		if (_levelSet == 0)
		{
			_levelCount = 1;
		}
		else
		{
			_levelCount = _levelSet;
		}
		_levelProgress = _progressSet;
		PlayerPrefsSafe.SetInt("LevelCount", _levelCount);
		PlayerPrefsSafe.SetInt("LevelProgress", _levelProgress);
		PlayerPrefs.Save();
		if ((_levelCount == _obj.LevelIcons.Length) & (PlayerPrefsSafe.GetInt("AccMedal_Quality") < _obj.MedalIcons.Length - 1))
		{
			_obj.LevelProgress.SetActive(value: false);
			_obj.ButtonMedal.SetActive(value: true);
		}
		else
		{
			_obj.LevelProgress.SetActive(value: true);
			_obj.ButtonMedal.SetActive(value: false);
		}
		_obj.LevelText.text = _levelCount.ToString();
		_obj.LevelProgressText.text = _levelProgress + " | 1000";
		_obj.LevelProgressCount.fillAmount = (float)_levelProgress / 1000f;
		_obj.LevelIcon.sprite = _obj.LevelIcons[_levelCount - 1];
	}

	public static void AddLevel(int _progressAdd)
	{
		_levelProgress += _progressAdd;
		if (_levelProgress >= 1000)
		{
			float num = (float)_levelProgress / 1000f;
			_levelProgress -= (int)num * 1000;
			if (_levelCount + (int)num < _obj.LevelIcons.Length)
			{
				_levelCount += (int)num;
			}
			else
			{
				_levelCount = _obj.LevelIcons.Length;
				if (PlayerPrefsSafe.GetInt("AccMedal_Quality") < _obj.MedalIcons.Length - 1)
				{
					_obj.LevelProgress.SetActive(value: false);
					_obj.ButtonMedal.SetActive(value: true);
				}
			}
			_obj.LevelIcon.sprite = _obj.LevelIcons[_levelCount - 1];
			_obj.LevelText.text = _levelCount.ToString();
			PlayerPrefsSafe.SetInt("LevelCount", _levelCount);
		}
		PlayerPrefsSafe.SetInt("LevelProgress", _levelProgress);
		_obj.LevelProgressText.text = _levelProgress + " | 1000";
		_obj.LevelProgressCount.fillAmount = (float)_levelProgress / 1000f;
		PlayerPrefs.Save();
	}

	public static void SetMoney(int _money)
	{
		if (_money > 1000000)
		{
			_money = 1000000;
		}
		else if (_money < -1000000)
		{
			_money = -1000000;
		}
		PlayerPrefsSafe.SetInt("Money", _money);
		PlayerPrefs.Save();
		_moneyText.text = _money.ToString();
		_transform.sizeDelta = new Vector2(130.25f + 20.5f * (float)_money.ToString().Length, _transform.sizeDelta.y);
		if (_money >= 0)
		{
			_obj.BlockInven.SetActive(value: false);
		}
		else
		{
			_obj.BlockInven.SetActive(value: true);
		}
	}

	public static void AddMoney(int _money)
	{
		_money += PlayerPrefsSafe.GetInt("Money");
		if (_money > 1000000)
		{
			_money = 1000000;
		}
		else if (_money < -1000000)
		{
			_money = -1000000;
		}
		PlayerPrefsSafe.SetInt("Money", _money);
		PlayerPrefs.Save();
		_moneyText.text = _money.ToString();
		_transform.sizeDelta = new Vector2(130.25f + 20.5f * (float)_money.ToString().Length, _transform.sizeDelta.y);
		if (_money >= 0)
		{
			_obj.BlockInven.SetActive(value: false);
		}
		else
		{
			_obj.BlockInven.SetActive(value: true);
		}
	}

	public static void MedalSet()
	{
		if (PlayerPrefsSafe.GetInt("AccMedalNumber") != 0)
		{
			_obj.AccMedal.sprite = _obj.MedalIcons[PlayerPrefsSafe.GetInt("AccMedalNumber")];
			_obj.AccMedal.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			_obj.AccMedal.sprite = _obj.MedalIcons[0];
			_obj.AccMedal.transform.localScale = new Vector3(0f, 0f, 0f);
		}
	}

	public void Context_OpenButton()
	{
		SaveItem.OpenWindowCase();
	}

	public void Context_ActiveButton()
	{
		SaveItem.ActiveItem();
	}

	public void Context_SellButton()
	{
		SaveItem.PredSellItem();
	}

	public void BuyItem()
	{
		SaveItem.BuyItem();
	}

	public void SellItem()
	{
		SaveItem.SellItem();
	}

	public void LeaveGame()
	{
		Time.timeScale = 1f;
		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.LoadLevel(0);
		}
		else
		{
			LoadLevel(0);
		}
	}

	public override void OnLeftRoom()
	{
		ServerManager._inRoom = false;
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void ClearInventory()
	{
		for (int i = 1; i <= PlayerPrefsSafe.GetInt("ItemSaveCount"); i++)
		{
			PlayerPrefsSafe.DeleteKey("ItemOnDelete_" + i);
			PlayerPrefsSafe.DeleteKey("ItemIdSell_" + i);
			PlayerPrefsSafe.DeleteKey("ItemIsActiveInt_" + i);
			PlayerPrefsSafe.DeleteKey("ItemIsNewInt_" + i);
		}
		PlayerPrefsSafe.SetInt("ItemSaveCount", 0);
		PlayerPrefs.Save();
		SceneManager.LoadScene(sceneIndex);
	}

	public void ClearData()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefsSafe.SetInt("GiftPickup", 1);
		PlayerPrefs.Save();
		SceneManager.LoadScene(sceneIndex);
	}

	public void OpenURL(string _url)
	{
		Application.OpenURL(_url);
	}

	public void BankPlay(string _eventName)
	{
		RuntimeManager.PlayOneShot(_eventName);
	}
}
