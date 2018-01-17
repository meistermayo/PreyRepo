using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
	public enum EnemyState{
		PATROL=0,
		ATTACK
	}

	EnemyState state=EnemyState.PATROL;
	float side = 1f;
	bool canAttack, isAttacking;
	Rigidbody2D body;
	[SerializeField] float moveSpeed;
	[SerializeField] float minDist;

	[SerializeField] GameObject hitbox;
	[SerializeField] int attackBurst;
	[SerializeField] bool randomBurst;
	[SerializeField] float initialAttackDelay;
	[SerializeField] float attackRecovery;
	[SerializeField] Transform hitboxSpawn;

	void Start()
	{
		body = GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		transform.localScale = new Vector3 (side, 1f);
		if (state == EnemyState.PATROL)
			Patrol ();
	}

	void Patrol()
	{
		body.velocity = Vector2.right * moveSpeed;
		if (Physics.Raycast (transform.position, transform.right*side, 2f)) {
			side = -side;
		}
		if (StaticPlayerTag.playerObject.transform.position.x - transform.position.x < minDist * side) {
			StartAttack ();
		}
	}

	void StartAttack()
	{
		StopAllCoroutines ();
		StartCoroutine (Attack ());
	}

	IEnumerator Attack()
	{
		body.velocity = Vector2.zero;
		state = EnemyState.ATTACK;
		yield return new WaitForSeconds (initialAttackDelay);
		int b = attackBurst;
		if (randomBurst)
			b = Random.Range (1, attackBurst);
		for (int i = 0; i < b; i++) {
			Instantiate (hitbox, hitboxSpawn.position, Quaternion.identity);
			yield return new WaitForSeconds (attackRecovery);
		}
		state = EnemyState.PATROL;
		side = StaticPlayerTag.playerObject.transform.position.x < 0f ? -1f : 1f;
	}
}
