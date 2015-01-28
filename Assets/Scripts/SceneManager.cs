using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
	static SceneManager _instance;
	
	public static SceneManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<SceneManager>();
			}
			return _instance;
		}
	}

	Rot _rot;
	GameObject _successText;
	bool _isPause;
	GameObject _pauseDialog;

	void Awake()
	{
		_successText = GameObject.Find("Success");
		_successText.SetActive(false);
		_pauseDialog = GameObject.Find("PauseDialog");
		_pauseDialog.SetActive(false);
		_isPause = false;
	}

	void Start()
	{
		_rot = GameObject.Find("Box").GetComponent<Rot>();
		_rot.enabled = false;

		StartCoroutine(StartAnimation());
	}

	/// <summary>
	/// 開始アニメ@ション
	/// </summary>
	IEnumerator StartAnimation()
	{
		var blocks = GameObject.FindGameObjectsWithTag("Block");

		foreach (var g in blocks)
		{
			var b = g.GetComponent<Block>();
			if (b.type != Rot.StaticBlock)
			{
				StartCoroutine(b.StartAnimation());
				yield return new WaitForSeconds(0.2f);
			}
		}

//		yield return new WaitForSeconds(1.0f);

		_rot.enabled = true;
	}

	public void Finish()
	{
		StartCoroutine(FinishAnimation());
		Record.AddScore(Score.instance.angle);
	}

	/// <summary>
	/// 終了アニメーション
	/// </summary>
	/// <returns>The animation.</returns>
	IEnumerator FinishAnimation()
	{
		_rot.enabled = false;
		_isPause = true;

		yield return new WaitForSeconds(1.0f);

		_successText.SetActive(true);

		while(!Input.GetMouseButtonDown(0))
		{
			yield return null;
		}
		
		yield return new WaitForSeconds(1.0f);

		Application.LoadLevel("Title");
	}
	
	void Update()
	{
		if (!_isPause && Input.GetKeyDown(KeyCode.Escape))
		{
//			Application.LoadLevel("Title");
//			Application.LoadLevel(Application.loadedLevel);
			Pause();
		}
	}

	/// <summary>
	/// ポーズ
	/// </summary>
	void Pause()
	{
		_isPause = true;
		_rot.enabled = false;
		_pauseDialog.SetActive(true);
	}

	public void Resume()
	{
		_isPause = false;
		_rot.enabled = true;
		_pauseDialog.SetActive(false);
	}

}
