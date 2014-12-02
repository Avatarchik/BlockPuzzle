using UnityEngine;
using UnityEditor;

public class CaptureScreenshot : Editor
{
	[MenuItem("Editor/Capture...")]
	public static void Capture()
	{
		string path = EditorUtility.SaveFilePanel("Save Screenshot", Application.dataPath, "cap", "png");
		if (!string.IsNullOrEmpty(path))
		{
			Application.CaptureScreenshot(path);
		}
	}
}
