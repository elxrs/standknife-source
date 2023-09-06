using UnityEngine;

public struct Int
{
	private int value;

	private int salt;

	public Int(int value)
	{
		salt = Random.Range(-536870912, 536870911);
		this.value = value ^ salt;
	}

	public override bool Equals(object obj)
	{
		return (int)this == (int)obj;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return ((int)this).ToString();
	}

	public static implicit operator int(Int safeInt)
	{
		return safeInt.value ^ safeInt.salt;
	}

	public static implicit operator Int(int normalInt)
	{
		return new Int(normalInt);
	}
}
