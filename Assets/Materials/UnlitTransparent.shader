Shader "Custom/UnlitTransparent" {
	Properties {
		_Color("Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform fixed4 _Color;
			
			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
			};
			
			v2f vert(appdata_base v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = _Color;
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR
			{
				return i.color;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
