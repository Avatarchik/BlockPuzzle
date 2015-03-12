using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
	public int x = 0;
	public int y = 0;
	public int type = 0;

	Transform _transform;
	Animator _animator;
	bool _moving;
	Vector3 _start;

	void Awake()
	{
		_transform = transform;
		_animator = GetComponent<Animator>();
		_moving = false;
//		gameObject.SetActive(false);
		_start = _transform.position;
		if (type != Rot.StaticBlock)
		{
			var p = _start;
			p.y += 5.0f;
			_transform.position = p;
		}
	}

	void Start()
	{
//		StartCoroutine(StartAnimation());
	}

	public IEnumerator StartAnimation()
	{
		int n = 16;
		Vector3 from = _transform.position;
		for (int i = 0; i <= n; i++)
		{
			float t = (float)i / n;
			_transform.position = Vector3.Lerp(from, _start, t);
			yield return null;
		}
		yield return null;
	}

	public bool IsBusy()
	{
		return _moving;
	}

	public void Move(int down)
	{
		y -= down;
//		Vector3 p = _transform.position;
//		p.z -= down;
//		_transform.position = p;
		StartCoroutine(MoveAnimation(down));
	}

	IEnumerator MoveAnimation(int down)
	{
		_moving = true;
		Vector3 p = _transform.position;
		Vector3 q = p;
		q.z -= down;
		int n = 4 * down;
		for (int i = 0; i <= n; i++)
		{
			float t = i / (float)n;
			_transform.position = Vector3.Lerp(p, q, t * t);
			yield return null;
		}
		_moving = false;
	}

	public void Change()
	{
		StartCoroutine(ChangeAnimation());
	}

	IEnumerator ChangeAnimation()
	{
		int n = 16;
		Vector3 start = new Vector3(1.0f, 2.0f, 1.0f);
		for (int i = 1; i <= n; i++)
		{
			float t = (float)i / n;
			_transform.localScale = Vector3.Lerp(start, Vector3.zero, t);
			yield return null;
		}
		GetComponent<Renderer>().material = SystemResource.instance.materials[0];
		for (int i = 1; i <= n; i++)
		{
			float t = (float)i / n;
			_transform.localScale = Vector3.Lerp(Vector3.zero, start, t);
			yield return null;
		}
		yield return null;
	}

	IEnumerator Disappear()
	{
		yield return new WaitForSeconds(0.4f);

		Vector3 scale = Vector3.one;

		for (int i = 1; i <= 16; i++)
		{
			float t = i / 16.0f;
			_transform.localScale = Vector3.Lerp(scale, Vector3.zero, t);
			yield return null;
		}

		gameObject.SetActive(false);
		Debug.Log("Disappear");
	}
}
