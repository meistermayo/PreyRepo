using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
	Animator animator;
	bool open;

	void Start()
	{
		animator = GetComponent<Animator> ();
	}


	public void OpenDoor()
	{
		if (!open) {
			open = true;
			animator.SetTrigger ("Open");
		}
	}
}
