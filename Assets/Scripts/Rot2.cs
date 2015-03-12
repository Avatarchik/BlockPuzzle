using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Rot2 : MonoBehaviour
{

	GameObject[] blocks;
	bool rotation = false;
	Transform _transform;

	Text _text;

	// Use this for initialization
	void Start()
	{
		blocks = GameObject.FindGameObjectsWithTag("Block");
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			foreach (var g in blocks)
			{
				g.GetComponent<Rigidbody2D>().gravityScale = 0;
			}
			StartCoroutine(Rotate(false));
		}
		else if (Input.GetMouseButtonDown(1))
		{
			foreach (var g in blocks)
			{
				g.GetComponent<Rigidbody2D>().gravityScale = 0;
			}
			StartCoroutine(Rotate(true));
		}
	}

	float EaseIn(float x)
	{
		return x * x;
	}

	IEnumerator Rotate(bool clockwise)
	{
		Quaternion start = _transform.localRotation;
		Quaternion end = Quaternion.Euler(0.0f, 0.0f, clockwise ? 90.0f : -90.0f) * start;
		rotation = true;
		for (int i = 1; i <= 15; i++)
		{
			float t = i / 15.0f;
			_transform.localRotation = Quaternion.Slerp(start, end, EaseIn(t));
			yield return new WaitForEndOfFrame();
		}
		rotation = false;

		foreach (var g in blocks)
		{
			g.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
		}
	}
}
