using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
	public int level = 0;

	int _levelCount = 0;
	bool _scrolling = false;
	RotObject _selectedLevel = null;

	static int selectedLevel = 0;

	void Awake()
	{
		if (selectedLevel > 0)
		{
			Vector3 p = transform.position;
			p.x = 7.0f * selectedLevel;
			transform.position = p;
		}

		var blocks = GameObject.FindGameObjectsWithTag("Block");
		if (blocks != null)
		{
			_levelCount = blocks.Length;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (!_scrolling)
		{
			if (Input.GetMouseButtonDown(0))
			{
				int halfWidth = Screen.width / 2;
				Vector3 pos = Input.mousePosition;

				if (_selectedLevel != null)
				{
					_selectedLevel.enabled = false;
					HighScore.instance.Hide();
				}

				if (pos.x < halfWidth / 4)
				{
					if (selectedLevel > 0)
					{
						selectedLevel--;
						StartCoroutine(Scroll(0.5f, -7.0f, EaseInOut));
					}
					else
					{
						StartCoroutine(Scroll(0.3f, -0.5f, Convex));
					}
				}
				else if (pos.x > Screen.width - halfWidth / 4)
				{
					if (selectedLevel < _levelCount / 3 - 1)
					{
						selectedLevel++;
						StartCoroutine(Scroll(0.5f, 7.0f, EaseInOut));
					}
					else
					{
						StartCoroutine(Scroll(0.3f, 0.5f, Convex));
					}
				}
				else
				{
					Ray ray = Camera.main.ScreenPointToRay(pos);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.collider.name.Contains("Level"))
						{
							var ro = hit.collider.GetComponent<RotObject>();
							if (_selectedLevel == ro)
							{
								Application.LoadLevel(_selectedLevel.name);
							}
							_selectedLevel = ro;
							_selectedLevel.enabled = true;
							HighScore.instance.Show(ro.name);
						}
					}
				}
			}
		}
	}

	float EaseInOut(float t)
	{
		return 3.0f * t * t - 2.0f * t * t * t;
	}

	float Convex(float t)
	{
		return 0.5f * Mathf.Sin((2.0f * t - 0.5f) * Mathf.PI) + 0.5f;
	}

	delegate float Ease(float t);

	IEnumerator Scroll(float d, float x, Ease f)
	{
		_scrolling = true;
		Transform trans = transform;
		Vector3 start = trans.position;
		Vector3 end = start;
		end.x += x;
		
		float elapsed = 0.0f;
		while(d > elapsed)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / d;
			trans.position = Vector3.Lerp(start, end, f(t));
			yield return null;
		}
		
		_scrolling = false;
	}
}
