using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnTriggerDoThing : MonoBehaviour {
	public bool active = true;

	void OnTriggerEnter2D( Collider2D other)
	{
		if (!active)
			return;
		if (other.GetComponent<PlayerMovement>() != null)
		{
			{
				OnTriggerDoThing[] triggers = FindObjectsOfType<OnTriggerDoThing> ();
				foreach (AudioTrigger trigger in triggers) {
					trigger.active = true;
				}	
			}
			active = false;
			DoThing ();
		}
	}

	public abstract void DoThing ();
}
