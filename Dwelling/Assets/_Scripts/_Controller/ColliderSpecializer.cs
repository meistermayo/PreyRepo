﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpecializer : MonoBehaviour {
	[SerializeField] Collider2D specialCollider;

	// Use this for initialization
	void Start () {
		Physics2D.IgnoreLayerCollision (8,9);
	}

}
