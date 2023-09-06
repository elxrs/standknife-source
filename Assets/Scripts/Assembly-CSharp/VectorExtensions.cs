using UnityEngine;

public static class VectorExtensions
{
	public static Vector3 VectorMa(Vector3 start, float scale, Vector3 direction)
	{
		Vector3 result = default(Vector3);
		result.x = start.x + direction.x * scale;
		result.y = start.y + direction.y * scale;
		result.z = start.z + direction.z * scale;
		return result;
	}
}
