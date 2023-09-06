using UnityEngine;

public class CamaraFoller : MonoBehaviour
{
	public GameObject Player;

	public GameObject Camera;

	public float speed;

	private void Update()
	{
		base.transform.position = Camera.transform.position;
		base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, Quaternion.Euler(Camera.transform.localRotation.eulerAngles.x, Player.transform.localRotation.eulerAngles.y, 0f), speed * Time.deltaTime);
	}
}
