using UnityEngine;
using UnityEngine.UI;

public class ControllerSettings : MonoBehaviour
{
	public Color EnableButtonColor;

	public Color DisableButtonColor;

	[Header("Вид рук 1:")]
	public Image[] ButtonsHands1;

	[Header("Вид рук 2:")]
	public Image[] ButtonsHands2;

	[Header("Пост Обработка:")]
	public Image[] ButtonsPost;

	public Toggle ToogleOcclusion;

	public Toggle ToogleGrading;

	public Toggle ToogleBloom;

	public Toggle ToogleBlur;

	public Toggle ToogleVignette;

	private bool _isChangePost;

	[Header("Текстуры:")]
	public Image[] ButtonsTexture;

	[Header("Модели:")]
	public Image[] ButtonsModels;

	[Header("Тени:")]
	public Image[] ButtonsShadows;

	[Header("Сглаживание:")]
	public Text CountTextAliasing;

	public string[] _strAliasing;

	public int _countAliasing;

	[Header("Анизотропная:")]
	public Text CountTextAnis;

	public string[] _strAnis;

	public int _countAnis;

	[Header("Эффекты:")]
	public Toggle ToggleEffects;

	[Header("Чувствительность:")]
	public Slider SliderSens;

	public Text TextSens;

	public float _countSens;

	[Header("Чувствительность с прицелом:")]
	public Slider SliderSensAim;

	public Text TextSensAim;

	public float _countAim;

	private void Start()
	{
		LoadSettings();
	}

	public void LoadSettings()
	{
		if (PlayerPrefs.GetInt("TextureYes") == 0)
		{
			PlayerPrefs.SetInt("TextureYes", 1);
			PlayerPrefs.SetInt("Texture", 1);
		}
		if (PlayerPrefs.GetInt("ModelsYes") == 0)
		{
			PlayerPrefs.SetInt("ModelsYes", 1);
			PlayerPrefs.SetInt("Models", 2);
		}
		if (PlayerPrefs.GetInt("ShadowsYes") == 0)
		{
			PlayerPrefs.SetInt("ShadowsYes", 1);
			PlayerPrefs.SetInt("Shadows", 1);
		}
		if (PlayerPrefs.GetInt("EffectsYes") == 0)
		{
			PlayerPrefs.SetInt("EffectsYes", 1);
			PlayerPrefs.SetInt("Effects", 1);
		}
		if (PlayerPrefs.GetInt("SensYes") == 0 || PlayerPrefs.GetInt("SensAimYes") == 0)
		{
			PlayerPrefs.SetInt("SensYes", 1);
			PlayerPrefs.SetFloat("Sens", _countSens);
		}
		if (PlayerPrefs.GetInt("SensAimYes") == 0)
		{
			PlayerPrefs.SetInt("SensAimYes", 1);
			PlayerPrefs.SetFloat("SensAim", _countAim);
		}
		SetHands1(PlayerPrefs.GetInt("Hands1"));
		SetHands2(PlayerPrefs.GetInt("Hands2"));
		SetPost(PlayerPrefs.GetInt("Post"));
		SetTexture(PlayerPrefs.GetInt("Texture"));
		SetModels(PlayerPrefs.GetInt("Models"));
		SetShadows(PlayerPrefs.GetInt("Shadows"));
		SetAliasing(PlayerPrefs.GetInt("Aliasing"));
		SetAnis(PlayerPrefs.GetInt("Anis"));
		SetEffects(PlayerPrefs.GetInt("Effects"));
		LoadSens(PlayerPrefs.GetFloat("Sens"));
		LoadSensAim(PlayerPrefs.GetFloat("SensAim"));
	}

	public void SetHands1(int _type)
	{
		for (int i = 0; i < ButtonsHands1.Length; i++)
		{
			ButtonsHands1[i].color = DisableButtonColor;
		}
		ButtonsHands1[_type].color = EnableButtonColor;
		PlayerPrefs.SetInt("Hands1", _type);
	}

	public void SetHands2(int _type)
	{
		for (int i = 0; i < ButtonsHands2.Length; i++)
		{
			ButtonsHands2[i].color = DisableButtonColor;
		}
		ButtonsHands2[_type].color = EnableButtonColor;
		PlayerPrefs.SetInt("Hands2", _type);
	}

