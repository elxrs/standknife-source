using System;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInventory : MonoBehaviour
{
	[Serializable]
	private class Badge
	{
		public string Name = "Badge ''None'' Bronze";

		public Sprite Preview;

		public int _typeBadge;

		public int _typeColor;

		public int _priority;
	}

	public static ControllerInventory _obj;

	public ShopController ItemPrefab;

	public GameObject ItemsInvent;

	public Text TextItemCount;

	public GameObject GiftWindow;

	public ShopController[] AllItems;

	public ToggleGroup[] GroupItems;

	public Color[] _nameColorItems;

	[SerializeField]
	private Badge[] Medals;

	public Achive LevelAchive;

	private ShopController MedalQuality;

	public static Text TextNewItem;

	public static int CountItem;

	public static int CountNewItem;

	public static int CountBuildParent;

	private void Awake()
	{
		_obj = this;
		CountItem = ItemsInvent.transform.childCount;
		CountNewItem = 0;
		CountBuildParent = 0;
		TextItemCount.text = "ПРЕДМЕТОВ: " + CountItem;
		TextNewItem = GameObject.Find("TextNew").GetComponent<Text>();
		LoadItem();
	}

	public static void BuyItem(int _idItem)
	{
		int num = PlayerPrefsSafe.GetInt("ItemSaveCount") + 1;
		PlayerPrefsSafe.SetInt("ItemSaveCount", num);
		CreateItem(num, _idItem, 0, 1);
		PlayerPrefsSafe.SetInt("ItemIdSell_" + num, _idItem);
		PlayerPrefsSafe.SetInt("ItemIsActiveInt_" + num, 0);
		PlayerPrefsSafe.SetInt("ItemIsNewInt_" + num, 1);
		PlayerPrefs.Save();
	}

	public static void CreateItem(int _itemNumber, int _idItem, int _isActiveItem, int _isNewItem)
	{
		int cena = (int)((float)_obj.AllItems[_idItem]._cena * 0.8f);
		ShopController component = UnityEngine.Object.Instantiate(_obj.ItemPrefab, new Vector2(0f, 0f), Quaternion.identity).GetComponent<ShopController>();
		component.Preview = _obj.AllItems[_idItem].Preview;
		component.NameItem = _obj.AllItems[_idItem].NameItem;
		component.PodNameItem = _obj.AllItems[_idItem].PodNameItem;
		component._colorType = _obj.AllItems[_idItem]._colorType;
		component._classItem = _obj.AllItems[_idItem]._classItem;
		if (component._classItem == 5)
		{
			component.mode = ShopController.Type.Case;
		}
		component._typeItem = _obj.AllItems[_idItem]._typeItem;
		component._typeItemSkin = _obj.AllItems[_idItem]._typeItemSkin;
		component._cena = cena;
		component._priorityItem = _obj.AllItems[_idItem]._priorityItem;
		component._numberItem = _itemNumber;
		if (_obj.AllItems[_idItem].mode != ShopController.Type.Blocked)
		{
			component.isBlockSell = 0;
		}
		else
		{
			component.isBlockSell = 1;
		}
		if (_obj.AllItems[_idItem].mode != 0)
		{
			_ = 2;
		}
		switch (_isNewItem)
		{
		case 0:
			component.isNewItem = false;
			component.isBuildParent = true;
			CountBuildParent++;
			break;
		case 1:
			component.isNewItem = true;
			CountNewItem++;
			if (CountNewItem < 100)
			{
				TextNewItem.text = CountNewItem.ToString();
			}
			else
			{
				TextNewItem.text = "99+";
			}
			break;
		}
		if (_isActiveItem == 1)
		{
			component.ActiveItem();
		}
		component.transform.SetParent(_obj.ItemsInvent.transform);
		component.transform.localScale = new Vector2(1f, 1f);
		component.transform.SetSiblingIndex(0);
		CountItem++;
		_obj.TextItemCount.text = "ПРЕДМЕТОВ: " + CountItem;
	}

	public void LoadItem()
	{
		for (int i = 0; i < AllItems.Length; i++)
		{
			AllItems[i]._numberItem = i;
		}
		if ((PlayerPrefsSafe.GetInt("AccMedal_Quality") != 0) & (PlayerPrefs.GetInt("GameMode") == 1))
		{
			ShopController component = UnityEngine.Object.Instantiate(ItemPrefab, new Vector2(0f, 0f), Quaternion.identity).GetComponent<ShopController>();
			component.NameItem = Medals[PlayerPrefsSafe.GetInt("AccMedal_Quality")].Name;
			component.Preview = Medals[PlayerPrefsSafe.GetInt("AccMedal_Quality")].Preview;
			component._typeItem = Medals[PlayerPrefsSafe.GetInt("AccMedal_Quality")]._typeBadge;
			component._colorType = Medals[PlayerPrefsSafe.GetInt("AccMedal_Quality")]._typeColor;
			component._priorityItem = Medals[PlayerPrefsSafe.GetInt("AccMedal_Quality")]._priority;
			component.isBlockSell = 1;
			component._classItem = 3;
			component.transform.SetParent(ItemsInvent.transform);
			component.transform.localScale = new Vector2(1f, 1f);
			component.transform.SetSiblingIndex(0);
			switch (PlayerPrefsSafe.GetInt("AccMedal_IsNewInt" + PlayerPrefsSafe.GetInt("AccMedal_Quality")))
			{
			case 0:
				component.isNewItem = false;
				component.isBuildParent = true;
				CountBuildParent++;
				break;
			case 1:
				component.isNewItem = true;
				CountNewItem++;
				if (CountNewItem < 100)
				{
					TextNewItem.text = CountNewItem.ToString();
				}
				else
				{
					TextNewItem.text = "99+";
				}
				break;
			}
			if (PlayerPrefsSafe.GetInt("AccMedal_IsActiveInt" + PlayerPrefsSafe.GetInt("AccMedal_Quality")) == 1)
			{
				component.ItemToggle.GetComponent<Toggle>().isOn = true;
			}
			MedalQuality = component;
			CountItem++;
			_obj.TextItemCount.text = "ПРЕДМЕТОВ: " + CountItem;
		}
		if ((PlayerPrefsSafe.GetInt("GiftPickup") == 0) & (PlayerPrefs.GetInt("GameMode") == 1))
		{
			GiftWindow.SetActive(value: true);
			Controller.AddMoney(100000);
			PlayerPrefsSafe.SetInt("SuperPrizeID", 160);
			if (PlayerPrefsSafe.GetInt("AccMedal_Quality") == 0)
			{
				UpdateMedal();
			}
			PlayerPrefsSafe.SetInt("GiftPickup", 1);
		}
		else if (PlayerPrefs.GetInt("GameMode") == 1)
		{
			for (int j = 1; j <= PlayerPrefsSafe.GetInt("ItemSaveCount"); j++)
			{
				if (PlayerPrefsSafe.GetInt("ItemOnDelete_" + j) != 1)
				{
					CreateItem(j, PlayerPrefsSafe.GetInt("ItemIdSell_" + j), PlayerPrefsSafe.GetInt("ItemIsActiveInt_" + j), PlayerPrefsSafe.GetInt("ItemIsNewInt_" + j));
				}
			}
		}
		else if (PlayerPrefs.GetInt("GameMode") == 2)
		{
			for (int k = 0; k < AllItems.Length; k++)
			{
				if (AllItems[k].gameObject.activeSelf & (AllItems[k]._classItem != 5))
				{
					CreateItem(k, k, PlayerPrefsSafe.GetInt("ItemIsActiveInt_" + k), 0);
				}
			}
		}
		if (PlayerPrefsSafe.GetInt("SuperPrizeID") != 0)
		{
			BuyItem(PlayerPrefsSafe.GetInt("SuperPrizeID"));
		}
		PlayerPrefsSafe.SetInt("SuperPrizeID", 0);
	}

	public void UpdateMedal()
	{
		int num = ((PlayerPrefsSafe.GetInt("AccMedal_Quality") + 1 >= Medals.Length) ? PlayerPrefsSafe.GetInt("AccMedal_Quality") : (PlayerPrefsSafe.GetInt("AccMedal_Quality") + 1));
		PlayerPrefsSafe.SetInt("AccMedal_Quality", num);
		if (MedalQuality != null)
		{
			if (MedalQuality.isBuildParent)
			{
				CountBuildParent--;
			}
			if (MedalQuality.isNewItem)
			{
				CountNewItem--;
			}
			UnityEngine.Object.Destroy(MedalQuality.gameObject);
		}
		else
		{
			CountItem++;
			_obj.TextItemCount.text = "ПРЕДМЕТОВ: " + CountItem;
		}
		ShopController component = UnityEngine.Object.Instantiate(ItemPrefab, new Vector2(0f, 0f), Quaternion.identity).GetComponent<ShopController>();
		component.NameItem = Medals[num].Name;
		component.Preview = Medals[num].Preview;
		component._typeItem = Medals[num]._typeBadge;
		component._colorType = Medals[num]._typeColor;
		component._priorityItem = Medals[num]._priority;
		component.isBlockSell = 1;
		component._classItem = 3;
		component.transform.SetParent(ItemsInvent.transform);
		component.transform.localScale = new Vector2(1f, 1f);
		component.transform.SetSiblingIndex(0);
		component.isNewItem = true;
		MedalQuality = component;
		CountNewItem++;
		if (CountNewItem < 100)
		{
			TextNewItem.text = CountNewItem.ToString();
		}
		else
		{
			TextNewItem.text = "99+";
		}
		PlayerPrefsSafe.SetInt("AccMedal_IsNewInt" + num, 1);
		PlayerPrefs.Save();
		LevelAchive.LoadAchive();
	}

	public static void BuildParents()
	{
		while (CountBuildParent > 0)
		{
			for (int i = 0; i < _obj.ItemsInvent.transform.childCount; i++)
			{
				if (!(_obj.ItemsInvent.transform.GetChild(i).GetComponent<ShopController>().isBuildParent & !_obj.ItemsInvent.transform.GetChild(i).GetComponent<ShopController>().isNewItem))
				{
					continue;
				}
				Transform child = _obj.ItemsInvent.transform.GetChild(i);
				int siblingIndex = 0;
				for (int j = 0; j < _obj.ItemsInvent.transform.childCount; j++)
				{
					siblingIndex = j;
					if ((_obj.ItemsInvent.transform.GetChild(j) != child) & !_obj.ItemsInvent.transform.GetChild(j).GetComponent<ShopController>().isBuildParent & !_obj.ItemsInvent.transform.GetChild(j).GetComponent<ShopController>().isNewItem)
					{
						if (_obj.ItemsInvent.transform.GetChild(j).GetComponent<ShopController>()._priorityItem < child.GetComponent<ShopController>()._priorityItem)
						{
							siblingIndex = j - 1;
							break;
						}
						if ((_obj.ItemsInvent.transform.GetChild(j).GetComponent<ShopController>()._priorityItem == child.GetComponent<ShopController>()._priorityItem) & (_obj.ItemsInvent.transform.GetChild(j).GetComponent<ShopController>()._cena <= child.GetComponent<ShopController>()._cena))
						{
							siblingIndex = j - 1;
							break;
						}
					}
				}
				child.SetSiblingIndex(siblingIndex);
				child.GetComponent<ShopController>().isBuildParent = false;
				CountBuildParent--;
				break;
			}
		}
	}
}
