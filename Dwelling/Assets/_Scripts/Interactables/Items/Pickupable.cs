using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioManager))]
public class Pickupable : MonoBehaviour
{
	Rigidbody2D body;
	Collider2D collider;
	AudioManager audioManager;

	void Awake()
	{
		audioManager = GetComponent<AudioManager> ();
		collider = GetComponent<Collider2D> ();
		body = GetComponent<Rigidbody2D> ();
	}

	public void GetPickedUp(Transform picker)
	{
		audioManager.Play (0, false);
		collider.enabled = false;
		transform.position = picker.position;
		transform.parent = picker;
		body.isKinematic = true;
	}

	public void PutDown()
	{
		audioManager.Play (1, false);
		collider.enabled = true;
		transform.parent = null;
		body.isKinematic = false;
	}
}

