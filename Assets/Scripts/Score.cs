using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour
{
	public Text text;
	public int angle = 0;

	static Score _instance;

	Animator _animator;

	public static Score instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<Score>();
			}
			return _instance;
		}
	}

	void Awake()
	{
		angle = 0;
		text.text = "0 °";
		_animator = text.GetComponent<Animator>();
	}

	public void Add()
	{
		const int max = 99999;
		angle += 90;
		if (angle < max)
		{
			text.text = angle.ToString() + " °";
		}
		else
		{
			text.text = max.ToString() + " °+";
		}
		_animator.SetTrigger("Add");
	}

	public void Reset()
	{
		angle = 0;
		text.text = angle.ToString() + " °";
	}
}
