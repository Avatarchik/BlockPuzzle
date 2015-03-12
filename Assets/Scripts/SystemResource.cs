using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SystemResource : MonoBehaviour
{
	static SystemResource _instance;

	public static SystemResource instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<SystemResource>();
			}
			return _instance;
		}
	}

	public void PlaySE()
	{
		if (!mute)
		{
			GetComponent<AudioSource>().PlayOneShot(se[0]);
		}
	}

	public bool mute = false;
	public Material[] materials;
	public AudioClip[] se;
	public Color[] colors;
}
