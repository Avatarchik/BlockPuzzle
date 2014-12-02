using UnityEngine;
using System.Collections;

public class Tweet : MonoBehaviour
{
	public GameObject countdown;

	void OnMouseDown()
	{
		countdown.SendMessage("Tweet");
	}
}
