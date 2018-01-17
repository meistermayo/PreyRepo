using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossHitbox{
	public GameObject prefab;
	public Transform spawnLocation;
}

public enum BOSS_STATES{
	IDLE = 0,
	APPROACH,
	ATTACK,
	ATTACK_CLOSE,
	ATTACK_FAR,
	JUMP,
	EVADE
}

public class BossController : MonoBehaviour {
	[SerializeField] BossHitbox[] hitboxes;
	[SerializeField] float maxDist;
	[SerializeField] float maxHeight;
	[SerializeField] float minDist;
	[SerializeField] Transform grappleTransform;
	[SerializeField] float[] grappleDamage;

	List<GameObject> currentHitboxes;
	BaseHealth healthScript;
	int bossStage;
	bool useOnce;
	Rigidbody2D body;
	Animator animator;

	bool moving;
	bool inIntro=true;
	bool inAttack = false;
	bool inGrapple=false;
	#region UnityCode
	void Start()
	{
		currentHitboxes = new List<GameObject>(10);
		body = GetComponent<Rigidbody2D> ();
		healthScript = GetComponent<BaseHealth> ();
		animator = GetComponent<Animator> ();
	}

	void Update()
	{
		// Do boss stuff
		if (!inAttack) {
			animator.SetFloat ("Height", (StaticPlayerTag.playerObject.transform.position.y - transform.position.y) / maxHeight);
			animator.SetBool ("TooHigh", Mathf.Abs (StaticPlayerTag.playerObject.transform.position.y - transform.position.y) > maxHeight / 2f);//&& Mathf.Abs(StaticPlayerTag.playerObject.transform.position.x - transform.position.x) < minDist);
		}
		if (!inIntro) {
			if (!moving && Mathf.Abs(transform.position.x-StaticPlayerTag.playerObject.transform.position.x) > maxDist) {
				moving = true;
				animator.SetTrigger ("Move");
			}
		}

		for (int i = 0; i < currentHitboxes.Count; i++) {
			if (currentHitboxes [i] == null)
				currentHitboxes.RemoveAt (i);
		}
		if (!inGrapple) {
			float diff = StaticPlayerTag.playerObject.transform.position.x - transform.position.x;
			if (diff != 0f)
				transform.localScale = new Vector3 (Mathf.Sign (diff), 1f, 1f);
		}
	}
	#endregion

	#region Boss State Code
	protected virtual void Idle()
	{
		// Do Nothing
	}

	protected virtual void Approach()
	{
		// if not too far or too close
		// Walk forward
	}

	protected virtual void Attack()
	{
		// use once
		// decide between attack close and far?
	}

	protected virtual void Attack_Close()
	{
		// use once (or not ^)
	}

	protected virtual void Attack_Far()
	{
		// use once (or not ^^)
	}

	protected virtual void Jump()
	{
		// if too far
		// use once 
	}

	protected virtual void Evade()
	{
		//if too close
	}

	protected virtual IEnumerator UseOnce(float seconds)
	{
		useOnce = false;
		yield return new WaitForSeconds (seconds);
		useOnce = true;
	}
	#endregion

	#region Hitbox Code
	public void CreateHitbox(int i)
	{
		if (i > -1 && i < hitboxes.Length) {
			currentHitboxes.Add( Instantiate (hitboxes [i].prefab, hitboxes [i].spawnLocation) as GameObject );
		}
	}

	public void DeleteAllHitBoxes()
	{
		for (int i = 0; i < currentHitboxes.Count; i++) {
			Destroy (currentHitboxes [i]);
		}
		currentHitboxes.Clear ();
	}

	public void DeacSpecificHitBox(int i)
	{
		if (currentHitboxes.Count > 0) {
			if (currentHitboxes [i] != null) {
				currentHitboxes [i].GetComponent<Collider2D> ().enabled = false;
				currentHitboxes.RemoveAt (i);
			}
		}
	}
	#endregion

	public void SetVelX(float x)
	{
		body.velocity = Vector2.right * x * transform.localScale.x;
	}

	public void StopMoving()
	{
		moving = false;
	}

	public void EndIntro()
	{
		inIntro = false;
	}

	public void StartAttack()
	{
		inAttack = true;
	}

	public void EndAttack()
	{
		inAttack = false;
	}

	public void StopBeingHurt()
	{
		animator.SetBool ("Hurt", false);
	}

	public void Grapple()
	{
		inGrapple = true;
		animator.SetTrigger ("Grapple");
		StaticPlayerTag.playerObject.GetComponent<PlayerMovement> ().Capture (null);
		StaticPlayerTag.playerObject.transform.parent = grappleTransform;
		StaticPlayerTag.playerObject.transform.localPosition = Vector3.zero;
	}

	public void EndGrapple()
	{
		inGrapple = false;
		StaticPlayerTag.playerObject.GetComponent<PlayerMovement> ().Escape (new Vector2(transform.localScale.x*20f,-3f));
		StaticPlayerTag.playerObject.transform.parent = null;
		StaticPlayerTag.playerObject.transform.rotation = Quaternion.identity;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!inIntro) {
			if (other.collider.tag == "Player" && !inGrapple) {	
				Grapple ();
			}
		}
	}

	public void GrappleDamage(int index)
	{
		StaticPlayerTag.playerObject.GetComponent<PlayerHealth> ().TakeDamage (grappleDamage[index]);
	}
}
