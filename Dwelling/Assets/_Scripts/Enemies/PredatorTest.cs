using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PREDATOR_STATE{
	DORMANT = 0,
	CHASING,
	CAPTURE,
	RETURN
}
public class PredatorTest : MonoBehaviour {

	[SerializeField] float moveSpeed;
	[SerializeField] AudioClip music;
	AudioClip saveMusic;
	[SerializeField] float minAwakenDist;
	[SerializeField] float minCaptureDist;
	[SerializeField] Transform handTransform;
	Animator animator;
	AudioManager audioManager;
	bool awoken;
	bool isChasing;
	Vector3 startingPos;
	PREDATOR_STATE predator_state;

	void Start()
	{
		startingPos = transform.position;
		audioManager = GetComponent<AudioManager> ();
		animator = GetComponent<Animator> ();
	}

	void Update()
	{
		if (predator_state == PREDATOR_STATE.DORMANT)
			WakeUpPredator ();
		else if (predator_state == PREDATOR_STATE.CHASING)
			PredatorChase ();
		else if (predator_state == PREDATOR_STATE.CAPTURE)
			Capture ();
		else if (predator_state == PREDATOR_STATE.RETURN)
			PredatorGoBack ();
	}

	void WakeUpPredator()
	{
		if (awoken)
			return;
		if (Vector3.Distance (transform.position, StaticPlayerTag.playerObject.transform.position) < minAwakenDist && StaticPlayerTag.playerObject.transform.position.y >= transform.position.y-3f) {
			animator.SetTrigger ("Awake");
			saveMusic = BGM_Controller.GetClip ();
			BGM_Controller.PlayMusic (music, false);
			audioManager.Play (Random.Range (0, 3), false);
			awoken = true;
		}
	}

	void PredatorChase()
	{
		if (!isChasing)
			return;
		float sign = Mathf.Sign (StaticPlayerTag.playerObject.transform.position.x - transform.position.x);
		transform.position +=  Vector3.right * sign * moveSpeed;
		transform.localScale = new Vector3 (sign,1f,1f);

		if (Vector3.Distance (transform.position, StaticPlayerTag.playerObject.transform.position) < minCaptureDist) {
			isChasing = false;
			animator.SetTrigger ("Capture");
			predator_state = PREDATOR_STATE.CAPTURE;
			StaticPlayerTag.playerObject.transform.parent = handTransform;
			StaticPlayerTag.playerObject.transform.localPosition = new Vector3(1.8f,2.3f,0f);
			StaticPlayerTag.playerObject.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,85f));
			StaticPlayerTag.playerObject.transform.localScale = new Vector3 (-1f,-1f,1f);
			StaticPlayerTag.playerMovement.SetInputActive (false);
		}
		if (Mathf.Abs(StaticPlayerTag.playerObject.transform.position.y - transform.position.y) > 5f && StaticPlayerTag.playerObject.transform.position.y < transform.position.y) {
			predator_state = PREDATOR_STATE.RETURN;
			isChasing = false;
		}
	}

	void PredatorGoBack()
	{
		float sign = Mathf.Sign (startingPos.x - transform.position.x);
		transform.position +=  Vector3.right * sign * moveSpeed;
		transform.localScale = new Vector3 (sign,1f,1f);
		if (Vector3.Distance (transform.position, startingPos) < minCaptureDist) {
			PredatorSleep ();
		}
	}

	void PredatorSleep()
	{
		animator.SetTrigger ("Sleep");
		predator_state = PREDATOR_STATE.DORMANT;
		awoken = false;
		if (awoken)
			BGM_Controller.PlayMusic (saveMusic, false);
	}

	void Capture()
	{
	}

	public void StartChasing()
	{
		predator_state = PREDATOR_STATE.CHASING;
		isChasing = true;
	}

	public void StartCameraCaptureMovement()
	{
		Animator camAnimator = Camera.main.GetComponent<Animator> ();
		camAnimator.SetBool ("capturePredator",true);
	}

	public void KillPlayer()
	{
		StaticPlayerTag.ResetScene ();
	}
}
