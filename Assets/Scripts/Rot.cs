using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SceneManager))]
public class Rot : MonoBehaviour
{
	public const int StaticBlock = 5;

	public enum Rule
	{
		Fixed,
		Switch
	}

	public Rule rule = Rule.Fixed;

	public int cols = 3;
	public int rows = 3;

	Transform _transfrom;
	bool rotation = false;

	GameObject[] blocks;
	int[, ] table;
	int fixedBlock = 1;

	Transform _left;
	Transform _right;

	int _turn = 0;

	void Awake()
	{
		table = new int[rows, cols];
	}

	void Start()
	{
		blocks = GameObject.FindGameObjectsWithTag("Block");

		for (int i = 0; i < blocks.Length; i++)
		{
			var b = blocks[i].GetComponent<Block>();
			table[b.y, b.x] = i + 1;
		}

		_left = GameObject.Find("rot_icon_left").transform;
		_right = GameObject.Find("rot_icon_right").transform;

		_transfrom = transform;

		if (rule == Rule.Switch)
		{
			_left.GetComponent<SpriteRenderer>().color = SystemResource.instance.colors[1];
			_right.GetComponent<SpriteRenderer>().color = SystemResource.instance.colors[1];
		}

		_turn = 0;
	}

	void Update()
	{
		if (!rotation)
		{
#if UNITY_ANDROID
			int halfWidth = Screen.width / 2;

			if (Input.GetMouseButtonDown(0))
			{
				StartCoroutine(Rotate(Input.mousePosition.x > halfWidth));
			}
#else
			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				StartCoroutine(Rotate(false));
			}
			else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				StartCoroutine(Rotate(true));
			}
#endif
		}
	}

	float EaseIn(float x)
	{
		return x * x;
	}

	IEnumerator Rotate(bool clockwise)
	{
		var icon = clockwise ? _right : _left;

		SystemResource.instance.PlaySE();

		Quaternion start = _transfrom.localRotation;
		Quaternion end = Quaternion.Euler(0.0f, clockwise ? 90.0f : -90.0f, 0.0f) * start;
		rotation = true;
		for (int i = 1; i <= 15; i++)
		{
			float t = i / 15.0f;
			_transfrom.localRotation = Quaternion.Slerp(start, end, EaseIn(t));

			icon.Rotate(0.0f, 0.0f, (clockwise ? -1.0f : 1.0f) / 15.0f * 90.0f);

			yield return new WaitForEndOfFrame();
		}

		if (rule == Rule.Fixed)
		{
			fixedBlock = clockwise ? 1 : 2;
		}
		else if (rule == Rule.Switch)
		{
			fixedBlock = (_turn & 1) == 0 ? 1 : 2;
		}

		if (clockwise)
		{
			RotTableRight();
		}
		else
		{
			RotTableLeft();
		}

		Score.instance.Add();

		Fall(fixedBlock);

		while (IsBusyBlocks())
		{
			yield return null;
		}

		_turn++;
		
		if (rule == Rule.Switch)
		{
			int color = (_turn & 1) == 0 ? 1 : 2;
			_left.GetComponent<SpriteRenderer>().color = SystemResource.instance.colors[color];
			_right.GetComponent<SpriteRenderer>().color = SystemResource.instance.colors[color];
		}

		if (Check())
		{
			yield return new WaitForSeconds(0.6f);
		}

		rotation = false;
	}

	/// <summary>
	/// ブロックがアニメーション中
	/// </summary>
	bool IsBusyBlocks()
	{
		foreach (var b in blocks)
		{
			if (b.GetComponent<Block>().IsBusy())
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// ブロックを落とす
	/// </summary>
	void Fall(int fixedBlock)
	{
		for (int i = rows - 2; i >= 0; i--)
		{
			for (int j = 0; j < cols; j++)
			{
				if (table[i, j] > 0)
				{
					int d = 0;
					for (int k = i + 1; k < cols; k++)
					{
						if (table[k, j] > 0)
						{
							break;
						}
						d++;
					}
					if (d > 0)
					{
						Block a = GetBlock(j, i);;
						if (a.type == fixedBlock)
						{
							table[i + d, j] = table[i, j];
							table[i, j] = 0;
							a.Move(d);
						}
					}
				}
			}
		}
	}

	static void Swap<T>(ref T a, ref T b)
	{
		T t = a;
		a = b;
		b = t;
	}

	void SwapReflection()
	{
		for (int i = 0; i < rows; i++)
		{
			for (int j = i + 1; j < cols; j++)
			{
				Swap(ref table[i, j], ref table[j, i]);
			}
		}
	}

	void SwapReflectionX()
	{
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols / 2; j++)
			{
				Swap(ref table[i, j], ref table[i, cols - j - 1]);
			}
		}
	}

	void SwapReflectionY()
	{
		for (int i = 0; i < rows / 2; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Swap(ref table[i, j], ref table[rows - i - 1, j]);
			}
		}
	}

	/// <summary>
	/// 左回転
	/// </summary>
	void RotTableLeft()
	{
//		// y = x 軸の鏡映変換
//		Swap(ref table[0, 1], ref table[1, 0]);
//		Swap(ref table[0, 2], ref table[2, 0]);
//		Swap(ref table[1, 2], ref table[2, 1]);
//		// y 軸の鏡映変換
//		Swap(ref table[0, 0], ref table[2, 0]);
//		Swap(ref table[0, 1], ref table[2, 1]);
//		Swap(ref table[0, 2], ref table[2, 2]);
		SwapReflection();
		SwapReflectionY();
	}

	/// <summary>
	/// 右回転
	/// </summary>
	void RotTableRight()
	{
		// y = x 軸の鏡映変換
//		Swap(ref table[0, 1], ref table[1, 0]);
//		Swap(ref table[0, 2], ref table[2, 0]);
//		Swap(ref table[1, 2], ref table[2, 1]);
//		// x 軸の鏡映変換
//		Swap(ref table[0, 0], ref table[0, 2]);
//		Swap(ref table[1, 0], ref table[1, 2]);
//		Swap(ref table[2, 0], ref table[2, 2]);
		SwapReflection();
		SwapReflectionX();
	}

	/// <summary>
	/// 右方向に調べる
	/// </summary>
	int CheckRight(int type, int x, int y)
	{
		for (int i = x + 1; i < cols; i++)
		{
			if (table[y, i] <= 0)
			{
				return 0;
			}

			var b = GetBlock(i, y);

			if (b.type == StaticBlock)
			{
				return 0;
			}

			if (b.type == type)
			{
				return i - x - 1;
			}
		}

		return 0;
	}

	/// <summary>
	/// 下方向に調べる
	/// </summary>
	int CheckDown(int type, int x, int y)
	{
		for (int i = y + 1; i < rows; i++)
		{
			if (table[i, x] <= 0)
			{
				return 0;
			}

			var b = GetBlock(x, i);

			if (b.type == StaticBlock)
			{
				return 0;
			}

			if (b.type == type)
			{
				return i - y - 1;
			}
		}

		return 0;
	}

	Block GetBlock(int x, int y)
	{
		return blocks[table[y, x] - 1].GetComponent<Block>();
	}

	bool Check()
	{
		bool check = false;

		for (int i = table.GetLength(0) - 1; i >= 0; i--)
		{
			for (int j = 0; j < table.GetLength(1); j++)
			{
				int k = table[i, j];
				if (k > 0)
				{
					var b = GetBlock(j, i);

					if (b.type == StaticBlock)
					{
						continue;
					}

					int r = CheckRight(b.type, j, i);
					if (r > 0)
					{
						while (r > 0)
						{
							var t = GetBlock(j + r, i);
							t.type = b.type;
							t.Change();
							t.type = StaticBlock;
							r--;
						}
						Debug.Log("OK");
						check = true;
						continue;
					}

					int d = CheckDown(b.type, j, i);
					if (d > 0)
					{
						while (d > 0)
						{
							var t = GetBlock(j, i + d);
							t.Change();
							t.type = StaticBlock;
							d--;
						}
						Debug.Log("OK");
						check = true;
					}
				}
			}
		}

//		if (CheckAll(1))
//		{
//			Debug.Log("Clear");
//			SceneManager.instance.Finish();
//		}
//
//		if (CheckAll(2))
//		{
//			Debug.Log("Clear");
//			SceneManager.instance.Finish();
//		}

		if (CheckAll())
		{
			SceneManager.instance.Finish();
		}

		return check;
	}

	/// <summary>
	/// ブロックが1種類だけになったかチェック
	/// </summary>
	bool CheckAll(int type)
	{
		for (int i = 0; i < blocks.Length; i++)
		{
			if (blocks[i].GetComponent<Block>().type == type)
			{
				return false;
			}
		}
		return true;
	}

	bool CheckAll()
	{
		int type = 0;
		foreach (var b in blocks)
		{
			int t = b.GetComponent<Block>().type;
			if (t != StaticBlock && t > 0)
			{
				if (type == 0)
				{
					type = t;
				}
				else if(type != t)
				{
					return false;
				}
			}
		}
		return true;
	}
}
