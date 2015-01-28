using UnityEngine;
using System.Collections;

public class PauseDialog : MonoBehaviour
{
	public Color fadeColor1End;
	public Color fadeColor2End;
	
	Color _fadeColor1Begin;
	Color _fadeColor2Begin;

	IEnumerator _fadeIn;
	IEnumerator _fadeOut;

	bool _fading = false;
	int _layerMask = 0;

	RadialGrad _radialGrad;

	void Awake()
	{
		_layerMask = 1 << LayerMask.NameToLayer("UI");
		_radialGrad = GameObject.FindObjectOfType<RadialGrad>();
		_fadeColor1Begin = _radialGrad.color1;
		_fadeColor2Begin = _radialGrad.color2;
	}

	void OnEnable()
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
			Color color1 = Color.Lerp(_fadeColor1Begin, fadeColor1End, t);
			Color color2 = Color.Lerp(_fadeColor2Begin, fadeColor2End, t);
			_radialGrad.color1 = color1;
			_radialGrad.color2 = color2;
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
			Color color1 = Color.Lerp(fadeColor1End, _fadeColor1Begin, t);
			Color color2 = Color.Lerp(fadeColor2End, _fadeColor2Begin, t);
			_radialGrad.color1 = color1;
			_radialGrad.color2 = color2;
			yield return null;
		}

		_fading = false;
		gameObject.SetActive(false);
		if (SceneManager.instance != null)
		{
			SceneManager.instance.Resume();
		}
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!_fading)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100.0f, _layerMask))
				{
					if (hit.collider.tag == "Reset")
					{
						Application.LoadLevel(Application.loadedLevel);
					}
					else if (hit.collider.tag == "Title")
					{
						Application.LoadLevel("Title");
					}
				}
				else
				{
					StartCoroutine(FadeIn(0.5f));
					GetComponent<Animator>().SetTrigger("Hide");
				}
			}
		}

		if (!_fading && Input.GetKeyDown(KeyCode.Escape))
		{
			StartCoroutine(FadeIn(0.5f));
			GetComponent<Animator>().SetTrigger("Hide");
		}
	}
}
