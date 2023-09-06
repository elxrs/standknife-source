using UnityEngine;

public class SamoKill : MonoBehaviour
{
	public float TimeKilled = 4.5f;

	public float TR_1 = 2.5f;

	public float TR_2 = 5.5f;

	public bool RandomTimeKilled;

	private float RandomInt;

	private void Start()
	{
		if (RandomTimeKilled)
		{
			RandomInt = Random.Range(TR_1, TR_2);
			Object.Destroy(base.gameObject, RandomInt);
		}
		else
		{
			Object.Destroy(base.gameObject, TimeKilled);
		}
	}
}
