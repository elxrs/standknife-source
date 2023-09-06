using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class CCController : MonoBehaviour
{
	public InputField InputCode;

	public Text ResultText;

	public string _cmdPrest;

	public string[] _commands;

	private void OnEnable()
	{
		InputCode.text = "";
		ResultText.text = "";
	}

	public void CheatUse()
	{
		RuntimeManager.PlayOneShot("event:/UI/ui default");
		string[] array = InputCode.text.Split(null);
		int num = 0;
		int num2 = 0;
		InputCode.text = "";
		if (array.Length > 1)
		{
			ResultText.text = "Неправильный ввод значения";
			num = int.Parse(array[1]);
			if (array.Length > 2)
			{
				num2 = int.Parse(array[2]);
			}
			if (array[0] == _cmdPrest + _commands[0])
			{
				if ((num >= 0) & (num < ControllerInventory._obj.AllItems.Length))
				{
					ControllerInventory.BuyItem(num);
					ResultText.text = "Успешно";
				}
			}
			else if (array[0] == _cmdPrest + _commands[1])
			{
				if (num >= -1000000 && num <= 1000000)
				{
					Controller.AddMoney(num);
					ResultText.text = "Успешно";
				}
			}
			else if (array[0] == _cmdPrest + _commands[2])
			{
				if (num >= -1000000 && num <= 1000000)
				{
					Controller.SetMoney(num);
					ResultText.text = "Успешно";
				}
			}
			else if (array[0] == _cmdPrest + _commands[3])
			{
				if (num >= 1 && num <= 50)
				{
					Controller.SetLevel(num, 0);
					ResultText.text = "Успешно";
				}
			}
			else if (array[0] == _cmdPrest + _commands[4])
			{
				num--;
				if ((num >= 0 && num < 7 && num2 >= 0) & (num2 <= AchiveController._obj.Achives[num]._countMain[AchiveController._obj.Achives[num]._countMain.Length - 1]))
				{
					switch (AchiveController._obj.Achives[num].modeZad)
					{
					case Achive.Type.KillDummy:
						PlayerPrefsSafe.SetInt("CountDeadDummy", num2);
						break;
					case Achive.Type.KillOther:
						PlayerPrefsSafe.SetInt("CountDeadOther", num2);
						break;
					case Achive.Type.RecordKillEnemy:
						PlayerPrefsSafe.SetInt("RecordMod2_15", num2);
						break;
					case Achive.Type.LevelUp:
						PlayerPrefsSafe.SetInt("AccMedal_Quality", num2);
						ControllerInventory._obj.UpdateMedal();
						break;
					case Achive.Type.KillDummyHead:
						PlayerPrefsSafe.SetInt("CountDeadDummyHead", num2);
						break;
					case Achive.Type.KillEnemyTimer:
						PlayerPrefsSafe.SetInt("CountDeadEnemyTimer", num2);
						break;
					case Achive.Type.FinishRounds:
						PlayerPrefsSafe.SetInt("CountFinishRounds", num2);
						break;
					case Achive.Type.KillDummyWeapon:
						PlayerPrefsSafe.SetInt("CountDeadDummyWeapon", num2);
						break;
					}
					AchiveController._obj.Achives[num].LoadAchive();
					ResultText.text = "Успешно";
				}
			}
			else if (array[0] == _cmdPrest + _commands[5])
			{
				if (num >= 0 && num <= 1)
				{
					PlayerPrefsSafe.SetInt("WhActive", num);
					ResultText.text = "Успешно";
				}
			}
			else if (array[0] == _cmdPrest + _commands[6])
			{
				if (num >= 0 && num <= 1)
				{
					PlayerPrefsSafe.SetInt("AimActive", num);
					ResultText.text = "Успешно";
				}
			}
			else if (array[0] == _cmdPrest + _commands[7])
			{
				if (num >= 0 && num <= 100)
				{
					PlayerPrefsSafe.SetInt("BoostLuck", num);
					ResultText.text = "Успешно";
				}
			}
			else if (array[0] == _cmdPrest + _commands[8])
			{
				PlayerPrefs.DeleteAll();
				PlayerPrefs.Save();
				ResultText.text = "Успешно";
			}
			else
			{
				ResultText.text = "Код не найден";
			}
		}
		else if (array.Length != 0)
		{
			ResultText.text = "Неправильный ввод кода";
		}
		else
		{
			ResultText.text = "";
		}
	}
}
