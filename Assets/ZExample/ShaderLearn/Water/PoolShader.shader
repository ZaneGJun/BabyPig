Shader "Test/Pool/PoolShader"
{
	Properties
	{
		_BumpTex ("Bump", 2D) = "white" {}

		_WaterColor("WaterColor", color) = (1,1,1,1)

		_RefractionOffset("RefractionOffset", vector) = (0.0,0.0,0.0,0.0)

		_BumpScale("BumpScale", float) = 1.0
		_Shinness("Shinness", float) = 1.0
		_Spec("Spec", float) = 0.3

		_FresnelBias("FresnelBias", float) = -0.1
		_FresnelScale("FresnelScale", float) = 0.5
		_FresnelPower("FresnelPower", float) = 4.0
	}
	SubShader
	{

		CGINCLUDE

		#include "UnityCG.cginc"
		#include "Lighting.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			half2 texcoord : TEXCOORD0;
			half3 worldNormal : TEXCOORD1;
			half4 screenPos : TEXCOORD2;
			half3 worldLightDir : TEXCOORD3;
			half3 worldViewDir : TEXCOORD4;
		};

		half3 GerstnerOffset4 (half2 xzVtx, half4 steepness, half4 amp, half4 freq, half4 speed, half4 dirAB, half4 dirCD) 
		{
			half3 offsets;
		
			half4 AB = steepness.xxyy * amp.xxyy * dirAB.xyzw;
			half4 CD = steepness.zzww * amp.zzww * dirCD.xyzw;
		
			half4 dotABCD = freq.xyzw * half4(dot(dirAB.xy, xzVtx), dot(dirAB.zw, xzVtx), dot(dirCD.xy, xzVtx), dot(dirCD.zw, xzVtx));
			half4 TIME = _Time.yyyy * speed;
		
			half4 COS = cos (dotABCD + TIME);
			half4 SIN = sin (dotABCD + TIME);
		
			offsets.x = dot(COS, half4(AB.xz, CD.xz));
			offsets.z = dot(COS, half4(AB.yw, CD.yw));
			offsets.y = dot(SIN, amp);

			return offsets;			
		}

		half3 GerstnerNormal4 (half2 xzVtx, half4 amp, half4 freq, half4 speed, half4 dirAB, half4 dirCD) 
		{
			half3 nrml = half3(0,2.0,0);
		
			half4 AB = freq.xxyy * amp.xxyy * dirAB.xyzw;
			half4 CD = freq.zzww * amp.zzww * dirCD.xyzw;
		
			half4 dotABCD = freq.xyzw * half4(dot(dirAB.xy, xzVtx), dot(dirAB.zw, xzVtx), dot(dirCD.xy, xzVtx), dot(dirCD.zw, xzVtx));
			half4 TIME = _Time.yyyy * speed;
		
			half4 COS = cos (dotABCD + TIME);
		
			nrml.x -= dot(COS, half4(AB.xz, CD.xz));
			nrml.z -= dot(COS, half4(AB.yw, CD.yw));
		
			nrml.xz *= 1.0;
			nrml = normalize (nrml);

			return nrml;			
		}

		v2f vert (appdata v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);

			o.worldNormal = half3(0,1,0);
			o.worldLightDir = normalize(WorldSpaceLightDir(v.vertex));
			o.worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
		
			o.screenPos = ComputeGrabScreenPos(o.pos);
			float4 localVertex = v.vertex;
			o.screenPos.z = COMPUTE_EYEDEPTH(localVertex);

			o.texcoord = v.uv;

			return o;
		}
		
		sampler2D _BumpTex;
		sampler2D _RefractionTex;

		sampler2D _CameraDepthTexture;

		float _Spec;

		half4 _RefractionOffset;
		float _BumpScale;

		float _Shinness;

		float _FresnelBias;
		float _FresnelScale;
		float _FresnelPower;  

		float4 _WaterColor;

		half4 frag (v2f i) : SV_Target
		{
			fixed4 col = _WaterColor;

			fixed3 worldNormal = normalize(i.worldNormal);
			fixed3 worldLightDir = normalize(i.worldLightDir);
			half3 worldViewDir = normalize(i.worldViewDir);

			fixed4 bump = tex2D(_BumpTex, i.texcoord + _Time.xx);
			fixed4 bump2 = tex2D(_BumpTex, i.texcoord + float2(0.4, 0.45) - _Time.xx);
			fixed3 bumpNormal = (UnpackNormal(bump) + UnpackNormal(bump2)) / 2.0 * _BumpScale;
			worldNormal += bumpNormal;

			//深度，计算水深
			float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
			float depth = i.screenPos.z;
			float waterDepth = saturate(abs(sceneZ - depth) * 2.0);  

			//refracte
			fixed2 refractionOffset = _RefractionOffset.xy + bumpNormal,xy;
			fixed4 screenPos = i.screenPos;
			screenPos.xy += refractionOffset;
			fixed4 refractionColor = tex2Dproj(_RefractionTex, screenPos);
			//reflect
			half3 reflectDir = reflect(-worldViewDir, worldNormal);
			fixed4 reflectColor = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflectDir);

			//fresnel
			float fresnelFactory = min(1.0, max(0, _FresnelBias + _FresnelScale * pow(1.0 + dot(normalize(-worldViewDir), worldNormal), _FresnelPower)));
			fixed4 fresnelColor = lerp(refractionColor, reflectColor, fresnelFactory);

			//diffuse
			float diff = max(0, dot(worldNormal, worldLightDir));
			fixed3 diffuseColor = _LightColor0 * diff;  

			//specular
			half3 h = normalize(worldLightDir + worldViewDir);
			float nh = max(0, dot(h, worldNormal));
			float spec = max(0, pow(nh, _Shinness));
			fixed3 specColor = _LightColor0 * spec * _Spec;  

			col.rgb += refractionColor.rgb * (1.0 - waterDepth) * 0.2;
			col.rgb += diffuseColor + specColor + fresnelColor;
			col.a = lerp(0.05, _WaterColor.a, waterDepth);
			return col;
		}

		ENDCG

		GrabPass
		{
			"_RefractionTex"
		}

		Pass
		{
			Tags {"RenderType" = "Opaque" "Queue" = "Opaque"}
			//Blend SrcAlpha OneMinusSrcAlpha
			// No culling or depth
			//ZTest LEqual
			//Cull Off ZWrite Off 

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			

			ENDCG
		}
	}
}
