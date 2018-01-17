using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour {
	[SerializeField] Text healthTextRef;
	static Text healthText;

	void Start()
	{
		healthText = healthTextRef;
	}

	public static void SetHealthUI(float health, float maxHealth)
	{
		healthText.text = Mathf.Round (health).ToString() + "/" + maxHealth.ToString ();
	}
}
