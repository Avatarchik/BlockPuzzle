using UnityEngine;
using System.Collections;

public class RadialGrad : MonoBehaviour
{
	public Material material;
	public Color color1;
	public Color color2;

	int _color1Id;
	int _color2Id;

	void Awake()
	{
		_color1Id = Shader.PropertyToID("_Color1");
		_color2Id = Shader.PropertyToID("_Color2");
	}

	void OnPostRender()
	{
		material.SetColor(_color1Id, color1);
		material.SetColor(_color2Id, color2);
		Graphics.Blit(null, material);
	}

#if UNITY_EDITOR
	void Start() {}
#endif
}
