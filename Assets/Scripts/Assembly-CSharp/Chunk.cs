using UnityEngine;

public class Chunk : MonoBehaviour
{
	public enum Type
	{
		Default = 0,
		RotateLeft = 1,
		RotateRight = 2
	}

	public Type type;

	public Transform Begin;

	public Transform End;

	public AnimationCurve ChanceFromDistance;
}
