using System;
using System.Collections;
using FMODUnity;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.Utility;

public class PlayerController : MonoBehaviour
{
	[Serializable]
	private class Weapon
	{
		public enum Type
		{
			Knife = 0,
			Pistol = 1,
			Rifle = 2,
			Sniper = 3
		}

		public string Name = "Knife";

		public Type type;

		public GameObject[] OldModels;

		public Renderer[] Models;

		public Material[] Skins;

		public AudioClip ShootAudio;

		[Range(1f, 10f)]
		public int _patrOnAttack;

		[Range(1f, 300f)]
		public int _strongAttack;

		[Range(0f, 20f)]
		public float _recoilX;

		[Range(0f, 20f)]
		public float _recoilY;

		[Range(0f, 0.5f)]
		public float _razbros;

		[Range(10f, 30f)]
		public int FOVSniper;

		public int _damageHit;

		public float _zadAttack;

		public float _zadTake;

		public int _idWeaponUI;

		public string Anim_hit1 = "Knife_hit1";

		public string Anim_hit2 = "Knife_hit2";

		public string Anim_hit3 = "";

		public string Anim_inspect = "Knife_inspect";

		public string Anim_take = "Knife_take";

		public string Anim_None = "Knife_None";
	}

	public GameObject Torse;

	public Camera Camera;

	public Camera CameraWeapon;

	public Camera CameraMinimap;

	public Animation Knifes;

	public Animator Hands;

	public Animator MainHands;

	public GameObject _ballonPlayer;

	public GameObject _ballonOther;

	public GameObject[] _modelsPlayer;

	public PostProcessProfile Profile;

	public float m_WalkSpeed = 5f;

	private float m_newWalkSpeed;

	private float m_oldWalkSpeed;

	public float m_AccelSpeed = 0.25f;

	public float m_JumpSpeed = 10f;

	public float m_Sensitivity = 2f;

	public float m_SensitivityAim = 1f;

	public bool m_controllPlayer = true;

	public bool m_loadSave = true;

	public bool m_animTest;

	public bool m_mobileTest;

	[Header("Оружие:")]
	public int TypeWeapon = 16;

	public int TypeSkin;

	public int TypeWeapon3;

	public int TypeSkin3;

	[SerializeField]
	private Weapon[] _weapons;

	public Color DefaultWeapon;

	public Color DonateWeapon;

	[Header("Руки и перчатки:")]
	public int TypeGloves;

	public int TypeHands;

	public GameObject[] _gloves;

	public GameObject[] _hands;

	public int[] _GlovesMapDefault;

	public int[] _HandsMapDefault;

	[Header("Доп:")]
	public float m_StickToGroundForce = 10f;

	public float m_GravityMultiplier = 2.5f;

	public bool m_UseHeadBob = true;

	public CurveControlledBob m_HeadBob = new CurveControlledBob();

	public LerpControlledBob m_JumpBob = new LerpControlledBob();

	public float m_StepInterval = 5f;

	public bool clampVerticalRotation = true;

	public float MinimumX = -90f;

	public float MaximumX = 90f;

	public float smoothTimePC = 12.5f;

	public float smoothTimeAndroid = 20f;

	public static bool m_JumpWhile;

	public static float OffsetX;

	public static float OffsetY;

	[HideInInspector]
	public float oldWalkSpeed;

	private float _sensitivity;

	private bool m_Falling;

	private bool m_JumpPeremes;

	private bool m_Jumping;

	private bool m_Walking;

	private bool m_IsShift;

	private float m_YRotation;

	private Vector2 m_Input;

	private Vector3 m_MoveDir = Vector3.zero;

	private GameObject Pricele;

	private CharacterController m_CharacterController;

	private CollisionFlags m_CollisionFlags;

	private bool m_PreviouslyGrounded;

	private Vector3 m_OriginalCameraPosition;

	private float m_StepCycle;

	private float m_NextStep;

	private bool zadSoundLadder;

	private bool JumpSoundEnabled = true;

	private float timer;

	private Quaternion m_CharacterTargetRot;

	private Quaternion m_CameraTargetRot;

	private RaycastHit hitInfo;

	private TouchPanel touchField;

	private Joystick joystick;

	private GameObject Cursore;

	private GameObject ZonaAttack;

	private GameObject ZonaShoot;

	private GameObject ButtonAttack;

	private GameObject ButtonShoot;

	private GameObject ButtonAim;

	private AudioSource _audioSource;

	private PhotonView _view;

	private int _typeWeapon;

	private int _typeWeapon3;

	private int _typeSkin;

	private int _typeSkin3;

	private int _typeTake;

	private int _typeTake3;

	private bool _pricel;

