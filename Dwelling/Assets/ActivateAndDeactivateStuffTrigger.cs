using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAndDeactivateStuffTrigger : AudioTriggerAndCameraChange {
	[SerializeField] protected GameObject[] stuffToActivate;
	[SerializeField] protected GameObject[] stuffToDeactivate;
	public override void DoThing ()
	{
		foreach (GameObject go in stuffToActivate)
			go.SetActive (true);
		foreach (GameObject go in stuffToDeactivate)
			go.SetActive (false);
		base.DoThing ();
	}
}
