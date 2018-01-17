using UnityEngine;
using System.Collections;

public class HitboxController_Enemy : MonoBehaviour
{
	[SerializeField] protected float damage;
	[SerializeField] protected float lifeTime;
	[SerializeField] protected Vector2 launchVec;
	[SerializeField] bool transcendant;
	protected virtual void Start()
	{
		Destroy (gameObject, lifeTime);
	}

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Player")) {
			if (launchVec == Vector2.zero)
				other.GetComponent<PlayerHealth> ().TakeDamage (damage);
			else
				other.GetComponent<PlayerHealth> ().TakeDamage (damage,new Vector2(launchVec.x*Mathf.Sign(other.transform.position.x-transform.position.x),launchVec.y));
			
			Destroy (gameObject);
		} else if (!transcendant && other.CompareTag ("Terrain")) {
			Destroy (gameObject);
		}
	}
}