	public void SetPost(int _type)
	{
		_isChangePost = true;
		for (int i = 0; i < ButtonsPost.Length; i++)
		{
			ButtonsPost[i].color = DisableButtonColor;
		}
		switch (_type)
		{
		case 0:
			ToogleOcclusion.isOn = false;
			PlayerPrefs.SetInt("Occlusion", 0);
			ToogleGrading.isOn = false;
			PlayerPrefs.SetInt("Grading", 0);
			ToogleBloom.isOn = false;
			PlayerPrefs.SetInt("Bloom", 0);
			ToogleBlur.isOn = false;
			PlayerPrefs.SetInt("Blur", 0);
			ToogleVignette.isOn = false;
			PlayerPrefs.SetInt("Vignette", 0);
			break;
		case 1:
			ToogleOcclusion.isOn = true;
			PlayerPrefs.SetInt("Occlusion", 1);
			ToogleGrading.isOn = true;
			PlayerPrefs.SetInt("Grading", 1);
			ToogleBloom.isOn = false;
			PlayerPrefs.SetInt("Bloom", 0);
			ToogleBlur.isOn = false;
			PlayerPrefs.SetInt("Blur", 0);
			ToogleVignette.isOn = false;
			PlayerPrefs.SetInt("Vignette", 0);
			break;
		case 2:
			ToogleOcclusion.isOn = true;
			PlayerPrefs.SetInt("Occlusion", 1);
			ToogleGrading.isOn = true;
			PlayerPrefs.SetInt("Grading", 1);
			ToogleBloom.isOn = true;
			PlayerPrefs.SetInt("Bloom", 1);
			ToogleBlur.isOn = true;
			PlayerPrefs.SetInt("Blur", 1);
			ToogleVignette.isOn = true;
			PlayerPrefs.SetInt("Vignette", 1);
			break;
		case 3:
			switch (PlayerPrefs.GetInt("Occlusion"))
			{
			case 0:
				ToogleOcclusion.isOn = false;
				break;
			case 1:
				ToogleOcclusion.isOn = true;
				break;
			}
			switch (PlayerPrefs.GetInt("Grading"))
			{
			case 0:
				ToogleGrading.isOn = false;
				break;
			case 1:
				ToogleGrading.isOn = true;
				break;
			}
			switch (PlayerPrefs.GetInt("Bloom"))
			{
			case 0:
				ToogleBloom.isOn = false;
				break;
			case 1:
				ToogleBloom.isOn = true;
				break;
			}
			switch (PlayerPrefs.GetInt("Blur"))
			{
			case 0:
				ToogleBlur.isOn = false;
				break;
			case 1:
				ToogleBlur.isOn = true;
				break;
			}
			switch (PlayerPrefs.GetInt("Vignette"))
			{
			case 0:
				ToogleVignette.isOn = false;
				break;
			case 1:
				ToogleVignette.isOn = true;
				break;
			}
			break;
		}
		ButtonsPost[_type].color = EnableButtonColor;
		PlayerPrefs.SetInt("Post", _type);
		_isChangePost = false;
	}

	public void SetPostToggle(int _type)
	{
		if (_isChangePost)
		{
			return;
		}
		switch (_type)
		{
		case 1:
			switch (ToogleOcclusion.isOn)
			{
			case false:
				PlayerPrefs.SetInt("Occlusion", 0);
				break;
			case true:
				PlayerPrefs.SetInt("Occlusion", 1);
				break;
			}
			break;
		case 2:
			switch (ToogleGrading.isOn)
			{
			case false:
				PlayerPrefs.SetInt("Grading", 0);
				break;
			case true:
				PlayerPrefs.SetInt("Grading", 1);
				break;
			}
			break;
		case 3:
			switch (ToogleBloom.isOn)
			{
			case false:
				PlayerPrefs.SetInt("Bloom", 0);
				break;
			case true:
				PlayerPrefs.SetInt("Bloom", 1);
				break;
			}
			break;
		case 4:
			switch (ToogleBlur.isOn)
			{
			case false:
				PlayerPrefs.SetInt("Blur", 0);
				break;
			case true:
				PlayerPrefs.SetInt("Blur", 1);
				break;
			}
			break;
		case 5:
			switch (ToogleVignette.isOn)
			{
			case false:
				PlayerPrefs.SetInt("Vignette", 0);
				break;
			case true:
				PlayerPrefs.SetInt("Vignette", 1);
				break;
			}
			break;
		}
		SetPost(3);
	}

	public void SetTexture(int _type)
	{
		for (int i = 0; i < ButtonsTexture.Length; i++)
		{
			ButtonsTexture[i].color = DisableButtonColor;
		}
		ButtonsTexture[_type].color = EnableButtonColor;
		QualitySettings.masterTextureLimit = _type;
		PlayerPrefs.SetInt("Texture", _type);
	}

