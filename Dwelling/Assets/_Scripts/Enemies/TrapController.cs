using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour {
	static Animator cameraAnimator;
	PlayerMovement playerMovement;
	Collider2D collider;

	[SerializeField] Vector2 launchDir;
	[SerializeField] float biteValue;
	[SerializeField] float biteRate;
	public float BiteValue { get { return biteValue; } }
	float escapeValue;
	bool hasCaptured;

	void Awake()
	{
		collider = GetComponent<Collider2D>();
		cameraAnimator = Camera.main.GetComponent<Animator> ();
	}

	void Update()
	{
		if (hasCaptured)
			CheckEscape ();	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		playerMovement = other.GetComponent<PlayerMovement> ();
		if (playerMovement == null)
			return;
		hasCaptured = true;
		playerMovement.Capture (this);
		cameraAnimator.SetBool ("captureFlyTrap", true);
	}

	void CheckEscape()
	{
		if (escapeValue > 100f) {
			hasCaptured = false;
			playerMovement.Escape(new Vector2(launchDir.x * transform.lossyScale.x, launchDir.y));
			cameraAnimator.SetBool ("captureFlyTrap", false);
			collider.enabled = false;
			escapeValue = 0f;
		} else 
		escapeValue -= biteRate;
	}

	public void ReceiveBite()
	{
		escapeValue += biteValue;
	}
}
