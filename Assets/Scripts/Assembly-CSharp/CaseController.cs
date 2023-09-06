using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class CaseController : MonoBehaviour
{
	[Serializable]
	private class CasePrize
	{
		public string Name = "Kabar";

		public bool _isGiftPrizes;

		public ShopController[] Prizes;
	}

	public static CaseController _obj;

	public GameObject CaseWindow;

	public Text CaseNameText;

	public Transform Spawner;

	public GameObject PrizesAll;

	public GameObject ScrollPanel;

	public GameObject ScrollPrefab;

	public Button[] AllButtons;

	[SerializeField]
	private CasePrize[] CasePrizes;

	private ShopController CaseItem;

	private ShopController FinalDrop;

	private int dropCount;

	private int _indexCase;

	private float m_StepCycle;

	private void Start()
	{
		_obj = this;
		dropCount = 45;
	}

	public void CaseLoad(string _caseName, int _typeCase, ShopController _caseItem)
	{
		CaseNameText.text = "''" + _caseName + "'' Кейс";
		_indexCase = _typeCase;
		CaseItem = _caseItem;
		for (int i = 0; i < PrizesAll.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(PrizesAll.transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < CasePrizes[_indexCase].Prizes.Length; j++)
		{
			ShopController shopController = UnityEngine.Object.Instantiate(CasePrizes[_indexCase].Prizes[j], new Vector2(0f, 0f), Quaternion.identity);
			shopController.mode = ShopController.Type.None;
			shopController.LoadItem();
			shopController.gameObject.SetActive(value: true);
			shopController.transform.SetParent(PrizesAll.transform);
			shopController.transform.localScale = new Vector2(1f, 1f);
		}
		CaseWindow.SetActive(value: true);
	}

	public void Open()
	{
		StartCoroutine(Opene());
	}

	private IEnumerator Opene()
	{
		RuntimeManager.PlayOneShot("event:/UI/open_case_click");
		CaseItem.DeleteItem();
		for (int i = 0; i < AllButtons.Length; i++)
		{
			AllButtons[i].interactable = false;
		}
		Spawner.localPosition = new Vector3(UnityEngine.Random.Range(11690, 11300), 0f, 0f);
		for (int j = 0; j < dropCount + 3; j++)
		{
			int num = UnityEngine.Random.Range(0, CasePrizes[_indexCase].Prizes.Length);
			int num2 = UnityEngine.Random.Range(0, 100);
			if (j == dropCount - 1)
			{
				if ((PlayerPrefsSafe.GetInt("BoostLuck") != 0) & (num2 < CasePrizes[_indexCase].Prizes.Length + PlayerPrefsSafe.GetInt("BoostLuck")))
				{
					FinalDrop = UnityEngine.Object.Instantiate(CasePrizes[_indexCase].Prizes[CasePrizes[_indexCase].Prizes.Length - 1], new Vector2(0f, 0f), Quaternion.identity);
				}
				else if (CasePrizes[_indexCase]._isGiftPrizes)
				{
					num = UnityEngine.Random.Range(0, CasePrizes[_indexCase].Prizes.Length - 1);
					FinalDrop = UnityEngine.Object.Instantiate(CasePrizes[_indexCase].Prizes[num], new Vector2(0f, 0f), Quaternion.identity);
				}
				else
				{
					FinalDrop = UnityEngine.Object.Instantiate(CasePrizes[_indexCase].Prizes[num], new Vector2(0f, 0f), Quaternion.identity);
				}
				FinalDrop.mode = ShopController.Type.None;
				FinalDrop.LoadItem();
				FinalDrop.gameObject.SetActive(value: true);
				FinalDrop.transform.SetParent(ScrollPanel.transform);
				FinalDrop.transform.localScale = new Vector2(1f, 1f);
				FinalDrop.name += " ПРИЗ!!!";
				ControllerInventory.BuyItem(FinalDrop._numberItem);
			}
			else
			{
				ShopController shopController = UnityEngine.Object.Instantiate(CasePrizes[_indexCase].Prizes[num], new Vector2(0f, 0f), Quaternion.identity);
				shopController.mode = ShopController.Type.None;
				shopController.LoadItem();
				shopController.gameObject.SetActive(value: true);
				shopController.transform.SetParent(ScrollPanel.transform);
				shopController.transform.localScale = new Vector2(1f, 1f);
				shopController.name += j;
			}
		}
		StartCoroutine(PlayHitAudio());
		ScrollPanel.GetComponent<Animation>().Play();
		yield return new WaitForSeconds(7.5f);
		for (int k = 0; k < AllButtons.Length; k++)
		{
			AllButtons[k].interactable = true;
		}
		Spawner.localPosition = new Vector3(11492.5f, 0f, 0f);
		UnityEngine.Object.Destroy(ScrollPanel);
		ScrollPanel = UnityEngine.Object.Instantiate(ScrollPrefab, Spawner.position, Spawner.rotation);
		ScrollPanel.transform.SetParent(Spawner);
		ScrollPanel.transform.localScale = new Vector2(1f, 1f);
		ScrollPanel.name = "ScrollPanel";
		Controller._obj._poslMenu = 0;
		Controller._obj.SwitchMenu(2);
		CaseWindow.SetActive(value: false);
	}

	private IEnumerator PlayHitAudio()
	{
		float speed = 100f;
		while (true)
		{
			speed -= 15f * Time.deltaTime;
			if (speed <= 0f)
			{
				break;
			}
			m_StepCycle += speed * 12f * Time.deltaTime;
			if (m_StepCycle > 100f)
			{
				m_StepCycle = 0f;
				RuntimeManager.PlayOneShot("event:/UI/open_case_click");
			}
			yield return new WaitForSeconds(0.001f);
		}
	}
}
