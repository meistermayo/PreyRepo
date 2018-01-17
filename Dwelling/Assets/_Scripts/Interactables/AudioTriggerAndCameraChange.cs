using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerAndCameraChange : AudioTrigger {
	[SerializeField] float camSize;
	[SerializeField] Transform newTransform;

	public override void DoThing ()
	{
		base.DoThing ();
		Camera.main.GetComponent<CameraLerp> ().SetCamera(newTransform,camSize);
	}
}
