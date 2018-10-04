Shader "Test/Shadow/ReciveShadow"
{
	Properties
	{
		ligthDepthTexture ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			//灯光深度贴图
			sampler2D ligthDepthTexture;
			//灯光的WVP矩阵
			float4x4 lightProjectionMatrix;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 lightClipPos: TEXCOORD1;	//深度贴图uv
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				
				//计算灯光的WVP
				lightProjectionMatrix = mul(lightProjectionMatrix, unity_ObjectToWorld);
				//计算在灯光坐标下的位置
				o.lightClipPos = mul(lightProjectionMatrix, float4(v.vertex.xyz, 1));
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = (1.0,1.0,1.0,1.0);
				//计算采样深度图的uv
				float2 depthUV = i.lightClipPos.xy / i.lightClipPos.w;
				fixed4 shadowCol = tex2D(ligthDepthTexture, depthUV);
				//转化为深度值
				float shadowDepth = DecodeFloatRGBA(shadowCol);
				//decode后是[0,1],需要映射到opengl ndc下z的[-1,1] 
				shadowDepth = shadowDepth * 2.0f - 1.0f;

				//计算像素在灯光坐标下的深度
				float pixelLightDepth = i.lightClipPos.z / i.lightClipPos.w;

				//判断是否处在阴影区域
				if(pixelLightDepth < shadowDepth){
					return 0;
				}

				return col;
			}
			ENDCG
		}
	}
}
