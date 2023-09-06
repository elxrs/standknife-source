using UnityEngine;

public class UnityEditors : MonoBehaviour
{
	public int _test;

	private void Start()
	{
		Debug.Log(_test *= (int)((float)int.Parse(Controller._obj.AccIDStringDef) * Controller._obj._acciddelete));
	}
}
