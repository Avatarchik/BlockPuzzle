Shader "Custom/VtxColor4" {
	Properties {
		_LightColor("Light Color", Color) = (1, 1, 1, 1)
		_DarkColor("Dark Color", Color) = (1, 1, 1, 1)
		_Dir("Dir", Vector) = (1, 1, 1, 0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				half3 normal : NORMAL;
				fixed4 color : COLOR;
				
			};
			
			struct v2f {
				float4 position : SV_POSITION;
				fixed4 color : COLOR;
			};
			
			uniform fixed4 _LightColor;
			uniform fixed4 _DarkColor;
			uniform half3 _Dir;
			
			v2f vert(appdata_t v) {
				v2f o;
				o.position = mul(UNITY_MATRIX_MVP, v.vertex);
				float l = dot(normalize(_Dir), v.normal) * 0.5 + 0.5;
				o.color = lerp(_DarkColor, _LightColor, l * l);
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR {
				fixed4 col = i.color;
				return col;
			}
			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
