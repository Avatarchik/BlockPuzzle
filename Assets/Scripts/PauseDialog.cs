using UnityEngine;
using System.Collections;

public class PauseDialog : MonoBehaviour
{
	public Color fadeColor0Begin;
	public Color fadeColor0End;
	public Color fadeColor1Begin;
	public Color fadeColor1End;

	public Material material;

	IEnumerator _fadeIn;
	IEnumerator _fadeOut;

	bool _fading = false;

	void Start()
	{
		_fadeIn = Fade(0.5f);
		StartCoroutine(_fadeIn);
	}

	IEnumerator Fade(float d)
	{
		_fading = true;

		float elapsed = 0.0f;
		while(d > elapsed)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / d;
			Color color0 = Color.Lerp(fadeColor0Begin, fadeColor0End, t);
			Color color1 = Color.Lerp(fadeColor1Begin, fadeColor1End, t);
			material.SetColor("_Color1", color0);
			material.SetColor("_Color2", color1);
			yield return null;
		}

		_fading = false;
	}

	IEnumerator FadeIn(float d)
	{
		_fading = true;

		float elapsed = 0.0f;
		while(d > elapsed)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / d;
			Color color0 = Color.Lerp(fadeColor0End, fadeColor0Begin, t);
			Color color1 = Color.Lerp(fadeColor1End, fadeColor1Begin, t);
			material.SetColor("_Color1", color0);
			material.SetColor("_Color2", color1);
			yield return null;
		}

		_fading = false;
		gameObject.SetActive(false);
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!_fading)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					Debug.Log(hit.collider.name);
				}
				else
				{
					StartCoroutine(FadeIn(0.5f));
				}
			}
		}
	}
}
