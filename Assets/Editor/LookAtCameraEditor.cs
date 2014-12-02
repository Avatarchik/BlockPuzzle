using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Camera))]
public class LookAtCameraEditor : Editor
{
	Vector3 value;
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		EditorGUILayout.BeginHorizontal();
		value = EditorGUILayout.Vector3Field("target", value);
		if (GUILayout.Button("Look", GUILayout.MaxWidth(60)))
		{
			Camera c = target as Camera;
			Undo.RecordObject(c, "Camera LookAt");
			c.transform.rotation = Quaternion.LookRotation(value - c.transform.position);
			EditorUtility.SetDirty(target);
		}
		EditorGUILayout.EndHorizontal();
	}
}
