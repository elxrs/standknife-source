using UnityEngine;

namespace Fragsurf.Movement
{
	[AddComponentMenu("Fragsurf/Surf Character")]
	public class SurfCharacter : MonoBehaviour, ISurfControllable
	{
		public enum ColliderType
		{
			Capsule = 0,
			Box = 1
		}

		[Header("Physics Settings")]
		public int TickRate = 128;

		public Vector3 ColliderSize = new Vector3(1f, 2f, 1f);

		public ColliderType CollisionType;

		[Header("View Settings")]
		public Transform viewTransform;

		public Vector3 ViewOffset = new Vector3(0f, 0.61f, 0f);

		[Header("Input Settings")]
		public float XSens = 15f;

		public float YSens = 15f;

		public KeyCode JumpButton = KeyCode.LeftShift;

		public KeyCode MoveLeft = KeyCode.A;

		public KeyCode MoveRight = KeyCode.D;

		public KeyCode MoveForward = KeyCode.W;

		public KeyCode MoveBack = KeyCode.S;

		[Header("Movement Config")]
		[SerializeField]
		public MovementConfig moveConfig;

		private GameObject _groundObject;

		private Vector3 _baseVelocity;

		private Collider _collider;

		private Vector3 _angles;

		private Vector3 _startPosition;

		private MoveData _moveData = new MoveData();

		private SurfController _controller = new SurfController();

		public MoveType MoveType => MoveType.Walk;

		public MovementConfig MoveConfig => moveConfig;

		public MoveData MoveData => _moveData;

		public Collider Collider => _collider;

		public GameObject GroundObject
		{
			get
			{
				return _groundObject;
			}
			set
			{
				_groundObject = value;
			}
		}

		public Vector3 BaseVelocity => _baseVelocity;

		public Vector3 Forward => viewTransform.forward;

		public Vector3 Right => viewTransform.right;

		public Vector3 Up => viewTransform.up;

		private void Awake()
		{
			Application.targetFrameRate = 144;
			QualitySettings.vSyncCount = 1;
			Time.fixedDeltaTime = 1f / (float)TickRate;
		}

		private void Start()
		{
			if (viewTransform == null)
			{
				viewTransform = Camera.main.transform;
			}
			viewTransform.SetParent(base.transform, worldPositionStays: false);
			viewTransform.localPosition = ViewOffset;
			viewTransform.localRotation = base.transform.rotation;
			_collider = base.gameObject.GetComponent<Collider>();
			if (_collider != null)
			{
				Object.Destroy(_collider);
			}
			Rigidbody rigidbody = base.gameObject.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = base.gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.isKinematic = true;
			switch (CollisionType)
			{
			case ColliderType.Box:
				_collider = base.gameObject.AddComponent<BoxCollider>();
				((BoxCollider)_collider).size = ColliderSize;
				break;
			case ColliderType.Capsule:
			{
				_collider = base.gameObject.AddComponent<CapsuleCollider>();
				CapsuleCollider obj = (CapsuleCollider)_collider;
				obj.height = ColliderSize.y;
				obj.radius = ColliderSize.x / 2f;
				break;
			}
			}
			_collider.isTrigger = true;
			_moveData.Origin = base.transform.position;
			_startPosition = base.transform.position;
		}

		private void Update()
		{
			UpdateTestBinds();
			UpdateRotation();
			UpdateMoveData();
		}

		private void UpdateTestBinds()
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				MoveData.Velocity = Vector3.zero;
				MoveData.Origin = _startPosition;
			}
		}

		private void LateUpdate()
		{
			viewTransform.rotation = Quaternion.Euler(_angles);
		}

		private void FixedUpdate()
		{
			_controller.ProcessMovement(this, moveConfig, Time.fixedDeltaTime);
			base.transform.position = MoveData.Origin;
		}

		private void UpdateMoveData()
		{
			bool key = Input.GetKey(MoveLeft);
			bool key2 = Input.GetKey(MoveRight);
			bool key3 = Input.GetKey(MoveForward);
			bool key4 = Input.GetKey(MoveBack);
			bool key5 = Input.GetKey(JumpButton);
			if (!key && !key2)
			{
				_moveData.SideMove = 0f;
			}
			else if (key)
			{
				_moveData.SideMove = 0f - MoveConfig.Accel;
			}
			else if (key2)
			{
				_moveData.SideMove = MoveConfig.Accel;
			}
			if (!key3 && !key4)
			{
				_moveData.ForwardMove = 0f;
			}
			else if (key3)
			{
				_moveData.ForwardMove = MoveConfig.Accel;
			}
			else if (key4)
			{
				_moveData.ForwardMove = 0f - MoveConfig.Accel;
			}
			if (key5)
			{
				_moveData.Buttons = _moveData.Buttons.AddFlag(2);
			}
			else
			{
				_moveData.Buttons = _moveData.Buttons.RemoveFlag(2);
			}
			_moveData.OldButtons = _moveData.Buttons;
			_moveData.ViewAngles = _angles;
		}

		private void UpdateRotation()
		{
			float y = Input.GetAxis("Mouse X") * XSens * 0.02f;
			float num = Input.GetAxis("Mouse Y") * YSens * 0.02f;
			Vector3 angles = _angles + new Vector3(0f - num, y, 0f);
			angles.x = ClampAngle(angles.x, -85f, 85f);
			_angles = angles;
		}

		public static float ClampAngle(float angle, float from, float to)
		{
			if (angle < 0f)
			{
				angle = 360f + angle;
			}
			if (angle > 180f)
			{
				return Mathf.Max(angle, 360f + from);
			}
			return Mathf.Min(angle, to);
		}
	}
}
