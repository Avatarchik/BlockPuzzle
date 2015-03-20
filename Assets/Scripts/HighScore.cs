using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScore : MonoBehaviour
{
	public Text text;
	static HighScore _instance;
	
	public static HighScore instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<HighScore>();
			}
			return _instance;
		}
	}

	void Awake()
	{
		text.gameObject.SetActive(false);
	}

	public void Show(string level)
	{
		text.gameObject.SetActive(true);
		if (Record.score.ContainsKey(level))
		{
			int angle = Record.score[level];
			const int max = 99999;
			if (angle < max)
			{
				text.text = angle.ToString() + " °";
			}
			else
			{
				text.text = max.ToString() + " °+";
			}
		}
		else
		{
			text.text = "";
		}
	}

	public void Hide()
	{
		text.gameObject.SetActive(false);
	}
}
