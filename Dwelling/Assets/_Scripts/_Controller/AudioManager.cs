using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX_SOLOMON{
	JUMP = 0,
	BITE,
	WALLBITE,
	WALLJUMP,
	HURT
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
	[SerializeField] AudioClip[] audioClips;
	AudioSource audioSource;

	void Awake()
	{
		audioSource = GetComponent<AudioSource> ();
	}

	public void Play(int audioClip)
	{
		if (audioClip > -1 && audioClip < audioClips.Length) {
			audioSource.clip = audioClips [audioClip];
			audioSource.Play ();
		}
	}

	public void PlayForFuckingStupidAnimatorWhoCantTellTheDifferenceBetweenTwoGoddamnFunctions(int audioClip)
	{
		Play (audioClip);
	}
	
	public void Play(int audioClip, bool looping)
	{
		if (audioClip > -1 && audioClip < audioClips.Length) {
			audioSource.clip = audioClips [audioClip];
			audioSource.loop = looping;
			audioSource.Play ();
		}
	}

	public void Play(SFX_SOLOMON solomonClip)
	{
		int audioClip = (int)solomonClip;
		if (audioClip > -1 && audioClip < audioClips.Length) {
			audioSource.clip = audioClips [audioClip];
			audioSource.Play ();
		}
	}

	public void Play(SFX_SOLOMON solomonClip, bool looping)
	{
		int audioClip = (int)solomonClip;
		if (audioClip > -1 && audioClip < audioClips.Length) {
			audioSource.clip = audioClips [audioClip];
			audioSource.loop = looping;
			audioSource.Play ();
		}
	}

	public void SetPitch(float addlPitch)
	{
		audioSource.pitch = 1 + addlPitch;
	}

	public void Stop()
	{
		audioSource.Stop ();
	}
}
