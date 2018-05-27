// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Game/NormalGridShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SecondaryTex("SencondaryTexture", 2D) = "white" {}
		_Diffuse("Diffuse", Color) = (1,1,1,1)

		_OutlineWidth("OutlineWidth", Range(0,1)) = 0.01
		_OutlineColor("OutlineColor",Color) = (0,0,0,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			//first pass outline
			Pass
			{
			//剔除正面
			Cull front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _OutlineWidth;
			fixed4 _OutlineColor;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				
				fixed3 Normal = normalize(v.normal);
				v.vertex.xyz += Normal.xyz * _OutlineWidth;
				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _OutlineColor;
			}
			ENDCG
		}
		
		//second pass
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			//#include "UnityStandardCore.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _SecondaryTex;
			float4 _MainTex_ST;
			fixed4 _Diffuse;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = normalize(mul(v.normal,(float3x3)unity_WorldToObject));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				// sample the texture
				if (i.normal.y > 0)
				{
					col = tex2D(_MainTex, i.uv);
				}
				else
				{
					col = tex2D(_SecondaryTex, i.uv);
				}

				//光照
				//环境光
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * col.xyz;
				//世界灯光方向
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				//漫反射
				fixed3 diffuse = _LightColor0.rbg * col.xyz * _Diffuse * saturate(dot(i.normal, worldLightDir) * 0.5 + 0.5);
				
				col.xyz = diffuse + ambient;

				return col;
			}
			ENDCG
		}
	}
}
