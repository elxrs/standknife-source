using Fragsurf.TraceUtil;
using UnityEngine;

namespace Fragsurf.Movement
{
	public class SurfPhysics
	{
		public static int GroundLayerMask = 1;

		private static Collider[] _colliders = new Collider[128];

		private static Vector3[] _planes = new Vector3[5];

		public const float HU2M = 52.49344f;

		private const int MaxCollisions = 128;

		private const int MaxClipPlanes = 5;

		private const int NumBumps = 1;

		public const float SurfSlope = 0.7f;

		public static void ResolveCollisions(Collider collider, ref Vector3 origin, ref Vector3 velocity)
		{
			int num = 0;
			if (collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = collider as CapsuleCollider;
				GetCapsulePoints(capsuleCollider, origin, out var p, out var p2);
				num = Physics.OverlapCapsuleNonAlloc(p, p2, capsuleCollider.radius, _colliders, GroundLayerMask, QueryTriggerInteraction.Ignore);
			}
			else if (collider is BoxCollider)
			{
				num = Physics.OverlapBoxNonAlloc(origin, collider.bounds.extents, _colliders, Quaternion.identity, GroundLayerMask, QueryTriggerInteraction.Ignore);
			}
			for (int i = 0; i < num; i++)
			{
				if (Physics.ComputePenetration(collider, origin, Quaternion.identity, _colliders[i], _colliders[i].transform.position, _colliders[i].transform.rotation, out var direction, out var distance))
				{
					direction.Normalize();
					Vector3 vector = direction * distance;
					Vector3 vector2 = Vector3.Project(velocity, -direction);
					vector2.y = 0f;
					origin += vector;
					velocity -= vector2;
				}
			}
		}

		public static void Friction(ref Vector3 velocity, float stopSpeed, float friction, float deltaTime)
		{
			float magnitude = velocity.magnitude;
			if (!(magnitude < 0.0001905f))
			{
				float num = 0f;
				float num2 = ((magnitude < stopSpeed) ? stopSpeed : magnitude);
				num += num2 * friction * deltaTime;
				float num3 = magnitude - num;
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				if (num3 != magnitude)
				{
					num3 /= magnitude;
					velocity *= num3;
				}
			}
		}

		public static Vector3 AirAccelerate(Vector3 velocity, Vector3 wishdir, float wishspeed, float accel, float airCap, float deltaTime)
		{
			float num = Mathf.Min(wishspeed, airCap);
			float num2 = Vector3.Dot(velocity, wishdir);
			float num3 = num - num2;
			if (num3 <= 0f)
			{
				return Vector3.zero;
			}
			float a = accel * wishspeed * deltaTime;
			a = Mathf.Min(a, num3);
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < 3; i++)
			{
				zero[i] += a * wishdir[i];
			}
			return zero;
		}

		public static Vector3 Accelerate(Vector3 currentVelocity, Vector3 wishdir, float wishspeed, float accel, float deltaTime, float surfaceFriction)
		{
			float num = Vector3.Dot(currentVelocity, wishdir);
			float num2 = wishspeed - num;
			if (num2 <= 0f)
			{
				return Vector3.zero;
			}
			float num3 = accel * deltaTime * wishspeed * surfaceFriction;
			if (num3 > num2)
			{
				num3 = num2;
			}
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < 3; i++)
			{
				zero[i] += num3 * wishdir[i];
			}
			return zero;
		}

		public static int Reflect(ref Vector3 velocity, Collider collider, Vector3 origin, float deltaTime)
		{
			Vector3 output = Vector3.zero;
			int num = 0;
			int num2 = 0;
			Vector3 input = velocity;
			Vector3 rhs = velocity;
			float num3 = 0f;
			float num4 = deltaTime;
			for (int i = 0; i < 1; i++)
			{
				if (velocity.magnitude == 0f)
				{
					break;
				}
				Vector3 end = VectorExtensions.VectorMa(origin, num4, velocity);
				Trace trace = Tracer.TraceCollider(collider, origin, end, GroundLayerMask);
				num3 += trace.Fraction;
				if (trace.Fraction > 0f)
				{
					input = velocity;
					num2 = 0;
				}
				if (trace.Fraction == 1f)
				{
					break;
				}
				if (trace.PlaneNormal.y > 0.7f)
				{
					num |= 1;
				}
				if (trace.PlaneNormal.y == 0f)
				{
					num |= 2;
				}
				num4 -= num4 * trace.Fraction;
				if (num2 >= 5)
				{
					velocity = Vector3.zero;
					break;
				}
				_planes[num2] = trace.PlaneNormal;
				num2++;
				if (num2 == 1)
				{
					for (int j = 0; j < num2; j++)
					{
						if (_planes[j][1] > 0.7f)
						{
							return num;
						}
						ClipVelocity(input, _planes[j], ref output, 1f);
					}
					velocity = output;
					input = output;
					continue;
				}
				int num5 = 0;
				for (num5 = 0; num5 < num2; num5++)
				{
					ClipVelocity(input, _planes[num5], ref velocity, 1f);
					int num6 = 0;
					for (num6 = 0; num6 < num2 && (num6 == num5 || !(Vector3.Dot(velocity, _planes[num6]) < 0f)); num6++)
					{
					}
					if (num6 == num2)
					{
						break;
					}
				}
				float num7;
				if (num5 == num2)
				{
					if (num2 != 2)
					{
						velocity = Vector3.zero;
						break;
					}
					Vector3 normalized = Vector3.Cross(_planes[0], _planes[1]).normalized;
					num7 = Vector3.Dot(normalized, velocity);
					velocity = normalized * num7;
				}
				num7 = Vector3.Dot(velocity, rhs);
				if (num7 <= 0f)
				{
					velocity = Vector3.zero;
					break;
				}
			}
			if (num3 == 0f)
			{
				velocity = Vector3.zero;
			}
			return num;
		}

		public static int ClipVelocity(Vector3 input, Vector3 normal, ref Vector3 output, float overbounce)
		{
			float num = normal[1];
			int num2 = 0;
			if (num > 0f)
			{
				num2 |= 1;
			}
			if (num == 0f)
			{
				num2 |= 2;
			}
			float num3 = Vector3.Dot(input, normal) * overbounce;
			for (int i = 0; i < 3; i++)
			{
				float num4 = normal[i] * num3;
				output[i] = input[i] - num4;
			}
			float num5 = Vector3.Dot(output, normal);
			if (num5 < 0f)
			{
				output -= normal * num5;
			}
			return num2;
		}

		public static void GetCapsulePoints(CapsuleCollider capc, Vector3 origin, out Vector3 p1, out Vector3 p2)
		{
			float num = capc.height / 2f - capc.radius;
			p1 = origin + capc.center + Vector3.up * num;
			p2 = origin + capc.center - Vector3.up * num;
		}
	}
}
