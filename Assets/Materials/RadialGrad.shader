Shader "Custom/RadialGrad"
{
	Properties
	{
		_Color1 ("Color 1", Color) = (1, 1, 1, 0)
		_Color2 ("Color 2", Color) = (1, 1, 1, 0)
		_Exponent ("Exponent", Float) = 1.0
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	uniform half4 _Color1;
	uniform half4 _Color2;
	uniform half _Exponent;
	
	struct v2f
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};
	
	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
//		float aspect = _ScreenParams.x / _ScreenParams.y;
		o.uv = v.texcoord * 2.0f - 1.0f;
//		o.uv.x *= aspect;
		return o;
	}
	
	fixed4 frag(v2f_img i) : COLOR
	{
		half d = length(i.uv);
//		half d = atan(i.uv.x / i.uv.y);
		//half d = length(i.uv * 0.5f + 0.5f);
		return lerp(_Color1, _Color2, pow(d, _Exponent));
	}
	
	ENDCG
	
	SubShader
	{
		Tags { "RenderType"="Transparent+1" }
		
		Pass
		{
			ZWrite Off
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
			Fog { Mode Off }
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
