using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth{
	[SerializeField] GameObject meatPrefab;
	[SerializeField] int amountOfMeat;
	[SerializeField] float spawnVariance;
	public override void Die ()
	{
		for (int i = 0; i < amountOfMeat; i++) {
			Instantiate (meatPrefab, transform.position + new Vector3 (Random.value, Random.value) * spawnVariance, Quaternion.identity);
		}
		Destroy (gameObject);
	}
}
