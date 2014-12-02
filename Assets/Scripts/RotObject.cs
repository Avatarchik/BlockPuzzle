using UnityEngine;
using System.Collections;

public class RotObject : MonoBehaviour
{
	Transform _transform;

	void Awake()
	{
		_transform = transform;
		enabled = false;
	}

	void Update()
	{
		_transform.Rotate(0.0f, Time.deltaTime * 16.0f, 0.0f);
	}
}
