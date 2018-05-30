Shader "HDR/ToneMappintShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}


	CGINCLUDE
	#include "UnityCG.cginc"

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;
	float _ExposureAdjustment;

	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 

	//reinhard
	float3 reinhard(float3 color, float adapted_lum)
	{
		const float MIDDLE_GREY = 1;
		color *= MIDDLE_GREY / adapted_lum;
		return color / (1.0f + color);
	}

	//simpleReinhard
	float3 simpleReinhard(float3 color, float adapted_lum)
	{
		float lum = Luminance(color); 
		float lumTm = lum * adapted_lum;
		float scale = lumTm / (1+lumTm);  
		return color * scale / lum;
	}

	//ACES
	float3 ACESToneMapping(float3 color, float adapted_lum)
	{
		const float A = 2.51f;
		const float B = 0.03f;
		const float C = 2.43f;
		const float D = 0.59f;
		const float E = 0.14f;

		color *= adapted_lum;
		return (color * (A * color + B)) / (color * (C * color + D) + E);
	}

	float4 fragACESToneMapping(v2f i) : COLOR
	{
		float4 texColor = tex2D(_MainTex, i.uv);
		
		float4 col = texColor;
		col.rgb = ACESToneMapping(col.rgb, _ExposureAdjustment);
		return col;
	}

	float4 fragReinhardToneMapping(v2f i) : COLOR
	{
		float4 texColor = tex2D(_MainTex, i.uv);
		
		float4 col = texColor;
		col.rgb = reinhard(col.rgb, _ExposureAdjustment);
		return col;
	}

	float4 fragSimpleReinhardToneMapping(v2f i) : COLOR
	{
		float4 texColor = tex2D(_MainTex, i.uv);
		
		float4 col = texColor;
		col.rgb = simpleReinhard(col.rgb, _ExposureAdjustment);
		return col;
	}

	ENDCG

	SubShader
	{
		//0
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragSimpleReinhardToneMapping
			
			ENDCG
		}

		//1
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragReinhardToneMapping
			
			ENDCG
		}

		//2
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragACESToneMapping
			
			ENDCG
		}
	}
}
