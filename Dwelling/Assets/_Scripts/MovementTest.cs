using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour {
	[SerializeField] float moveSpeed;
	[SerializeField] float jumpSpeed;
	Rigidbody2D body;
	float movement;

	void Start()
	{
		body = GetComponent<Rigidbody2D> ();
		StartCoroutine (TestMovemement ());
	}

	void Update()
	{
		body.velocity = Vector2.up * body.velocity.y + Vector2.right * movement;
	}

	IEnumerator TestMovemement()
	{
		while (true) {
			float sign = (Random.value > .5f) ? 1f : -1f;
			movement = moveSpeed * sign;
			GetComponent<Bodyanimator> ().FlipChest (sign);
			yield return new WaitForSeconds (Random.Range (.25f, 5f));
		}
	}
}
