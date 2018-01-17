using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : HitboxController_Enemy {
	[SerializeField] protected float moveSpeed;

	protected override void Start()
	{
		GetComponent<Rigidbody2D> ().velocity = Vector2.right * moveSpeed;
		base.Start ();
	}

}
