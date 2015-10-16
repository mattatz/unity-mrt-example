Shader "MRT/MRTTest"
{

	Properties {
	}

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			struct f2o 
			{
				float4 col0 : COLOR0;
				float4 col1 : COLOR1;
				float4 col2 : COLOR2;
			};

			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.uv = IN.uv;
				return OUT;
			}
			
			sampler2D _MainTex;

			f2o frag (v2f IN) {
				f2o OUT;
				OUT.col0 = float4(1, 0, 0, 1);
				OUT.col1 = float4(0, 1, 0, 1);
				OUT.col2 = float4(0, 0, 1, 1);
				return OUT;
			}
			ENDCG
		}
	}
}