	public void SetModels(int _type)
	{
		for (int i = 0; i < ButtonsModels.Length; i++)
		{
			ButtonsModels[i].color = DisableButtonColor;
		}
		ButtonsModels[_type].color = EnableButtonColor;
		switch (_type)
		{
		case 0:
			QualitySettings.skinWeights = SkinWeights.OneBone;
			break;
		case 1:
			QualitySettings.skinWeights = SkinWeights.TwoBones;
			break;
		case 2:
			QualitySettings.skinWeights = SkinWeights.FourBones;
			break;
		case 3:
			QualitySettings.skinWeights = SkinWeights.Unlimited;
			break;
		}
		PlayerPrefs.SetInt("Models", _type);
	}

	public void SetShadows(int _type)
	{
		for (int i = 0; i < ButtonsShadows.Length; i++)
		{
			ButtonsShadows[i].color = DisableButtonColor;
		}
		ButtonsShadows[_type].color = EnableButtonColor;
		switch (_type)
		{
		case 0:
			QualitySettings.shadows = ShadowQuality.Disable;
			QualitySettings.pixelLightCount = 0;
			break;
		case 1:
			QualitySettings.shadows = ShadowQuality.HardOnly;
			QualitySettings.pixelLightCount = 2;
			break;
		case 2:
			QualitySettings.shadows = ShadowQuality.All;
			QualitySettings.pixelLightCount = 4;
			break;
		}
		PlayerPrefs.SetInt("Shadows", _type);
	}

	public void SetAliasing(int _type)
	{
		_countAliasing = _type;
		CountTextAliasing.text = _strAliasing[_countAliasing];
		switch (_countAliasing)
		{
		case 0:
			QualitySettings.antiAliasing = 0;
			break;
		case 1:
			QualitySettings.antiAliasing = 2;
			break;
		case 2:
			QualitySettings.antiAliasing = 4;
			break;
		case 3:
			QualitySettings.antiAliasing = 8;
			break;
		}
		PlayerPrefs.SetInt("Aliasing", _countAliasing);
	}

	public void AddAliasing(int _addCount)
	{
		if (_countAliasing + _addCount > _strAliasing.Length - 1)
		{
			_countAliasing = 0;
		}
		else if (_countAliasing + _addCount < 0)
		{
			_countAliasing = _strAliasing.Length - 1;
		}
		else
		{
			_countAliasing += _addCount;
		}
		SetAliasing(_countAliasing);
	}

	public void SetAnis(int _type)
	{
		_countAnis = _type;
		CountTextAnis.text = _strAnis[_countAnis];
		switch (_countAnis)
		{
		case 0:
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
			break;
		case 1:
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
			break;
		case 2:
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
			break;
		}
		PlayerPrefs.SetInt("Anis", _countAnis);
	}

	public void AddAnis(int _addCount)
	{
		if (_countAnis + _addCount > _strAnis.Length - 1)
		{
			_countAnis = 0;
		}
		else if (_countAnis + _addCount < 0)
		{
			_countAnis = _strAnis.Length - 1;
		}
		else
		{
			_countAnis += _addCount;
		}
		SetAnis(_countAnis);
	}

	public void SetEffects(int _type)
	{
		switch (_type)
		{
		case 0:
			ToggleEffects.isOn = false;
			break;
		case 1:
			ToggleEffects.isOn = true;
			break;
		}
	}

	public void SetToggleEffects()
	{
		switch (ToggleEffects.isOn)
		{
		case false:
			PlayerPrefs.SetInt("Effects", 0);
			break;
		case true:
			PlayerPrefs.SetInt("Effects", 1);
			break;
		}
		Controller._obj.ReloadEffects();
	}

	public void SetSensSlider()
	{
		TextSens.text = SliderSens.value.ToString();
		PlayerPrefs.SetFloat("Sens", SliderSens.value);
	}

	public void LoadSens(float _type)
	{
		SliderSens.value = _type;
		TextSens.text = _type.ToString();
	}

	public void SetSensAimSlider()
	{
		TextSensAim.text = SliderSensAim.value.ToString();
		PlayerPrefs.SetFloat("SensAim", SliderSensAim.value);
	}

	public void LoadSensAim(float _type)
	{
		SliderSensAim.value = _type;
		TextSensAim.text = _type.ToString();
	}
}