	private bool _zad;

	private float rand1;

	private float rand2;

	private void Start()
	{
		_view = base.transform.parent.GetComponent<PhotonView>();
		if (!_view.IsMine & ServerManager._inRoom)
		{
			Camera.gameObject.SetActive(value: false);
			CameraWeapon.gameObject.SetActive(value: false);
			CameraMinimap.gameObject.SetActive(value: false);
			_ballonPlayer.SetActive(value: false);
			_ballonOther.SetActive(value: true);
			for (int i = 0; i < _modelsPlayer.Length; i++)
			{
				_modelsPlayer[i].SetActive(value: true);
			}
			GetComponent<PlayerController>().enabled = false;
		}
		else
		{
			if (PlayerPrefsSafe.GetInt("TypeWeapon") < 17)
			{
				PlayerPrefsSafe.SetInt("TypeWeapon", TypeWeapon);
			}
			if (m_loadSave)
			{
				TypeWeapon = PlayerPrefsSafe.GetInt("TypeWeapon");
				TypeSkin = PlayerPrefsSafe.GetInt("TypeSkin");
				TypeWeapon3 = PlayerPrefsSafe.GetInt("TypeWeapon3");
				TypeSkin3 = PlayerPrefsSafe.GetInt("TypeSkin3");
				if (PlayerPrefsSafe.GetInt("TypeGloves") != 0)
				{
					TypeGloves = PlayerPrefsSafe.GetInt("TypeGloves");
				}
				else
				{
					TypeGloves = _GlovesMapDefault[ControllerGameServer.sceneIndex - 3];
				}
				if (PlayerPrefsSafe.GetInt("TypeHands") != 0)
				{
					TypeHands = PlayerPrefsSafe.GetInt("TypeHands");
				}
				else
				{
					TypeHands = _HandsMapDefault[ControllerGameServer.sceneIndex - 3];
				}
			}
		}
		if (((Application.platform != RuntimePlatform.WindowsPlayer) & (Application.platform != RuntimePlatform.WindowsEditor)) || m_mobileTest)
		{
			touchField = ControllerGameServer._obj.Canvas.touchField;
			joystick = ControllerGameServer._obj.Canvas.joystick;
			ZonaAttack = ControllerGameServer._obj.Canvas.ZonaAttack;
			ZonaShoot = ControllerGameServer._obj.Canvas.ZonaShoot;
			ButtonAttack = ControllerGameServer._obj.Canvas.ButtonAttack;
			ButtonShoot = ControllerGameServer._obj.Canvas.ButtonShoot;
		}
		else
		{
			ControllerGameServer._obj.Canvas.UIMobile.SetActive(value: false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		Pricele = ControllerGameServer._obj.Canvas.Pricele;
		Cursore = ControllerGameServer._obj.Canvas.Cursore;
		ButtonAim = ControllerGameServer._obj.Canvas.ButtonAim;
		_typeTake = 1;
		_typeTake3 = 3;
		Take(3);
		switch (PlayerPrefsSafe.GetInt("Hands1"))
		{
		case 0:
			MainHands.transform.localPosition = new Vector3(0f, 0f, 0f);
			break;
		case 1:
			MainHands.transform.localPosition = new Vector3(0f, 0.025f, -0.075f);
			break;
		}
		switch (PlayerPrefsSafe.GetInt("Hands2"))
		{
		case 0:
			MainHands.transform.localScale = new Vector3(1f, 1f, 1f);
			break;
		case 1:
			MainHands.transform.localScale = new Vector3(-1f, 1f, 1f);
			break;
		}
		if (PlayerPrefs.GetInt("Post") != 0)
		{
			CameraWeapon.GetComponent<PostProcessLayer>().enabled = true;
			CameraWeapon.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.None;
			Profile.TryGetSettings<AmbientOcclusion>(out var outSetting);
			switch (PlayerPrefs.GetInt("Occlusion"))
			{
			case 0:
				outSetting.enabled.value = false;
				break;
			case 1:
				outSetting.enabled.value = true;
				break;
			}
			Profile.TryGetSettings<ColorGrading>(out var outSetting2);
			switch (PlayerPrefs.GetInt("Grading"))
			{
			case 0:
				outSetting2.enabled.value = false;
				break;
			case 1:
				outSetting2.enabled.value = true;
				break;
			}
			Profile.TryGetSettings<Bloom>(out var outSetting3);
			switch (PlayerPrefs.GetInt("Bloom"))
			{
			case 0:
				outSetting3.enabled.value = false;
				break;
			case 1:
				outSetting3.enabled.value = true;
				break;
			}
			Profile.TryGetSettings<MotionBlur>(out var outSetting4);
			switch (PlayerPrefs.GetInt("Blur"))
			{
			case 0:
				outSetting4.enabled.value = false;
				break;
			case 1:
				outSetting4.enabled.value = true;
				break;
			}
			Profile.TryGetSettings<Vignette>(out var outSetting5);
			switch (PlayerPrefs.GetInt("Vignette"))
			{
			case 0:
				outSetting5.enabled.value = false;
				break;
			case 1:
				outSetting5.enabled.value = true;
				break;
			}
			CameraWeapon.GetComponent<PostProcessVolume>().enabled = true;
		}
		m_Sensitivity = PlayerPrefs.GetFloat("Sens");
		m_SensitivityAim = PlayerPrefs.GetFloat("SensAim");
		_sensitivity = m_Sensitivity;
		_audioSource = GetComponent<AudioSource>();
		m_CharacterController = base.transform.parent.GetComponent<CharacterController>();
		m_OriginalCameraPosition = Camera.transform.localPosition;
		m_HeadBob.Setup(Camera, m_StepInterval);
		m_StepCycle = 0f;
		m_NextStep = m_StepCycle / 2f;
		m_Jumping = false;
		m_JumpWhile = false;
		m_Falling = false;
		oldWalkSpeed = m_WalkSpeed;
		m_oldWalkSpeed = m_WalkSpeed;
		m_newWalkSpeed = m_WalkSpeed;
		Init(base.transform, Torse.transform);
	}

	private void Update()
	{
		if (m_controllPlayer)
		{
			LookRotation();
			if (Input.GetKeyDown(KeyCode.Space))
			{
				m_JumpWhile = true;
			}
			else if (Input.GetKeyUp(KeyCode.Space))
			{
				m_JumpWhile = false;
			}
			if (m_Walking)
			{
				MainHands.SetBool("Step", value: true);
			}
			else
			{
				MainHands.SetBool("Step", value: false);
			}
		}
		else
		{
			m_JumpWhile = false;
		}
		if ((m_controllPlayer || m_animTest) && ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) & !m_mobileTest))
		{
			if (Input.GetButton("Fire1") & (_weapons[_typeWeapon].type == Weapon.Type.Knife))
			{
				StartCoroutine(Attack(1));
			}
			else if (Input.GetButton("Fire2") & (_weapons[_typeWeapon].type == Weapon.Type.Knife))
			{
				StartCoroutine(Attack(2));
			}
			if (Input.GetButton("Fire1") & (_weapons[_typeWeapon].type != Weapon.Type.Knife))
			{
				StartCoroutine(Shoot());
			}
			else if (Input.GetButtonDown("Fire2") & (_weapons[_typeWeapon].type == Weapon.Type.Sniper))
			{
				Pricel();
			}
			else if (Input.GetKeyDown(KeyCode.F))
			{
				StartCoroutine(Inspect());
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				StopAllCoroutines();
				StartCoroutine(Podbor(1));
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				StopAllCoroutines();
				StartCoroutine(Podbor(1));
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				StopAllCoroutines();
				StartCoroutine(Podbor(3));
			}
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				ControllerGameServer._obj.Canvas.MiniMapObject.SetActive(value: false);
				ControllerGameServer._obj.Canvas.BigMapObject.SetActive(value: true);
			}
			else if (Input.GetKeyUp(KeyCode.Tab))
			{
				ControllerGameServer._obj.Canvas.MiniMapObject.SetActive(value: true);
				ControllerGameServer._obj.Canvas.BigMapObject.SetActive(value: false);
			}
		}
		if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) & !m_mobileTest & Input.GetKeyDown(KeyCode.Escape))
		{
			ControllerGameServer._obj.Canvas.MiniMapObject.SetActive(value: true);
			ControllerGameServer._obj.Canvas.BigMapObject.SetActive(value: false);
			ControllerGameServer._obj.Canvas.ButtonPause();
		}
	}

	private void FixedUpdate()
	{
		GetInput();
		if (!m_CharacterController.isGrounded & !m_Falling)
		{
			m_Falling = true;
			MainHands.SetBool("Falling", value: true);
			MainHands.SetTrigger("Jump");
			MainHands.SetBool("Jumping", value: true);
			RuntimeManager.PlayOneShot("event:/Footsteps/character/character_jump");
		}
		else if (m_CharacterController.isGrounded & m_Falling)
		{
			m_Falling = false;
			MainHands.SetBool("Falling", value: false);
		}
		if (!m_PreviouslyGrounded && (m_CharacterController.isGrounded & JumpSoundEnabled))
		{
			if (m_AccelSpeed != 0f)
			{
				if (m_JumpWhile & (m_MoveDir.x != 0f) & !m_IsShift)
				{
					float num = UnityEngine.Random.Range(m_AccelSpeed - 0.05f, m_AccelSpeed + 0.05f);
					if (m_newWalkSpeed + num > 15f)
					{
						m_newWalkSpeed = 15f;
					}
					else
					{
						m_newWalkSpeed += num;
					}
				}
				else
				{
					m_newWalkSpeed = m_oldWalkSpeed;
				}
			}
			StartCoroutine(m_JumpBob.DoBobCycle());
			PlayLandingSound();
			if (!m_JumpWhile)
			{
				MainHands.SetTrigger("Jump");
			}
			MainHands.SetBool("Jumping", value: false);
			m_MoveDir = new Vector3(0f, 0f, 0f);
			m_Jumping = false;
		}
		if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
		{
			m_MoveDir.y = 0f;
		}
		m_PreviouslyGrounded = m_CharacterController.isGrounded;
		Vector3 vector = base.transform.forward * m_Input.y + base.transform.right * m_Input.x;
		Physics.SphereCast(base.transform.position, m_CharacterController.radius, Vector3.down, out hitInfo, m_CharacterController.height / 2f, -1, QueryTriggerInteraction.Ignore);
		vector = Vector3.ProjectOnPlane(vector, hitInfo.normal).normalized;
		m_MoveDir.x = vector.x * m_WalkSpeed;
		m_MoveDir.z = vector.z * m_WalkSpeed;
		if (m_MoveDir.x != 0f)
		{
			m_Walking = true;
		}
		else
		{
			m_Walking = false;
		}
		if (m_CharacterController.isGrounded)
		{
			m_MoveDir.y = 0f - m_StickToGroundForce;
			if (m_JumpWhile & (m_JumpSpeed != 0f))
			{
				m_MoveDir.y = m_JumpSpeed;
				m_Jumping = true;
			}
		}
		else
		{
			m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
		}
		m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
		ProgressStepCycle(m_WalkSpeed);
		UpdateCameraPosition(m_WalkSpeed);
	}

	private void PlayLandingSound()
	{
		RuntimeManager.PlayOneShot("event:/Footsteps/character/charater_land");
		m_NextStep = m_StepCycle + 0.5f;
	}

	private void ProgressStepCycle(float speed)
	{
		if (m_CharacterController.velocity.sqrMagnitude > 0f && (m_Input.x != 0f || m_Input.y != 0f))
		{
			m_StepCycle += (m_CharacterController.velocity.magnitude + speed) * Time.fixedDeltaTime;
		}
		if (m_StepCycle > m_NextStep)
		{
			m_NextStep = m_StepCycle + m_StepInterval;
			PlayFootStepAudio();
		}
	}

	private void PlayFootStepAudio()
	{
		if (m_CharacterController.isGrounded & (m_WalkSpeed > 2.5f))
		{
			RuntimeManager.PlayOneShot("event:/Footsteps/character/character_footsteps");
		}
	}

	private void UpdateCameraPosition(float speed)
	{
		if (m_UseHeadBob)
		{
			Vector3 localPosition;
			if (m_CharacterController.velocity.magnitude > 0f && m_CharacterController.isGrounded)
			{
				Camera.transform.localPosition = m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude + speed);
				localPosition = Camera.transform.localPosition;
				localPosition.y = Camera.transform.localPosition.y - m_JumpBob.Offset();
			}
			else
			{
				localPosition = Camera.transform.localPosition;
				localPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
			}
			Camera.transform.localPosition = localPosition;
		}
	}

	private void GetInput()
	{
		if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) & !m_mobileTest)
		{
			float num = Input.GetAxis("Horizontal");
			float num2 = Input.GetAxis("Vertical");
			m_IsShift = Input.GetKey(KeyCode.LeftShift);
			if (m_controllPlayer)
			{
				m_Input = new Vector2(num, num2);
			}
			else
			{
				m_Input = new Vector2(0f, 0f);
			}
			if (num < 0f)
			{
				num = 0f - num;
			}
			if (num2 < 0f)
			{
				num2 = 0f - num2;
			}
			if (!m_IsShift)
			{
				if (num > num2)
				{
					m_WalkSpeed = m_newWalkSpeed * num;
				}
				else
				{
					m_WalkSpeed = m_newWalkSpeed * num2;
				}
			}
			else if (num > num2)
			{
				m_WalkSpeed = m_oldWalkSpeed * num / 2f;
			}
			else
			{
				m_WalkSpeed = m_oldWalkSpeed * num2 / 2f;
			}
			MainHands.SetFloat("Speed", m_WalkSpeed);
		}
		else
		{
			float num3 = joystick.Horizontal;
			float num4 = joystick.Vertical;
			if (num3 > 0.2f || num3 < -0.2f || num4 > 0.2f || num4 < -0.2f)
			{
				if (m_controllPlayer)
				{
					m_Input = new Vector2(num3, num4);
				}
				else
				{
					m_Input = new Vector2(0f, 0f);
				}
				if (num3 < 0f)
				{
					num3 = 0f - num3;
				}
				if (num4 < 0f)
				{
					num4 = 0f - num4;
				}
				if (num3 > num4)
				{
					m_WalkSpeed = m_newWalkSpeed * num3;
				}
				else
				{
					m_WalkSpeed = m_newWalkSpeed * num4;
				}
				MainHands.SetFloat("Speed", m_WalkSpeed);
			}
			else
			{
				m_Input = new Vector2(0f, 0f);
			}
		}
		if (m_Input.sqrMagnitude > 1f)
		{
			m_Input.Normalize();
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (m_CollisionFlags != CollisionFlags.Below && !(attachedRigidbody == null) && !attachedRigidbody.isKinematic)
		{
			attachedRigidbody.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
		}
	}

	public void Init(Transform character, Transform camera)
	{
		m_CharacterTargetRot = character.localRotation;
		m_CameraTargetRot = camera.localRotation;
		OffsetY = 0.4f;
	}

	public void ChangeOffset(float x, float y)
	{
		OffsetX = x;
		OffsetY = y;
	}

	public void RotateOffset(Quaternion quat)
	{
		quat.z = 0f;
		quat.x = 0f;
		m_CharacterTargetRot *= quat;
	}

	public void LookRotation()
	{
		float y = 0f;
		float num = 0f;
		if (m_controllPlayer)
		{
			if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) & !m_mobileTest)
			{
				y = Input.GetAxis("Mouse X") * _sensitivity + OffsetY;
				num = Input.GetAxis("Mouse Y") * _sensitivity + OffsetX;
			}
			else
			{
				y = touchField.TouchDist.x / 20f * _sensitivity + OffsetY;
				num = touchField.TouchDist.y / 20f * _sensitivity + OffsetX;
			}
		}
		OffsetY = 0f;
		OffsetX = 0f;
		m_CharacterTargetRot *= Quaternion.Euler(0f, y, 0f);
		m_CameraTargetRot *= Quaternion.Euler(0f - num, 0f, 0f);
		if (clampVerticalRotation)
		{
			m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
		}
		if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) & !m_mobileTest)
		{
			base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, m_CharacterTargetRot, smoothTimePC * Time.fixedDeltaTime);
			Torse.transform.localRotation = Quaternion.Slerp(Torse.transform.localRotation, m_CameraTargetRot, smoothTimePC * Time.fixedDeltaTime);
		}
		else
		{
			base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, m_CharacterTargetRot, smoothTimeAndroid * Time.fixedDeltaTime);
			Torse.transform.localRotation = Quaternion.Slerp(Torse.transform.localRotation, m_CameraTargetRot, smoothTimeAndroid * Time.fixedDeltaTime);
		}
	}

	private Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;
		float value = 114.59156f * Mathf.Atan(q.x);
		value = Mathf.Clamp(value, MinimumX, MaximumX);
		q.x = Mathf.Tan((float)Math.PI / 360f * value);
		return q;
	}

	private IEnumerator Podbor(int TypeTake)
	{
		_zad = false;
		if (_pricel)
		{
			Pricel();
		}
		_zad = true;
		_typeTake = TypeTake;
		switch (_typeTake)
		{
		case 1:
			_typeWeapon = TypeWeapon;
			_typeWeapon3 = TypeWeapon3;
			_typeSkin = TypeSkin;
			_typeSkin3 = TypeSkin3;
			_typeTake3 = 3;
			break;
		case 3:
			_typeWeapon = TypeWeapon3;
			_typeWeapon3 = TypeWeapon;
			_typeSkin = TypeSkin3;
			_typeSkin3 = TypeSkin;
			_typeTake3 = 1;
			break;
		}
		if (((Application.platform != RuntimePlatform.WindowsPlayer) & (Application.platform != RuntimePlatform.WindowsEditor)) || m_mobileTest)
		{
			ZonaAttack.transform.localScale = new Vector3(0f, 0f, 0f);
			ZonaShoot.transform.localScale = new Vector3(0f, 0f, 0f);
			ButtonAttack.transform.localScale = new Vector3(0f, 0f, 0f);
			ButtonShoot.transform.localScale = new Vector3(0f, 0f, 0f);
			ButtonAim.transform.localScale = new Vector3(0f, 0f, 0f);
			if (_weapons[_typeWeapon].type == Weapon.Type.Knife)
			{
				ButtonAttack.transform.localScale = new Vector3(1f, 1f, 1f);
				ZonaAttack.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else if (_weapons[_typeWeapon].type == Weapon.Type.Sniper)
			{
				ButtonAim.transform.localScale = new Vector3(1f, 1f, 1f);
				ButtonShoot.transform.localScale = new Vector3(1f, 1f, 1f);
				ZonaShoot.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				ButtonShoot.transform.localScale = new Vector3(1f, 1f, 1f);
				ZonaShoot.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		if (_weapons[_typeWeapon].type != Weapon.Type.Sniper)
		{
			Cursore.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			Cursore.transform.localScale = new Vector3(0f, 0f, 0f);
		}
		ControllerGameServer._obj.Canvas.NoneUI.gameObject.SetActive(value: false);
		ControllerGameServer._obj.Canvas.NoneUI3.gameObject.SetActive(value: false);
		ControllerGameServer._obj.Canvas.WeaponUI.gameObject.SetActive(value: false);
		ControllerGameServer._obj.Canvas.WeaponUI3.gameObject.SetActive(value: false);
		ControllerGameServer._obj.Canvas.WeaponUI.color = DefaultWeapon;
		ControllerGameServer._obj.Canvas.WeaponUI3.color = DefaultWeapon;
		if (_weapons[_typeWeapon]._idWeaponUI != 0)
		{
			ControllerGameServer._obj.Canvas.WeaponUI.sprite = ControllerGameServer._obj.Canvas.WeaponImage[_weapons[_typeWeapon]._idWeaponUI - 1];
			ControllerGameServer._obj.Canvas.WeaponUI.gameObject.SetActive(value: true);
			if (_typeSkin != 0)
			{
				ControllerGameServer._obj.Canvas.WeaponUI.color = DonateWeapon;
			}
		}
		else
		{
			ControllerGameServer._obj.Canvas.NoneUI.gameObject.SetActive(value: true);
		}
		if (_weapons[_typeWeapon3]._idWeaponUI != 0)
		{
			ControllerGameServer._obj.Canvas.WeaponUI3.sprite = ControllerGameServer._obj.Canvas.WeaponImage[_weapons[_typeWeapon3]._idWeaponUI - 1];
			ControllerGameServer._obj.Canvas.WeaponUI3.gameObject.SetActive(value: true);
			if (_typeSkin3 != 0)
			{
				ControllerGameServer._obj.Canvas.WeaponUI3.color = DonateWeapon;
			}
		}
		else
		{
			ControllerGameServer._obj.Canvas.NoneUI3.gameObject.SetActive(value: true);
		}
		if (_weapons[_typeWeapon].Skins.Length > _typeSkin)
		{
			if (_weapons[_typeWeapon].OldModels != null)
			{
				for (int i = 0; i < _weapons[_typeWeapon].OldModels.Length; i++)
				{
					_weapons[_typeWeapon].OldModels[i].SetActive(value: false);
				}
			}
			for (int j = 0; j < _weapons[_typeWeapon].Models.Length; j++)
			{
				_weapons[_typeWeapon].Models[j].sharedMaterial = _weapons[_typeWeapon].Skins[_typeSkin];
				_weapons[_typeWeapon].Models[j].gameObject.SetActive(value: true);
			}
		}
		if (_gloves.Length > TypeGloves)
		{
			for (int k = 0; k < _gloves.Length; k++)
			{
				_gloves[k].SetActive(value: false);
			}
			_gloves[TypeGloves].SetActive(value: true);
		}
		if (_hands.Length > TypeHands)
		{
			for (int l = 0; l < _hands.Length; l++)
			{
				_hands[l].SetActive(value: false);
			}
			_hands[TypeHands].SetActive(value: true);
		}
		rand1 = 0f;
		rand2 = 0f;
		Hands.Play("None");
		yield return new WaitForSeconds(0.0001f);
		Hands.Play(_weapons[_typeWeapon].Anim_take);
		if (!_view.IsMine & PhotonNetwork.InRoom)
		{
			yield return new WaitForSeconds(0.001f);
			Renderer[] componentsInChildren = MainHands.GetComponentsInChildren<Renderer>();
			for (int m = 0; m < componentsInChildren.Length; m++)
			{
				componentsInChildren[m].gameObject.layer = 0;
			}
		}
		yield return new WaitForSeconds(_weapons[_typeWeapon]._zadTake);
		_zad = false;
	}

	public IEnumerator AttackWhile()
	{
		while (Controller._attackWhile)
		{
			StartCoroutine(Attack(3));
			yield return new WaitForSeconds(0.01f);
		}
	}

	public IEnumerator Attack(int _typeAttack)
	{
		if (!(!_zad & (Time.timeScale != 0f)))
		{
			yield break;
		}
		_zad = true;
		Hands.Play("None");
		yield return new WaitForSeconds(0.0001f);
		switch (_typeAttack)
		{
		case 1:
			if (_weapons[_typeWeapon].Anim_hit3 != "")
			{
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					Hands.Play(_weapons[_typeWeapon].Anim_hit1);
				}
				else
				{
					Hands.Play(_weapons[_typeWeapon].Anim_hit2);
				}
			}
			else
			{
				Hands.Play(_weapons[_typeWeapon].Anim_hit1);
			}
			break;
		case 2:
			if (_weapons[_typeWeapon].Anim_hit3 != "")
			{
				Hands.Play(_weapons[_typeWeapon].Anim_hit3);
			}
			else
			{
				Hands.Play(_weapons[_typeWeapon].Anim_hit2);
			}
			break;
		default:
			if (_weapons[_typeWeapon].Anim_hit3 != "")
			{
				switch (UnityEngine.Random.Range(0, 3))
				{
				case 0:
					Hands.Play(_weapons[_typeWeapon].Anim_hit1);
					break;
				case 1:
					Hands.Play(_weapons[_typeWeapon].Anim_hit2);
					break;
				default:
					Hands.Play(_weapons[_typeWeapon].Anim_hit3);
					break;
				}
			}
			else if (UnityEngine.Random.Range(0, 2) == 0)
			{
				Hands.Play(_weapons[_typeWeapon].Anim_hit1);
			}
			else
			{
				Hands.Play(_weapons[_typeWeapon].Anim_hit2);
			}
			break;
		}
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		if (Physics.Raycast(ray, out var hit, 1.5f))
		{
			Hit(hit, ray);
		}
		else if ((PlayerPrefsSafe.GetInt("AimActive") == 1) & (ControllerPolygon._enemy != null))
		{
			ControllerPolygon.HitEnemy(_weapons[_typeWeapon]._damageHit, 3f, 1);
		}
		yield return new WaitForSeconds(0.55f);
		_zad = false;
	}

	public IEnumerator ShootWhile()
	{
		while (Controller._shootWhile)
		{
			StartCoroutine(Shoot());
			yield return new WaitForSeconds(0.01f);
		}
	}

	public IEnumerator Shoot()
	{
		if (!_zad & (Time.timeScale != 0f))
		{
			_zad = true;
			if (_pricel)
			{
				_sensitivity = m_Sensitivity;
				Camera.fieldOfView = 60f;
				Pricele.transform.localScale = new Vector3(0f, 0f, 0f);
				Hands.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			Hands.Play("None");
			yield return new WaitForSeconds(0.0001f);
			Hands.Play(_weapons[_typeWeapon].Anim_hit1);
			_audioSource.clip = _weapons[_typeWeapon].ShootAudio;
			_audioSource.PlayOneShot(_audioSource.clip);
			StartCoroutine(Shot(_weapons[_typeWeapon]._patrOnAttack));
		}
	}

	private IEnumerator Shot(int _patrOnAttack)
	{
		_patrOnAttack--;
		rand1 = UnityEngine.Random.Range(0.5f - _weapons[_typeWeapon]._razbros, 0.5f + _weapons[_typeWeapon]._razbros);
		rand2 = UnityEngine.Random.Range(0.5f - _weapons[_typeWeapon]._razbros, 0.5f + _weapons[_typeWeapon]._razbros);
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(rand1, rand2));
		if (Physics.Raycast(ray, out var hit))
		{
			Hit(hit, ray);
		}
		if (_patrOnAttack > 0)
		{
			StartCoroutine(Shot(_patrOnAttack));
			yield break;
		}
		ChangeOffset(UnityEngine.Random.Range(_weapons[_typeWeapon]._recoilY / 2f, _weapons[_typeWeapon]._recoilY), UnityEngine.Random.Range(0f - _weapons[_typeWeapon]._recoilX, _weapons[_typeWeapon]._recoilX));
		yield return new WaitForSeconds(_weapons[_typeWeapon]._zadAttack);
		if (_pricel)
		{
			_sensitivity = m_SensitivityAim;
			Camera.fieldOfView = _weapons[_typeWeapon].FOVSniper;
			Pricele.transform.localScale = new Vector3(1f, 1f, 1f);
			Hands.transform.localScale = new Vector3(0f, 0f, 0f);
		}
		_zad = false;
	}

	public IEnumerator Inspect()
	{
		if (!_zad & (Time.timeScale != 0f))
		{
			Hands.Play("None");
			yield return new WaitForSeconds(0.0001f);
			Hands.Play(_weapons[_typeWeapon].Anim_inspect);
		}
	}

	public void Take(int TypeTake)
	{
		if (Time.timeScale != 0f)
		{
			StopAllCoroutines();
			switch (TypeTake)
			{
			case 1:
				StartCoroutine(Podbor(_typeTake));
				break;
			case 3:
				StartCoroutine(Podbor(_typeTake3));
				break;
			}
		}
	}

	public void Pricel()
	{
		if (!_zad & (Time.timeScale != 0f))
		{
			RuntimeManager.PlayOneShot("event:/Weapon/Character/AWM/character_awm_scope");
			switch (_pricel)
			{
			case true:
				_sensitivity = m_Sensitivity;
				Camera.fieldOfView = 60f;
				Pricele.transform.localScale = new Vector3(0f, 0f, 0f);
				Hands.transform.localScale = new Vector3(1f, 1f, 1f);
				break;
			case false:
				_sensitivity = m_SensitivityAim;
				Camera.fieldOfView = _weapons[_typeWeapon].FOVSniper;
				Pricele.transform.localScale = new Vector3(1f, 1f, 1f);
				Hands.transform.localScale = new Vector3(0f, 0f, 0f);
				break;
			}
			_pricel = !_pricel;
		}
	}

	private void Hit(RaycastHit hit, Ray ray)
	{
		if (hit.transform.tag == "ButtonPerecl")
		{
			ControllerPolygon.PereclGame();
		}
		else if (hit.transform.tag == "ButtonLeft_Priz")
		{
			ControllerPolygon.ModePrizSet(0);
		}
		else if (hit.transform.tag == "ButtonRight_Priz")
		{
			ControllerPolygon.ModePrizSet(1);
		}
		else if (hit.transform.tag == "ButtonLeft_Game")
		{
			ControllerPolygon.ModeGameSet(0);
		}
		else if (hit.transform.tag == "ButtonRight_Game")
		{
			ControllerPolygon.ModeGameSet(1);
		}
		else if (hit.transform.tag == "ButtonRight2_Game")
		{
			ControllerPolygon.ModeGameSet(2);
		}
		else if (hit.transform.tag == "ButtonLeft_Timer")
		{
			ControllerPolygon.DurationTimerAdd(0, 0, -15);
		}
		else if (hit.transform.tag == "ButtonRight_Timer")
		{
			ControllerPolygon.DurationTimerAdd(0, 1, 15);
		}
		else if (hit.transform.tag == "ButtonLeft_Timer2")
		{
			ControllerPolygon.DurationTimerAdd(1, 0, -5);
		}
		else if (hit.transform.tag == "ButtonRight_Timer2")
		{
			ControllerPolygon.DurationTimerAdd(1, 1, 5);
		}
		else if (_weapons[_typeWeapon].type != 0)
		{
			if (hit.transform.tag == "EnemyHead")
			{
				ControllerPolygon.HitEnemy(_weapons[_typeWeapon]._damageHit, 3f, 0);
			}
			else if (hit.transform.tag == "EnemyBody")
			{
				ControllerPolygon.HitEnemy(_weapons[_typeWeapon]._damageHit, 2f, 0);
			}
			else if (hit.transform.tag == "EnemyLegs")
			{
				ControllerPolygon.HitEnemy(_weapons[_typeWeapon]._damageHit, 1f, 0);
			}
			else if ((PlayerPrefs.GetInt("AimActive") == 1) & (ControllerPolygon._enemy != null))
			{
				ControllerPolygon.HitEnemy(_weapons[_typeWeapon]._damageHit, 3f, 0);
			}
		}
		else if (Physics.Raycast(ray, out hit, 0.8f) & (_weapons[_typeWeapon].type == Weapon.Type.Knife))
		{
			if (hit.transform.tag == "EnemyHead")
			{
				ControllerPolygon.HitEnemy(_weapons[_typeWeapon]._damageHit, 3f, 1);
			}
			else if (hit.transform.tag == "EnemyBody")
			{
				ControllerPolygon.HitEnemy(_weapons[_typeWeapon]._damageHit, 2f, 1);
			}
			else if (hit.transform.tag == "EnemyLegs")
			{
				ControllerPolygon.HitEnemy(_weapons[_typeWeapon]._damageHit, 1f, 1);
			}
			else if ((PlayerPrefs.GetInt("AimActive") == 1) & (ControllerPolygon._enemy != null))
			{
				ControllerPolygon.HitEnemy(_weapons[_typeWeapon]._damageHit, 3f, 1);
			}
		}
	}
}
