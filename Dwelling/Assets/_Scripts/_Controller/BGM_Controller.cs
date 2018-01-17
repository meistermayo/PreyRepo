using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM_Controller : MonoBehaviour {

	static AudioSource audioSource;
	static bool fadingOut;
	static float fadeOutRate = .0025f;
	static AudioClip clipToPlay;
	float audioVolume;
	public static AudioClip GetClip()
	{
		return audioSource.clip;
	}
	void Start()
	{
		audioSource = GetComponent<AudioSource> ();
		audioVolume = audioSource.volume;
	}

	public static void PlayMusic(AudioClip music, bool fadeOut)
	{
		clipToPlay = music;
		if (fadeOut)
			fadingOut = true;
		else {
			audioSource.clip = clipToPlay;
			audioSource.Play ();
		}
	}

	void FixedUpdate()
	{
		if (fadingOut) {
			FadeOutMusic ();
		}
	}

	void FadeOutMusic()
	{
		audioSource.volume -= fadeOutRate;
		if (audioSource.volume <= 0f) {
			fadingOut = false;
			audioSource.volume = audioVolume;
			audioSource.clip = clipToPlay;
			audioSource.Play ();
		}
	}
}
