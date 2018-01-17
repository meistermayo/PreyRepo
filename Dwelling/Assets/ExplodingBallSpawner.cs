using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBallSpawner : MonoBehaviour {
	[SerializeField] Transform t1,t2;
	[SerializeField] Transform tc;
	[SerializeField] GameObject ballPrefab;
	[SerializeField] GameObject currentBall;

	void Update()
	{
		if (currentBall == null) {
			tc = (tc == t1) ? t2 : t1;
			currentBall = Instantiate (ballPrefab, tc.position, Quaternion.identity) as GameObject;

		}
	}
}
