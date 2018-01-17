using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour {
	[SerializeField] protected float maxHealth;
	protected float health;

	public float MaxHealth{ get { return maxHealth; } }
	public float Health {get {return health;}}

	public virtual void Start()
	{
		health = maxHealth;
	}

	public virtual void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0f)
			Die ();
	}

	public virtual void Die()
	{
		Destroy (gameObject);
	}

	public virtual void Heal(float heal)
	{
		health += heal;
		if (health > maxHealth)
			health = maxHealth;
	}
}
