using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioManager))]
public class Launcher : MonoBehaviour {
	AudioManager audioManager;
	[SerializeField] Vector2 direction;
	[SerializeField] float magnitude;
	[SerializeField] float maxSpeed;

	void Awake()
	{
		audioManager = GetComponent<AudioManager> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Rigidbody2D body = other.GetComponent<Rigidbody2D> ();
		if (body != null) {
			if (body.position.y < GetComponent<Collider2D> ().bounds.center.y || body.velocity.y >= 0f)
				return;
			audioManager.SetPitch (-body.velocity.y/20f);
			audioManager.Play (0, false);
			body.velocity = new Vector2(body.velocity.x,Mathf.Clamp(-body.velocity.y,1f,maxSpeed));
			body.AddForce (direction*magnitude);
		}
	}
}
