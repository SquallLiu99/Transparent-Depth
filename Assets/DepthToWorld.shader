Shader "Custom/DepthToWorld"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
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
				float4 clipPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				// keep normalized clip pos [-1 1] at near plane
				o.clipPos = float4(o.vertex.xy, 0, 1);

				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			float4 frag (v2f i) : SV_Target
			{
				float4 depth = tex2D(_CameraDepthTexture, i.uv);
				float3 vray = mul(unity_CameraInvProjection, i.clipPos * _ProjectionParams.z).xyz;
				vray.z *= -1;
				float3 vpos = vray * Linear01Depth(depth);
				float3 wpos = mul(unity_CameraToWorld, float4(vpos, 1));

				return float4(wpos, 1);
			}
			ENDCG
		}
	}
}
