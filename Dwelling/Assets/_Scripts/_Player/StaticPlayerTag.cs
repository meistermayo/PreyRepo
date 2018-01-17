using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticPlayerTag : MonoBehaviour {
	public static GameObject playerObject;
	public static PlayerMovement playerMovement;

	void Start()
	{
		playerObject = gameObject;
		playerMovement = GetComponent<PlayerMovement> ();
	}

	public static void ResetScene()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
