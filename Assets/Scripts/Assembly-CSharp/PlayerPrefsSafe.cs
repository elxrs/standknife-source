using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class PlayerPrefsSafe
{
	private const int salt = 678309397;

	public static void SetInt(string key, int value)
	{
		int value2 = value ^ 0x286E2E15;
		PlayerPrefs.SetInt(StringHash(key), value2);
		PlayerPrefs.SetInt(StringHash("_" + key), IntHash(value));
	}

	public static int GetInt(string key)
	{
		return GetInt(key, 0);
	}

	public static int GetInt(string key, int defaultValue)
	{
		string key2 = StringHash(key);
		if (!PlayerPrefs.HasKey(key2))
		{
			return defaultValue;
		}
		int num = PlayerPrefs.GetInt(key2) ^ 0x286E2E15;
		if (PlayerPrefs.GetInt(StringHash("_" + key)) != IntHash(num))
		{
			return defaultValue;
		}
		return num;
	}

	public static void SetFloat(string key, float value)
	{
		int num = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
		int value2 = num ^ 0x286E2E15;
		PlayerPrefs.SetInt(StringHash(key), value2);
		PlayerPrefs.SetInt(StringHash("_" + key), IntHash(num));
	}

	public static float GetFloat(string key)
	{
		return GetFloat(key, 0f);
	}

	public static float GetFloat(string key, float defaultValue)
	{
		string key2 = StringHash(key);
		if (!PlayerPrefs.HasKey(key2))
		{
			return defaultValue;
		}
		int num = PlayerPrefs.GetInt(key2) ^ 0x286E2E15;
		if (PlayerPrefs.GetInt(StringHash("_" + key)) != IntHash(num))
		{
			return defaultValue;
		}
		return BitConverter.ToSingle(BitConverter.GetBytes(num), 0);
	}

	private static int IntHash(int x)
	{
		x = ((x >> 16) ^ x) * 73244475;
		x = ((x >> 16) ^ x) * 73244475;
		x = (x >> 16) ^ x;
		return x;
	}

	public static string StringHash(string x)
	{
		SHA256 sHA = SHA256.Create();
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = sHA.ComputeHash(Encoding.UTF8.GetBytes(x));
		foreach (byte b in array)
		{
			stringBuilder.Append(b.ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(StringHash(key));
		PlayerPrefs.DeleteKey(StringHash("_" + key));
	}

	public static bool HasKey(string key)
	{
		string key2 = StringHash(key);
		if (!PlayerPrefs.HasKey(key2))
		{
			return false;
		}
		int x = PlayerPrefs.GetInt(key2) ^ 0x286E2E15;
		return PlayerPrefs.GetInt(StringHash("_" + key)) == IntHash(x);
	}
}
