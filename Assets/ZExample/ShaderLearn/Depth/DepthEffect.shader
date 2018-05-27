Shader "Depth/DepthEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MainColor("MainColor", color) = (1,1,1,1)
		_IntersectColor("IntersectColor", color) = (1,1,1,1)
		_RimColor("RimColor", color) = (0,0,0,1)

		_DepthScale("DepthScale", float) = 20.0
	}
	SubShader
	{
		Tags {"RenderType" = "Transparent" "Queue" = "Transparent"} 
		// No culling or depth
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off 

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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				float3 worldViewDir : TEXCOORD3;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				o.screenPos = ComputeGrabScreenPos(o.vertex);
				float4 localVertex = v.vertex;
				o.screenPos.z = COMPUTE_EYEDEPTH(localVertex);

				o.worldNormal = v.normal;

				o.worldViewDir = UnityWorldSpaceViewDir(v.vertex);

				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			float _DepthScale;

			float4 _MainColor;
			float4 _IntersectColor;
			float4 _RimColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
				float depth = i.screenPos.z;
				float intersectValue = saturate(abs(sceneZ - depth) * _DepthScale);

				half3 worldNormal = normalize(i.worldNormal);
				half3 worldViewDir = normalize(i.worldViewDir);

				fixed4 rimColor = _RimColor * saturate(1.0 - dot(worldNormal,worldViewDir));

				col = col * _MainColor + rimColor + _IntersectColor * (1.0 - intersectValue);

				return col;
			}
			ENDCG
		}
	}
}
