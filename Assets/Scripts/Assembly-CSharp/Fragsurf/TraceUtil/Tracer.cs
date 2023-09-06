using System;
using Fragsurf.Movement;
using UnityEngine;

namespace Fragsurf.TraceUtil
{
	public class Tracer
	{
		public static Trace TraceCollider(Collider collider, Vector3 origin, Vector3 end, int layerMask)
		{
			if (collider is BoxCollider)
			{
				return TraceBox(origin, end, collider.bounds.extents, collider.contactOffset, layerMask);
			}
			if (collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)collider;
				SurfPhysics.GetCapsulePoints(capsuleCollider, origin, out var p, out var p2);
				return TraceCapsule(p, p2, capsuleCollider.radius, origin, end, capsuleCollider.contactOffset, layerMask);
			}
			throw new NotImplementedException("Trace missing for collider: " + collider.GetType());
		}

		public static Trace TraceCapsule(Vector3 point1, Vector3 point2, float radius, Vector3 start, Vector3 destination, float contactOffset, int layerMask)
		{
			Trace trace = default(Trace);
			trace.StartPos = start;
			trace.EndPos = destination;
			Trace result = trace;
			float num = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
			radius *= 1f - contactOffset;
			Vector3 normalized = (destination - start).normalized;
			float num2 = Vector3.Distance(start, destination) + num;
			if (Physics.CapsuleCast(point1, point2, radius, normalized, out var hitInfo, num2, layerMask))
			{
				result.Fraction = hitInfo.distance / num2;
				result.HitCollider = hitInfo.collider;
				result.HitPoint = hitInfo.point;
				result.PlaneNormal = hitInfo.normal;
			}
			else
			{
				result.Fraction = 1f;
			}
			return result;
		}

		public static Trace TraceBox(Vector3 start, Vector3 destination, Vector3 extents, float contactOffset, int layerMask)
		{
			Trace trace = default(Trace);
			trace.StartPos = start;
			trace.EndPos = destination;
			Trace result = trace;
			float num = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
			Vector3 normalized = (destination - start).normalized;
			float num2 = Vector3.Distance(start, destination) + num;
			extents *= 1f - contactOffset;
			if (Physics.BoxCast(start, extents, normalized, out var hitInfo, Quaternion.identity, num2, layerMask))
			{
				result.Fraction = hitInfo.distance / num2;
				result.HitCollider = hitInfo.collider;
				result.HitPoint = hitInfo.point;
				result.PlaneNormal = hitInfo.normal;
			}
			else
			{
				result.Fraction = 1f;
			}
			return result;
		}
	}
}
