using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : OnTriggerDoThing {
	[SerializeField] AudioClip audioClip;

	public override void DoThing ()
	{
		BGM_Controller.PlayMusic(audioClip,true);
	}
}
