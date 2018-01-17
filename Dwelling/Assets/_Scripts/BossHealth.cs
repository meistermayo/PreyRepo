using UnityEngine;
using System.Collections;

public class BossHealth : EnemyHealth
{
	Animator animator;
	public override void Start ()
	{
		animator = GetComponent<Animator> ();
		base.Start ();
	}

	public override void TakeDamage (float damage)
	{
		animator.SetBool ("Hurt",true);
		animator.SetTrigger("StartHurt");
		base.TakeDamage (damage);
	}
}

