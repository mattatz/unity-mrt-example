Shader "GPUParticleSystem/Update" {

	Properties {
		_PosTex ("Position Texture", 2D) = "white" {}
		_VelTex ("Velocity Texture", 2D) = "white" {}
		_AccTex ("Acceleration Texture", 2D) = "white" {}

		_TrackingTo ("Tracking Target", Vector) = (0, 0, 0, 0)
	}

	SubShader {

		Cull Off ZWrite Off ZTest Always

		CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Random.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		struct f2o {
			float4 color0 : COLOR0;
			float4 color1 : COLOR1;
			float4 color2 : COLOR2;
		};

		v2f vert (appdata IN) {
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
			o.uv = IN.uv;
			return o;
		}
		
		sampler2D _PosTex;
		sampler2D _VelTex;
		sampler2D _AccTex;
		float3 _TrackingTo;

		f2o init (v2f IN) {
			f2o OUT;

			OUT.color0 = float4((rand3(IN.uv) - 0.5) * 2.0, rand(IN.uv));
			OUT.color1 = float4(0, 0, 0, 1);
			OUT.color2 = float4(0, 0, 0, 1);

			return OUT;
		}

		float3 track (float3 p) {
			float3 v = _TrackingTo - p;
			return normalize(v);
		}

		f2o update (v2f IN) {
			f2o OUT;

			float4 pos = tex2D(_PosTex, IN.uv);
			float4 vel = tex2D(_VelTex, IN.uv);
			float4 acc = tex2D(_AccTex, IN.uv);

			acc.xyz += track(pos.xyz);
			vel.xyz *= 0.9;
			vel.xyz += (acc.xyz * unity_DeltaTime.x) / pos.w;
			pos.xyz += vel.xyz * unity_DeltaTime.x;

			OUT.color0 = pos;
			OUT.color1 = vel;
			OUT.color2 = acc;

			return OUT;
		}

		ENDCG

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment init
			ENDCG
		}

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment update
			ENDCG
		}

	}
}
