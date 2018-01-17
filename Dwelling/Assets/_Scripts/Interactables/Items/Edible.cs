using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour {
	[SerializeField] GameObject particle;
	[SerializeField] float amount;
	public void Eat(GameObject picker)
	{
		if (particle != null)
			Instantiate (particle, transform.position, Quaternion.identity);
		EatEffect (picker);
		Destroy (gameObject);
	}

	public virtual void EatEffect(GameObject picker)
	{
		picker.GetComponent<BaseHealth> ().Heal (amount);
	}
}
