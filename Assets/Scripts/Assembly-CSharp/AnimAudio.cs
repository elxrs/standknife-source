using FMODUnity;
using UnityEngine;

public class AnimAudio : MonoBehaviour
{
	public void Play(string _eventName)
	{
		RuntimeManager.PlayOneShot(_eventName);
	}

	public void PlayAudio(string _eventName)
	{
		RuntimeManager.PlayOneShot(_eventName);
	}
}
