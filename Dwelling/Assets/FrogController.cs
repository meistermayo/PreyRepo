using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour {
	[SerializeField] float jumpSpeed;
	[SerializeField] float moveSpeed;
	[SerializeField] bool startLeft;
	Rigidbody2D body;

	void Start()
	{
		if (startLeft)
			transform.localScale = new Vector3 (-1f, 1f, 1f);
		body = GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		RaycastHit2D hit = Physics2D.Raycast (transform.position + transform.right * transform.localScale.x*2f, transform.right * transform.localScale.x, 1f,LayerMask.NameToLayer("Jumpable"));
		if (hit.collider != null) {
			transform.localScale = new Vector3 (-transform.localScale.x, 1f, 1f);
		}
	}

	public void Jump()
	{
		body.velocity = new Vector2 (moveSpeed* transform.localScale.x, jumpSpeed);
	}

	public void Attack()
	{
	}
}
