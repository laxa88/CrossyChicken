using UnityEngine;
using System.Collections;

public class AudioManagerScript : MonoBehaviour {

	public static AudioManagerScript instance;
	public AudioSource source;
	public AudioClip smackCarSound;
	public AudioClip hopSound;
	public AudioClip bgmSound;

	public static void playSound (string soundName)
	{
		// playSound can be repeatedly used to play
		// different sounds in short bursts

		if (instance == null)
			instance = FindObjectOfType<AudioManagerScript>();
			
		switch (soundName)
		{
		case "smack":
			instance.source.PlayOneShot(instance.smackCarSound);
			break;

		case "hop":
			instance.source.PlayOneShot(instance.hopSound);
			break;
		}
	}

	public static void playMusic (string musicName)
	{
		// playMusic is for background music that is
		// usually just called once

		if (instance == null)
			instance = FindObjectOfType<AudioManagerScript>();

		switch (musicName)
		{
		case "maingame":
			instance.source.clip = instance.bgmSound;
			instance.source.loop = true;
			instance.source.Play();
			break;
		}
	}
}
