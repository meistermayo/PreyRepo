using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour {
	[SerializeField] Transform targetTransform;
	[SerializeField] Vector3 offset;
	[SerializeField] float lerp;
	float zStart;

	void Awake()
	{
		zStart = transform.position.z;
	}

	void FixedUpdate()
	{
		offset.z = zStart;
		transform.position = Vector3.Lerp (transform.position, targetTransform.position + offset, lerp);
	}

	public void SetCamera(Transform newTransform, float newSize, float newLerp)
	{
		targetTransform = newTransform;
		Camera.main.orthographicSize = newSize;
		lerp = newLerp;
	}

	public void SetCamera(Transform newTransform, float newSize)
	{
		targetTransform = newTransform;
		Camera.main.orthographicSize = newSize;
	}

}
