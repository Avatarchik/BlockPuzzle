using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Record
{

	static public Dictionary<string, int> score = new Dictionary<string, int>();

	static public void Load()
	{
		var levels = GameObject.FindGameObjectsWithTag("Block");
		foreach (var g in levels)
		{
			score.Add(g.name, PlayerPrefs.GetInt(g.name));
		}
	}

	static public void Save()
	{
		foreach (var e in score)
		{
			PlayerPrefs.SetInt(e.Key, e.Value);
		}
	}

	static public void AddScore(int s)
	{
		string level = Application.loadedLevelName;
		if (score.ContainsKey(level))
		{
			if (score[level] > s)
			{
				score[level] = s;
			}
		}
		else
		{
			score.Add(level, s);
		}
	}
}
