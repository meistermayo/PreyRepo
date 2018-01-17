using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour {

	[SerializeField] protected float damage;

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Enemy")) {
			//if (other.GetComponent<BossHealth>() != null)
				//other.GetComponent<BossHealth> ().TakeDamage (damage);
			//else
				other.GetComponent<EnemyHealth> ().TakeDamage (damage);
			Destroy (gameObject);
		} else if (other.CompareTag ("Terrain")) {
		//	Destroy (gameObject);
		}
	}
}
