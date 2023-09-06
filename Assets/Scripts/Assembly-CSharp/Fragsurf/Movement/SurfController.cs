using Fragsurf.TraceUtil;
using UnityEngine;

namespace Fragsurf.Movement
{
	public class SurfController
	{
		private ISurfControllable _surfer;

		private MovementConfig _config;

		private float _deltaTime;

		public void ProcessMovement(ISurfControllable surfer, MovementConfig config, float deltaTime)
		{
			_surfer = surfer;
			_config = config;
			_deltaTime = deltaTime;
			if (_surfer.GroundObject == null)
			{
				_surfer.MoveData.Velocity.y -= _surfer.MoveData.GravityFactor * _config.Gravity * _deltaTime;
				_surfer.MoveData.Velocity.y += _surfer.BaseVelocity.y * _deltaTime;
			}
			CheckGrounded();
			CalculateMovementVelocity();
			_surfer.MoveData.Origin += _surfer.MoveData.Velocity * _deltaTime;
			SurfPhysics.ResolveCollisions(_surfer.Collider, ref _surfer.MoveData.Origin, ref _surfer.MoveData.Velocity);
			_surfer = null;
		}

		private void CalculateMovementVelocity()
		{
			MoveType moveType = _surfer.MoveType;
			if (moveType != MoveType.Walk)
			{
				return;
			}
			if (_surfer.GroundObject == null)
			{
				_surfer.MoveData.Velocity += AirInputMovement();
				SurfPhysics.Reflect(ref _surfer.MoveData.Velocity, _surfer.Collider, _surfer.MoveData.Origin, _deltaTime);
				return;
			}
			_surfer.MoveData.Velocity += GroundInputMovement();
			if (_surfer.MoveData.Buttons.HasFlag(2))
			{
				Jump();
				return;
			}
			float friction = _surfer.MoveData.SurfaceFriction * _config.Friction;
			float stopSpeed = _config.StopSpeed;
			SurfPhysics.Friction(ref _surfer.MoveData.Velocity, stopSpeed, friction, _deltaTime);
		}

		private Vector3 GroundInputMovement()
		{
			GetWishValues(out var wishVel, out var wishDir, out var wishSpeed);
			if (wishSpeed != 0f && wishSpeed > _config.MaxSpeed)
			{
				wishVel *= _config.MaxSpeed / wishSpeed;
				wishSpeed = _config.MaxSpeed;
			}
			wishSpeed *= _surfer.MoveData.WalkFactor;
			return SurfPhysics.Accelerate(_surfer.MoveData.Velocity, wishDir, wishSpeed, _config.Accel, _deltaTime, _surfer.MoveData.SurfaceFriction);
		}

		private Vector3 AirInputMovement()
		{
			GetWishValues(out var wishVel, out var wishDir, out var wishSpeed);
			if (_config.ClampAirSpeed && wishSpeed != 0f && wishSpeed > _config.MaxSpeed)
			{
				wishVel *= _config.MaxSpeed / wishSpeed;
				wishSpeed = _config.MaxSpeed;
			}
			return SurfPhysics.AirAccelerate(_surfer.MoveData.Velocity, wishDir, wishSpeed, _config.AirAccel, _config.AirCap, _deltaTime);
		}

		private void GetWishValues(out Vector3 wishVel, out Vector3 wishDir, out float wishSpeed)
		{
			wishVel = Vector3.zero;
			wishDir = Vector3.zero;
			wishSpeed = 0f;
			Vector3 forward = _surfer.Forward;
			Vector3 right = _surfer.Right;
			forward[1] = 0f;
			right[1] = 0f;
			forward.Normalize();
			right.Normalize();
			for (int i = 0; i < 3; i++)
			{
				wishVel[i] = forward[i] * _surfer.MoveData.ForwardMove + right[i] * _surfer.MoveData.SideMove;
			}
			wishVel[1] = 0f;
			wishSpeed = wishVel.magnitude;
			wishDir = wishVel.normalized;
		}

		private void Jump()
		{
			if (_config.AutoBhop || !_surfer.MoveData.OldButtons.HasFlag(2))
			{
				_surfer.MoveData.Velocity.y += _config.JumpPower;
			}
		}

		private bool CheckGrounded()
		{
			_surfer.MoveData.SurfaceFriction = 1f;
			bool flag = _surfer.MoveData.Velocity.y > 0f;
			Trace trace = TraceToFloor();
			if (trace.HitCollider == null || trace.PlaneNormal.y < 0.7f || flag)
			{
				SetGround(null);
				if (flag && _surfer.MoveType != MoveType.Noclip)
				{
					_surfer.MoveData.SurfaceFriction = _config.AirFriction;
				}
				return false;
			}
			SetGround(trace.HitCollider.gameObject);
			return true;
		}

		private void SetGround(GameObject obj)
		{
			if (obj != null)
			{
				_surfer.GroundObject = obj;
				_surfer.MoveData.Velocity.y = 0f;
			}
			else
			{
				_surfer.GroundObject = null;
			}
		}

		private Trace TraceBounds(Vector3 start, Vector3 end, int layerMask)
		{
			return Tracer.TraceCollider(_surfer.Collider, start, end, layerMask);
		}

		private Trace TraceToFloor()
		{
			Vector3 origin = _surfer.MoveData.Origin;
			origin.y -= 0.1f;
			return Tracer.TraceCollider(_surfer.Collider, _surfer.MoveData.Origin, origin, SurfPhysics.GroundLayerMask);
		}
	}
}
