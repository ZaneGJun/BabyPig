Shader "Test/Shadow/ReciveShadow"
{
	Properties
	{
		ligthDepthTexture ("Texture", 2D) = "white" {}
		_shadowColor("ShadowColor", color) = (0,0,0,1)
	}
	SubShader
	{
		CGINCLUDE
		#include "UnityCG.cginc"

		//灯光深度贴图
		sampler2D ligthDepthTexture;
		//灯光的WVP矩阵
		float4x4 lightProjectionMatrix;
		//阴影颜色
		float4 _shadowColor;

		//***************************************************

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float3 normal: NORMAL;
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

			//计算像素在灯光坐标下的深度,因为矩阵已经处理，所以是[0,1]
			float pixelLightDepth = i.lightClipPos.z / i.lightClipPos.w;

			//判断是否处在阴影区域
			if(pixelLightDepth < shadowDepth){
				return _shadowColor;
			}

			return col;
		}

		//***************************************************
		//Shadow Bias

		/**
		 * 采用ShadowBias消除Shadow Acne
		 */
		fixed4 fragShadowBias(v2f i) : SV_Target
		{
			fixed4 col = (1.0,1.0,1.0,1.0);
			//计算采样深度图的uv
			float2 depthUV = i.lightClipPos.xy / i.lightClipPos.w;
			fixed4 shadowCol = tex2D(ligthDepthTexture, depthUV);
			//转化为深度值
			float shadowDepth = DecodeFloatRGBA(shadowCol);

			//计算像素在灯光坐标下的深度
			float pixelLightDepth = i.lightClipPos.z / i.lightClipPos.w;

			//shadow bias
			pixelLightDepth += 0.005f;

			//判断是否处在阴影区域
			if(pixelLightDepth < shadowDepth){
				return _shadowColor;
			}

			return col;
		}

		//***************************************************
		//Slop-Scale Depth Bias

		struct v2fSlopeScale
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
			float4 lightClipPos: TEXCOORD1;	//深度贴图uv
			float3 worldNormal: TEXCOORD2;	//法线
			float3 worldLightDir: TEXCOORD3;
		};

		v2fSlopeScale vertSlopeScale (appdata v)
		{
			v2fSlopeScale o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
				
			//计算灯光的WVP
			lightProjectionMatrix = mul(lightProjectionMatrix, unity_ObjectToWorld);
			//计算在灯光坐标下的位置
			o.lightClipPos = mul(lightProjectionMatrix, float4(v.vertex.xyz, 1));
			//世界坐标下normal
			o.worldNormal = UnityObjectToWorldNormal(v.normal);
			//世界坐标
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			//世界坐标下光源方向
			o.worldLightDir = UnityWorldSpaceLightDir(worldPos);

			return o;
		}

		/**
		 * 计算Slop-Scale Depth Bias
		 * bias = miniBais + maxBais * SlopeScale
		 * SlopeScale可以理解为光线方向与法线夹角的tan值
		 */
		float CaculateSlopeScaleDepthBias(float3 lightDir, float3 normal, float maxBias, float baseBias)
		{
			float cos_val = saturate(dot(lightDir, normal));
			float sin_val = sqrt(1 - cos_val * cos_val);//sin(acos(L·N))
			float tan_val = sin_val / cos_val; //tan(acos(L·N))

			float bias = baseBias + clamp(tan_val, 0, maxBias);
			return bias;
		}

		/**
		 * 采用Slope-Scale Depth Bias消除Shadow Acne
		 */
		fixed4 fragSlopeScaleDepthBias(v2fSlopeScale i) : SV_Target
		{
			fixed4 col = (1.0,1.0,1.0,1.0);
			//计算采样深度图的uv
			float2 depthUV = i.lightClipPos.xy / i.lightClipPos.w;
			fixed4 shadowCol = tex2D(ligthDepthTexture, depthUV);
			//转化为深度值
			float shadowDepth = DecodeFloatRGBA(shadowCol);

			//计算像素在灯光坐标下的深度
			float pixelLightDepth = i.lightClipPos.z / i.lightClipPos.w;

			float bias = CaculateSlopeScaleDepthBias(normalize(i.worldLightDir), normalize(i.worldNormal), 0.001f, 0.002f);
			pixelLightDepth += bias;

			//判断是否处在阴影区域
			if(pixelLightDepth < shadowDepth){
				return _shadowColor;
			}

			return col;
		}

		//***************************************************
		//PCF

		/**
		 * PCF滤波
		 */
		float PencentCloaerFilter(float2 uv, float sceneDepth, float bias)
		{
			float _TexturePixelWidth = 1024.0f;
			float _TexturePixelHeight = 1024.0f;
			float _FilterSize = 1.5f;

			float shadow = 0.0f;
			float2 texelSize = float2(_TexturePixelWidth, _TexturePixelHeight);
			texelSize = 1.0f / texelSize;

			for(int x = -_FilterSize; x <= _FilterSize; ++x)
			{
				for(int y = -_FilterSize; y <= _FilterSize; ++y)
				{
					float uv_offset = float2(x, y) * texelSize;
					fixed4 shadowCol = tex2D(ligthDepthTexture, uv + uv_offset);
					float shadowDepth = DecodeFloatRGBA(shadowCol);
					
					float pixelLightDepth = sceneDepth + bias;
					if(pixelLightDepth < shadowDepth){
						shadow += 1.0f;
					}
				}
			}
		
			float total = (_FilterSize * 2.0f + 1.0f) * (_FilterSize * 2.0f + 1.0f);
			shadow = shadow / total;
			return shadow;
		}

		/**
		 * 泊松分布PCF
		 */
		float PencentCloaerPoissonFilter(float2 uv, float sceneDepth, float bias)
		{
			float2 possionDisk[4] = 
			{
				float2(-0.94201624, -0.39906216),
				float2(0.94558609, -0.76890725),
			    float2(-0.094184101, -0.92938870),
				float2(0.34495938, 0.29387760)
			};
	
			float shadow = 0.0f;

			for(int i = 0; i < 4; ++i)
			{
				float2 uv_offset = possionDisk[i]/500.0f;
				fixed4 shadowCol = tex2D(ligthDepthTexture, uv + uv_offset);
				float shadowDepth = DecodeFloatRGBA(shadowCol);
					
				float pixelLightDepth = sceneDepth + bias;
				if(pixelLightDepth < shadowDepth){
					shadow += 0.2f;
				}
			}

			return shadow;
		}

		/**
		 * 采用Slope-Scale Depth Bias消除Shadow Acne
		 * 采用Pencentage close Filtering(PCF)来进行Aliasing
		 */
		fixed4 fragSlopeScaleDepthBiasPCF(v2fSlopeScale i) : SV_Target
		{
			fixed4 col = (1.0,1.0,1.0,1.0);

			//计算采样深度图的uv
			float2 depthUV = i.lightClipPos.xy / i.lightClipPos.w;
			fixed4 shadowCol = tex2D(ligthDepthTexture, depthUV);
			//转化为深度值
			float shadowDepth = DecodeFloatRGBA(shadowCol);

			//计算像素在灯光坐标下的深度
			float pixelLightDepth = i.lightClipPos.z / i.lightClipPos.w;
			float bias = CaculateSlopeScaleDepthBias(normalize(i.worldLightDir), normalize(i.worldNormal), 0.001f, 0.002f);
			pixelLightDepth += bias;

			//PCF
			float shadowFilterFactor = PencentCloaerPoissonFilter(depthUV, pixelLightDepth, bias); //PencentCloaerFilter(depthUV, pixelLightDepth, bias);
	
			//根据计算出的进行插值
			fixed4 retCol = col * (1.0f - shadowFilterFactor) + _shadowColor * shadowFilterFactor;
			return retCol;
		}

		//***************************************************
		//CSM(Cascaded Shadow Maps)

		ENDCG

		Tags { "RenderType"="Opaque" }
		Pass
		{
			CGPROGRAM
			//#pragma vertex vert
			//#pragma fragment frag

			#pragma vertex vertSlopeScale
			#pragma fragment fragSlopeScaleDepthBiasPCF
			
			ENDCG
		}
	}
}
