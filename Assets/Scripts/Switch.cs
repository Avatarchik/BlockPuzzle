using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void On()
	{
		Camera.main.backgroundColor = Color.white;
	}

	public void Off()
	{
		Camera.main.backgroundColor = Color.black;
	}
}
