using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	#region serialized variables
	[Header("Basic Movement")]
	[SerializeField] float moveSpeed;
	[SerializeField] float airMoveSpeed;
	[SerializeField] float moveSpeedMax;
	[SerializeField] float jumpSpeed;
	[SerializeField] float fallSpeedMax;

	[Header("Wall Movement")]
	[SerializeField] float wall_jumpSpeed;
	[SerializeField] float wall_moveSpeed;

	[Header("Combat")]
	[SerializeField] float biteRange;
	[SerializeField] float biteDamage;
	#endregion

	Rigidbody2D body;
	AudioManager audioManager;
	bool input_active=true;
	bool captured;

	float h,v;
	bool jump_down, jump_held,
		 bite_down, bite_held;

	bool canJump, canCling = true;
	bool isClinging;
	bool facingRight=true;
	bool isHolding;
	Pickupable heldItem;
	[SerializeField] LayerMask groundLayer;
	[SerializeField] Transform itemHolder;
	TrapController captor;
	Bodyanimator bodyAnimator;
	Animator animator;
	void Awake()
	{
		animator = GetComponentInChildren<Animator> ();
		bodyAnimator = GetComponent<Bodyanimator> ();
		Collider2D[] colliders = GetComponentsInChildren<Collider2D> (); // disable me with them
		foreach (Collider2D collider in colliders)
		{
			Physics2D.IgnoreCollision (GetComponent<Collider2D> (), collider);
		}

		foreach (Collider2D collider in colliders) { // disable them with them
			foreach (Collider2D otherCollider in colliders) {
				Physics2D.IgnoreCollision (collider, otherCollider);
			}
		}

		audioManager = GetComponent<AudioManager> ();
		body = GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		if (input_active) {
			GetInput ();
			if (captured) {
				EscapeBite ();
			} else {
				Walk ();
				Jump ();
				Bite ();
			}
		}
	}

	void FixedUpdate()
	{
	}

	void GetInput()
	{
		h = Input.GetKey (KeyCode.LeftArrow) ? -1.0f : 0.0f;
		h += Input.GetKey (KeyCode.RightArrow) ? 1.0f : 0.0f; // optimal?... probably not... do we care?... no.
		v = Input.GetKey (KeyCode.UpArrow) ? 1.0f : 0.0f;	// up is no longer negative :(
		v += Input.GetKey (KeyCode.DownArrow) ? -1.0f : 0.0f; 

		jump_down = Input.GetKeyDown (KeyCode.Space);
		jump_held = Input.GetKey (KeyCode.Space);
		bite_down = Input.GetKeyDown (KeyCode.Z);
		bite_held = Input.GetKey (KeyCode.Z);
	}

	void Walk()
	{
		bodyAnimator.FlipChest (h);
		if (h != 0f)
			facingRight = h > 0f;
		RaycastHit2D hit = SideBoxCast (8);
		if (hit) {
			if (!canJump)
				body.velocity = new Vector2 (0.0f, body.velocity.y);
		} else {
			body.velocity += Vector2.right * h * moveSpeed;
			if (Mathf.Abs (body.velocity.x) > moveSpeedMax) {
				body.velocity = new Vector2 (body.velocity.x * .9f, body.velocity.y);//new Vector2 (Mathf.Sign (body.velocity.x) * moveSpeedMax, body.velocity.y);
			}
		}


	}

	void Jump()
	{
		CheckGround ();
		if (jump_held) {
			//CheckGround ();
			if (canJump) {
				animator.SetTrigger ("Jump");
				canJump = false;
				body.velocity = new Vector2 (body.velocity.x, jumpSpeed);
				audioManager.Play (SFX_SOLOMON.JUMP, false);
			}
		}
		if (body.velocity.y < -fallSpeedMax) {
			body.velocity = new Vector2 (body.velocity.x, -fallSpeedMax);
		}
		if (!jump_held) {
			// TODO...noraml gravity?...
		}
	}

	void Bite()
	{
		if (isHolding) {
			HoldItem ();
			return;
		}
		if (bite_down) {
			animator.SetTrigger ("Bite");
			RaycastHit2D hit = CheckSideRaycast(biteRange);
			if (hit) {
				Debug.Log (hit.collider.name);
				if (CheckForPickup (hit)) // if we find an object we don't want any other action.
					return;
				if (CheckForEdible (hit))
					return;
				if (CheckForDoor (hit))
					return;
				if (CheckForBiteAttack (hit))
					return;
				if (canCling) {
					animator.SetBool ("IsClinging", true);
					audioManager.Play (SFX_SOLOMON.WALLBITE, false);
					isClinging = true;
					canCling = false;
				}
			} else {
				audioManager.Play (SFX_SOLOMON.BITE, false);
			}
		}
		if (isClinging)
			Cling ();
	}

	void Cling()
	{
		body.velocity = Vector2.zero;
		if (jump_held)
			WallJump ();
		if (!bite_held) {
			isClinging = false;
			animator.SetBool ("IsClinging", false);
		}
	}

	void WallJump()
	{
		animator.SetTrigger ("Jump");
		animator.SetBool ("IsGrounded", false);
		audioManager.Play (SFX_SOLOMON.WALLJUMP, false);
		isClinging = false;
		animator.SetBool ("IsClinging", false);
		body.velocity = Vector3.up * wall_jumpSpeed + Vector3.right * wall_moveSpeed * h;
	}

	void HoldItem()
	{
		if (!bite_held) {
			isHolding = false;
			heldItem.PutDown ();
			heldItem.GetComponent<Rigidbody2D> ().velocity = body.velocity;
		}
	}
	

	void CheckGround()
	{
		if (Physics2D.OverlapCircle (transform.position + Vector3.down * GetComponent<BoxCollider2D> ().bounds.extents.y, 0.3f, groundLayer) && body.velocity.y >= 0f) {
			canJump = true;
			canCling = true;
			animator.SetBool ("IsGrounded", true);
		} else {
			animator.SetBool ("IsGrounded", false);
			canJump = false;
		}
	}
	
	RaycastHit2D CheckSideRaycast(float dist)
	{
		return Physics2D.Raycast (transform.position, (facingRight ? transform.right : -transform.right), dist, groundLayer);
	}

	RaycastHit2D CheckDown(float dist)
	{
		return Physics2D.Raycast (transform.position, Vector2.down, dist,groundLayer);
	}
	void CheckSlopeRising()
	{
		// TODO
	}

	void CheckSlopeFalling()
	{
		// TODO
	}

	RaycastHit2D SideBoxCast(int layer)
	{
		Collider2D col = GetComponent<Collider2D> ();
		return Physics2D.BoxCast (col.bounds.center + Vector3.up * .1f + Vector3.down * .2f, (col.bounds.extents*2f) + (Vector3.down * .4f), 0, transform.right * h,.1f,groundLayer);
	}

	bool CheckForPickup(RaycastHit2D hit)
	{
		Pickupable pickupable = hit.collider.GetComponent<Pickupable> ();

		if (pickupable != null) {
			isHolding = true;
			pickupable.GetPickedUp (itemHolder);
			heldItem = pickupable;
			return true;
		}

		hit = CheckDown (biteRange);
		if (hit != null && hit.collider != null) {
			pickupable = hit.collider.GetComponent<Pickupable> ();

			if (pickupable != null) {
				isHolding = true;
				pickupable.GetPickedUp (itemHolder);
				heldItem = pickupable;
				return true;
			}
			return false;
		}return false;
	}


	bool CheckForEdible(RaycastHit2D hit)
	{
		Edible edible = hit.collider.GetComponent<Edible> ();

		if (edible != null) {
			edible.Eat (this.gameObject);
			return true;
		}
		return false;
	}

	public void SetInputActive(bool active)
	{
		input_active = active;
	}

	void EscapeBite()
	{
		if (bite_down) {
			audioManager.Play (SFX_SOLOMON.BITE, false);
			if (captor != null)
				captor.ReceiveBite ();
		}
	}

	public void Capture(TrapController captor)
	{
		this.captor = captor;
		captured = true;
		body.isKinematic = true;
		body.velocity = Vector2.zero;
	}

	public void Escape(Vector2 launch)
	{
		this.captor = null;
		captured = false;
		body.isKinematic = false;
		body.velocity = launch;
	}

	bool CheckForDoor(RaycastHit2D hit)
	{
		DoorController door = hit.collider.GetComponent<DoorController> ();

		if (door != null)
			door.OpenDoor ();
		
		return (door != null);
	}

	bool CheckForBiteAttack(RaycastHit2D hit)
	{
		if (hit.collider.CompareTag("Enemy"))
		{
			hit.collider.GetComponent<BaseHealth> ().TakeDamage(biteDamage);
			return true;
		}
		return false;
	}
/*
TODO::::
Hinge joints!?!!?!?!?!!? Anitonshuns?!!?!?!?!?
Jumping
Jump held
Walking Correctness
Slope climbing
Biting/Walljumping
*/
}
