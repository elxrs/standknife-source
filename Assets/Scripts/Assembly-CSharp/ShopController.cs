using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
	public enum Type
	{
		Item = 0,
		Salled = 1,
		Blocked = 2,
		Case = 3,
		None = 4
	}

	public Type mode;

	public bool isNewItem;

	public Sprite Preview;

	public string NameItem = "Bayonet";

	public string PodNameItem = "Scratch";

	public int _numberItem;

	public int _classItem = 1;

	public int _typeItem;

	public int _typeItemSkin;

	public int _colorType;

	public int _cena;

	public int _priorityItem;

	[Header("Префаб:")]
	public GameObject ItemContext;

	public GameObject ItemToggle;

	public GameObject Sell;

	public GameObject Blocked;

	public GameObject NewItem;

	public Image PreviewImage;

	public Image NameColorBC;

	public Image Checkmark;

	public Text NameText;

	public Text CenaText;

	public bool isBuildParent;

	private Int Cena;

	[HideInInspector]
	public int isBlockSell;

	private GameObject ErrorWindow;

	private GameObject BuyWindow;

	private Text BuyText;

	private GameObject SellWindow;

	private Text SellText;

	private void Start()
	{
		Cena = _cena;
		LoadItem();
	}

	public void LoadItem()
	{
		if (Application.isPlaying)
		{
			switch (mode)
			{
			case Type.Item:
				ItemContext.SetActive(value: true);
				Sell.SetActive(value: false);
				Blocked.SetActive(value: false);
				ItemToggle.GetComponent<Toggle>().group = ControllerInventory._obj.GroupItems[_classItem];
				if (isNewItem)
				{
					NewItem.SetActive(value: true);
				}
				break;
			case Type.Salled:
				ItemContext.SetActive(value: false);
				Sell.SetActive(value: true);
				Blocked.SetActive(value: false);
				NewItem.SetActive(value: false);
				break;
			case Type.Blocked:
				ItemContext.SetActive(value: false);
				Sell.SetActive(value: false);
				Blocked.SetActive(value: true);
				NewItem.SetActive(value: false);
				break;
			case Type.Case:
				ItemContext.SetActive(value: true);
				Sell.SetActive(value: false);
				Blocked.SetActive(value: false);
				if (isNewItem)
				{
					NewItem.SetActive(value: true);
				}
				break;
			case Type.None:
				ItemContext.SetActive(value: false);
				Sell.SetActive(value: false);
				Blocked.SetActive(value: false);
				NewItem.SetActive(value: false);
				break;
			}
			if (mode != Type.None)
			{
				ErrorWindow = GameObject.Find("ErrorWindow");
				BuyWindow = GameObject.Find("BuyWindow");
				BuyText = GameObject.Find("BuyText").GetComponent<Text>();
				SellWindow = GameObject.Find("SellWindow");
				SellText = GameObject.Find("SellText").GetComponent<Text>();
			}
			else
			{
				ErrorWindow = null;
				BuyWindow = null;
				BuyText = null;
			}
			PreviewImage.sprite = Preview;
			PreviewImage.gameObject.SetActive(value: true);
		}
		if (PodNameItem != "")
		{
			if (_classItem != 5)
			{
				NameText.text = NameItem + " ''" + PodNameItem + "''";
			}
			else
			{
				NameText.text = "''" + PodNameItem + "'' " + NameItem;
			}
		}
		else
		{
			NameText.text = NameItem;
		}
		NameColorBC.color = ControllerInventory._obj._nameColorItems[_colorType];
		CenaText.text = Cena.ToString();
	}

	public void PredBuyItem()
	{
		RuntimeManager.PlayOneShot("event:/UI/ui default");
		if (PodNameItem != "")
		{
			BuyText.text = string.Concat("Вы хотите купить <color=white>", NameItem, "\n ''", PodNameItem, "''</color> за <color=yellow>G.", Cena, "</color>?");
		}
		else
		{
			BuyText.text = string.Concat("Вы хотите купить <color=white>", NameItem, "</color>\n за <color=yellow>G.", Cena, "</color>?");
		}
		Controller.SaveItem = this;
		BuyWindow.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void BuyItem()
	{
		if (PlayerPrefsSafe.GetInt("Money") >= (int)Cena)
		{
			Controller.AddMoney(-(int)Cena);
			ControllerInventory.BuyItem(_numberItem);
		}
		else
		{
			ErrorWindow.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void PredSellItem()
	{
		if (PodNameItem != "")
		{
			SellText.text = string.Concat("Вы хотите продать <color=white>", NameItem, "\n ''", PodNameItem, "''</color> за <color=yellow>G.", Cena, "</color>?");
		}
		else
		{
			SellText.text = string.Concat("Вы хотите продать <color=white>", NameItem, "</color>\n за <color=yellow>G.", Cena, "</color>?");
		}
		Controller.SaveItem = this;
		SellWindow.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void SellItem()
	{
		Controller.AddMoney(Cena);
		DeleteItem();
	}

	public void DeleteItem()
	{
		PlayerPrefsSafe.SetInt("ItemOnDelete_" + _numberItem, 1);
		PlayerPrefsSafe.DeleteKey("ItemIdSell_" + _numberItem);
		PlayerPrefsSafe.DeleteKey("ItemIsActiveInt_" + _numberItem);
		PlayerPrefsSafe.DeleteKey("ItemIsNewInt_" + _numberItem);
		if (isBuildParent)
		{
			ControllerInventory.CountBuildParent--;
		}
		if (ItemToggle.GetComponent<Toggle>().isOn)
		{
			ActiveItem();
		}
		ControllerInventory.CountItem--;
		ControllerInventory._obj.TextItemCount.text = "ПРЕДМЕТОВ: " + ControllerInventory.CountItem;
		Object.Destroy(base.gameObject);
	}

	public void OpenContextMenu(ContextVisov Button)
	{
		RuntimeManager.PlayOneShot("event:/UI/ui horizontal");
		if (isNewItem)
		{
			IsNewItem();
		}
		Controller._obj.ContextMenu.transform.position = Button.PositionDown;
		if (Controller._obj.ContextMenu.transform.localPosition.x < 395f)
		{
			Controller._obj.ContextMenu.transform.localScale = new Vector3(1f, 1f, 1f);
			Controller._obj.ContextGroup.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			Controller._obj.ContextMenu.transform.localScale = new Vector3(-1f, 1f, 1f);
			Controller._obj.ContextGroup.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		switch (mode)
		{
		case Type.Item:
			Controller._obj.ContextGroup.transform.GetChild(0).gameObject.SetActive(value: false);
			if (!ItemToggle.GetComponent<Toggle>().isOn)
			{
				Controller._obj.ContextGroup.transform.GetChild(1).gameObject.SetActive(value: true);
				Controller._obj.ContextGroup.transform.GetChild(2).gameObject.SetActive(value: false);
			}
			else
			{
				Controller._obj.ContextGroup.transform.GetChild(1).gameObject.SetActive(value: false);
				Controller._obj.ContextGroup.transform.GetChild(2).gameObject.SetActive(value: true);
			}
			if ((isBlockSell == 0) & (PlayerPrefs.GetInt("GameMode") == 1))
			{
				Controller._obj.ContextGroup.transform.GetChild(3).gameObject.SetActive(value: true);
			}
			else
			{
				Controller._obj.ContextGroup.transform.GetChild(3).gameObject.SetActive(value: false);
			}
			break;
		case Type.Case:
			Controller._obj.ContextGroup.transform.GetChild(0).gameObject.SetActive(value: true);
			Controller._obj.ContextGroup.transform.GetChild(1).gameObject.SetActive(value: false);
			Controller._obj.ContextGroup.transform.GetChild(2).gameObject.SetActive(value: false);
			Controller._obj.ContextGroup.transform.GetChild(3).gameObject.SetActive(value: false);
			break;
		}
		Controller.SaveItem = this;
		Controller._obj.ContextObject.SetActive(value: true);
	}

	public void ActiveItem()
	{
		ItemToggle.GetComponent<Toggle>().isOn = !ItemToggle.GetComponent<Toggle>().isOn;
		if (ItemToggle.GetComponent<Toggle>().isOn)
		{
			switch (_classItem)
			{
			case 0:
				PlayerPrefsSafe.SetInt("TypeWeapon3", _typeItem);
				PlayerPrefsSafe.SetInt("TypeSkin3", _typeItemSkin);
				break;
			case 4:
				PlayerPrefsSafe.SetInt("TypeWeapon", _typeItem);
				PlayerPrefsSafe.SetInt("TypeSkin", _typeItemSkin);
				break;
			case 1:
				PlayerPrefsSafe.SetInt("TypeGloves", _typeItem);
				break;
			case 2:
				PlayerPrefsSafe.SetInt("TypeHands", _typeItem);
				break;
			case 3:
				PlayerPrefsSafe.SetInt("AccMedalNumber", _typeItem);
				Controller.MedalSet();
				break;
			}
		}
		else
		{
			switch (_classItem)
			{
			case 0:
				PlayerPrefsSafe.SetInt("TypeWeapon3", 0);
				PlayerPrefsSafe.SetInt("TypeSkin3", 0);
				break;
			case 4:
				PlayerPrefsSafe.SetInt("TypeWeapon", 16);
				PlayerPrefsSafe.SetInt("TypeSkin", 0);
				break;
			case 1:
				PlayerPrefsSafe.SetInt("TypeGloves", 0);
				break;
			case 2:
				PlayerPrefsSafe.SetInt("TypeHands", 0);
				break;
			case 3:
				PlayerPrefsSafe.SetInt("AccMedalNumber", _typeItem);
				Controller.MedalSet();
				break;
			}
		}
		SaveActiveItem();
	}

	public void SaveActiveItem()
	{
		if (_classItem != 3)
		{
			switch (ItemToggle.GetComponent<Toggle>().isOn)
			{
			case true:
				PlayerPrefsSafe.SetInt("ItemIsActiveInt_" + _numberItem, 1);
				break;
			case false:
				PlayerPrefsSafe.SetInt("ItemIsActiveInt_" + _numberItem, 0);
				break;
			}
		}
		else
		{
			switch (ItemToggle.GetComponent<Toggle>().isOn)
			{
			case true:
				PlayerPrefsSafe.SetInt("AccMedal_IsActiveInt" + _typeItem, 1);
				break;
			case false:
				PlayerPrefsSafe.SetInt("AccMedal_IsActiveInt" + _typeItem, 0);
				break;
			}
		}
		PlayerPrefs.Save();
	}

	public void IsNewItem()
	{
		if (_classItem != 3)
		{
			PlayerPrefsSafe.SetInt("ItemIsNewInt_" + _numberItem, 0);
		}
		else
		{
			PlayerPrefsSafe.SetInt("AccMedal_IsNewInt" + _typeItem, 0);
		}
		PlayerPrefs.Save();
		isNewItem = false;
		NewItem.SetActive(value: false);
		isBuildParent = true;
		ControllerInventory.CountBuildParent++;
		ControllerInventory.CountNewItem--;
		if ((ControllerInventory.CountNewItem != 0) & (ControllerInventory.CountNewItem < 100))
		{
			ControllerInventory.TextNewItem.text = ControllerInventory.CountNewItem.ToString();
		}
		else if (ControllerInventory.CountNewItem == 0)
		{
			ControllerInventory.TextNewItem.text = "";
		}
		else
		{
			ControllerInventory.TextNewItem.text = "99+";
		}
	}

	public void OpenWindowCase()
	{
		CaseController._obj.CaseLoad(PodNameItem, _typeItem, this);
	}
}
