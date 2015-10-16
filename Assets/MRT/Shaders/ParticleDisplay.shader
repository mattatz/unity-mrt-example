Shader "GPUParticleSystem/Display"  {

	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_PosTex ("Position Texture", 2D) = "white" {}
	}

	SubShader {
		Cull Off ZWrite Off ZTest Always

		CGINCLUDE

		#include "UnityCG.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f {
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
		};

		sampler2D _MainTex;
		sampler2D _PosTex;

		v2f vert (appdata IN) {
			v2f o;

			float4 vertex = tex2Dlod(_PosTex, float4(IN.uv, 0, 0));
			o.vertex = mul(UNITY_MATRIX_MVP, float4(vertex.xyz, 1));

			o.uv = IN.uv;

			return o;
		}

		fixed4 frag (v2f i) : SV_Target {
			fixed4 col = tex2D(_MainTex, i.uv);
			return col;
		}
	
		ENDCG

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
