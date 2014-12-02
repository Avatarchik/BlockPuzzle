using UnityEngine;
using System.Collections;

public class TextAnimation : MonoBehaviour
{
	public Animator[] chars;

	void Awake()
	{
		chars = transform.GetComponentsInChildren<Animator>();
		foreach (var e in chars)
		{
			e.gameObject.SetActive(false);
		}
	}

	IEnumerator PlayAnimation()
	{
		for (int i = 0; i < chars.Length; i++)
		{
			chars[i].gameObject.SetActive(true);
			chars[i].SetTrigger("Appear");
			yield return new WaitForSeconds(0.2f);
		}
	}

	void Start()
	{
		StartCoroutine(PlayAnimation());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
