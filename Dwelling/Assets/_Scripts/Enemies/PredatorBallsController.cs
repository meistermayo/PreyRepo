using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorBallsController : MonoBehaviour {

	Transform[] balls;
	[SerializeField] float offset;
	[SerializeField] float increment;
	[SerializeField] float rate;
	[SerializeField] float depth;
	[SerializeField] Transform head;
	float angle;
	void Awake()
	{
		balls = GetComponentsInChildren<Transform> ();
	}

	void Update()
	{
		UpdateBalls ();	
	}

	void UpdateBalls()
	{
		angle += offset;
		for (int i = 0; i < balls.Length; i++) {
			balls [i].position = transform.position + (head.position - transform.position) * i / balls.Length + Vector3.up * Mathf.Sin((angle + (i * increment))/rate)*depth;
		}
	}
}
