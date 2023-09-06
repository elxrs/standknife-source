using System.Collections;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
	public int TypeNumber;

	public GameObject[] Models;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Equals) & (TypeNumber < Models.Length - 1))
		{
			Models[TypeNumber].SetActive(value: false);
			TypeNumber++;
			Models[TypeNumber].SetActive(value: true);
			Debug.Log("(+)[" + TypeNumber + "] " + Models[TypeNumber].name + ".png");
		}
		else if (Input.GetKeyDown(KeyCode.Minus) & (TypeNumber > 0))
		{
			Models[TypeNumber].SetActive(value: false);
			TypeNumber--;
			Models[TypeNumber].SetActive(value: true);
			Debug.Log("(-)[" + TypeNumber + "] " + Models[TypeNumber].name + ".png");
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			ScreenCapture.CaptureScreenshot(Models[TypeNumber].name + ".png");
			Debug.Log("СКРИНШОТ| " + Models[TypeNumber].name + ".png");
		}
		else if (Input.GetKeyDown(KeyCode.Keypad0))
		{
			StartCoroutine(ScreenshotAll());
		}
	}

	private IEnumerator ScreenshotAll()
	{
		Debug.Log("!!! |СКРИНШОТ ВСЁ (СТАРТ)| !!!");
		Models[TypeNumber].SetActive(value: false);
		TypeNumber = 0;
		Models[TypeNumber].SetActive(value: true);
		ScreenCapture.CaptureScreenshot(Models[TypeNumber].name + ".png");
		yield return new WaitForSeconds(0.2f);
		Debug.Log(Models[TypeNumber].name + ".png");
		for (int o = 0; o < Models.Length - 1; o++)
		{
			Models[TypeNumber].SetActive(value: false);
			TypeNumber++;
			Models[TypeNumber].SetActive(value: true);
			ScreenCapture.CaptureScreenshot(Models[TypeNumber].name + ".png");
			yield return new WaitForSeconds(0.2f);
			Debug.Log(Models[TypeNumber].name + ".png");
		}
		Debug.Log("!!! |СКРИНШОТ ВСЁ (ФИНИШ)| !!!");
	}
}
