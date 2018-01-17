using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : BaseHealth
{
	AudioManager audioManager;
	SpriteRenderer[] sprites;
	bool invincible;
	public override void Start ()
	{
		sprites = GetComponentsInChildren<SpriteRenderer> ();
		audioManager = GetComponent<AudioManager> ();
		base.Start ();
	}

	public override void TakeDamage (float damage)
	{
		audioManager.Play (SFX_SOLOMON.HURT);
		base.TakeDamage (damage);
		PlayerUI.SetHealthUI (health, maxHealth);
	}

	public virtual void TakeDamage(float damage,Vector2 launchVec)
	{
		if (invincible)
			return;
		GetComponent<Rigidbody2D> ().velocity = launchVec;
		audioManager.Play (SFX_SOLOMON.HURT);
		base.TakeDamage (damage);
		PlayerUI.SetHealthUI (health, maxHealth);
		GetComponentInChildren<Animator> ().SetTrigger ("Hurt");
		StartInvincibility ();
	}

	public override void Die ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void StartInvincibility()
	{
		StopAllCoroutines ();
		StartCoroutine (Invincibility ());
	}

	IEnumerator Invincibility()
	{
		invincible = true;
		for (int i = 0; i > 120; i++) {
			for (int j = 0; j < sprites.Length; j++) {
				sprites [j].enabled = !sprites [j].enabled;
			}
			yield return null;
		}
		invincible = false;
	}
}

