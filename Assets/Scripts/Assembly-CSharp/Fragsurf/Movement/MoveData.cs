using UnityEngine;

namespace Fragsurf.Movement
{
	public class MoveData
	{
		public Vector3 Origin;

		public Vector3 ViewAngles;

		public Vector3 Velocity;

		public int Buttons;

		public int OldButtons;

		public float ForwardMove;

		public float SideMove;

		public float UpMove;

		public float SurfaceFriction = 1f;

		public float GravityFactor = 1f;

		public float WalkFactor = 1f;
	}
}
