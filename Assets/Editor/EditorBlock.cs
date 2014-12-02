using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Block))]
public class EditorBlock : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if (GUILayout.Button("Setup"))
		{
			Block b = target as Block;
			Rot r = b.transform.parent.GetComponent<Rot>();
			float x = b.x - r.cols * 0.5f + 0.5f;
			float z = r.rows * 0.5f - b.y - 0.5f;
			b.transform.position = new Vector3(x, -0.5f, z);
		}
	}
}
