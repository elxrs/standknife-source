using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
	public float speed;

	public VariableJoystick variableJoystick;

	public Rigidbody rb;

	public void FixedUpdate()
	{
		Vector3 vector = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
		rb.AddForce(vector * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
	}
}
