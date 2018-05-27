Shader "Game/RimLightHalfLambert"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "white" {}
		_RimColor("RimColor", color) = (1,1,1,1)
		_RimFactor("RimFactor", float) = 1.0
		_Diffuse("Diffuse", color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		/*
		1.NormalMap中每个像素记录的是该像素在切线空间TBN中的方向，还是这个该像素相对的法线偏移值？
		2.将光照、视线等转换到TBN中计算还是将TBN法线转换到世界坐标中进行计算？如果转换到TBN，怎样转换
		*/

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc" 

			struct appdata
			{
				float4 vertex : POSITION;		//顶点
				float2 uv : TEXCOORD0;			//uv
				float3 normal: NORMAL;		//法线
				//float3 tangant : TEXCOORD2;		//切线
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldNormal : TEXCOORD1;
				float3 worldViewDir : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NormalMap;
			fixed4 _RimColor;		//边缘光颜色
			float _RimFactor;		//边缘光因子

			fixed4 _Diffuse;	//漫反射
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//计算worldNormal,worldViewDir
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldViewDir = WorldSpaceViewDir(v.vertex);


				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 normalCol = tex2D(_NormalMap, i.uv);
				fixed3 pixelNormal = UnpackNormal(normalCol);

				//根据世界坐标下的法线与视线计算RimLight
				float3 worldNormal = normalize(i.worldNormal);
				float3 worldViewDir = normalize(i.worldViewDir);
				fixed4 rimLightColor = (1.0 - max(0, dot(worldNormal, worldViewDir))) * _RimFactor * _RimColor;

				//half-lambert光照模型
				float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				float3 lambertFactor = 0.5 * dot(worldNormal, worldLightDir) + 0.5;
				float3 diffuse = _Diffuse * lambertFactor * _LightColor0.xyz;

				//环境光
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				col.xyz +=  diffuse + ambient + rimLightColor.xyz;
				col.a = col.a;
				return col;
			}
			ENDCG
		}
	}
}
