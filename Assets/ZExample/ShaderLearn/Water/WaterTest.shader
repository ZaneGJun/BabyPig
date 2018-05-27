Shader "Water/WaterTest" {
	Properties{
		_Bump("Bump", 2D) = "white" {}
		_ReflectTex("Reflect", Cube) = "" {}
		_RefractionTex("RefractionTex", 2D) = "white" {}
		_Color("Color", color) = (1,1,1,1)
		_LightColor("LightColor", color) = (1,1,1,1)
		_SpeColor("SpeColor", color) = (1,1,1,1)

		_Steepness("Steepness", Vector) = (1.0,1.0,1.0,1.0)
		_Amplitude("Amplitude", Vector) = (0.3,0.35,0.25,0.25)
		_Frequency("Frequency", Vector) = (1.0,1.0,1.0,1.0)
		_Speed("Speed", Vector) = (1.0,1.0,1.0,1.0)
		_DirAB("DirAB", Vector) = (1.0,1.0,1.0,1.0)
		_DirCD("DirCD", Vector) = (1.0,1.0,1.0,1.0)
		
		_Idiff("Idiff", float) = 0.15
		_Kspe("Kspe", float) = 0.5
		_Shininess("Shininess", float) = 220.0
		_WorldLightDir("WroldLightDir", Vector) = (0.1, -0.11, -1.37, 0.0)

		_GerstnerIntensity("GerstnerIntensity", float) = 0.73

		_NormalScale("NormalScale", float) = 1.0

		_RefractRatio("ReftactRatio", float) = 1.3333

		_FresnelScale("FresnelScale", float) = 0.1
		_FresnelBias("FresnelBias", float) = -0.2
		_FresnelPower("FresnelPower", float) = 5.0
	}


CGINCLUDE
		#include "UnityCG.cginc"

		struct appdata_t {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			float2 bumpCoord : TEXCOORD0;
			float3 normal : TEXCOORD1;
			float3 eyeDir : TEXCOORD2;
			float4 screenPos : TEXCOORD4;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		sampler2D _Bump;
		sampler2D _RefractionTex;
		samplerCUBE _ReflectTex; 
		fixed4 _Color;
		
		//漫反射光颜色
		fixed4 _LightColor;
		//镜面反射颜色
		fixed4 _SpeColor;

		//法线系数
		float _NormalScale;
		
		//漫反射系数
		float _Idiff;

		//物体对于反射光的衰减系数
		float _Kspe;
		//镜面反射系数
		float _Shininess;
		float4 _WorldLightDir;

		//Gerstner波强度
		float _GerstnerIntensity;

		//折射
		float _RefractRatio;

		//fresnel 菲涅尔
		float _FresnelBias;
		float _FresnelPower;
		float _FresnelScale;

		float4 _Steepness;		//坡度
		float4 _Amplitude;		//振幅
		float4 _Frequency;		//频率
		float4 _Speed;			//速度
		float4 _DirAB;			//方向
		float4 _DirCD;			//方向

		//=================================================
		//单个Gerstner波
		half3 Gerstner1(half3 worldPos)
		{
			half3 offset;

			offset.x = _Steepness.x * _Amplitude.x * _DirAB.x * cos(_Frequency.x * dot(_DirAB.xy, worldPos.xz) + _Speed.x * _Time.y);
			offset.z = _Steepness.x * _Amplitude.x * _DirAB.y * cos(_Frequency.x * dot(_DirAB.xy, worldPos.xz) + _Speed.x * _Time.y);
			offset.y = _Amplitude.x * sin(_Frequency.x * dot(_DirAB.xy, worldPos.xz) + _Speed.x * _Time.y);

			return offset;
		}

		//4个Gerstner波叠加
		half3 Gerstner4(half3 worldPos)
		{
			half3 offsets;
			float2 xzVtx = worldPos.xz;

			half4 AB = _Steepness.xxyy * _Amplitude.xxyy * _DirAB.xyzw;
			half4 CD = _Steepness.zzww * _Amplitude.zzww * _DirCD.xyzw;

			half4 dotABCD = _Frequency.xyzw * half4(dot(_DirAB.xy, xzVtx), dot(_DirAB.zw, xzVtx), dot(_DirCD.xy, xzVtx), dot(_DirCD.zw, xzVtx));
			half4 TIME = _Time.yyyy * _Speed;

			half4 COS = cos(dotABCD + TIME);
			half4 SIN = sin(dotABCD + TIME);

			offsets.x = dot(COS, half4(AB.xz, CD.xz));
			offsets.z = dot(COS, half4(AB.yw, CD.yw));
			offsets.y = dot(SIN, _Amplitude);

			return offsets;
		}

		half3 Gerstner1Normal(half3 worldPos)
		{
			half3 normal = half3(0.0, 0.0, 0.0);

			normal.x -= _DirAB.x * (_Frequency.x * _Amplitude.x)
				* cos(_Frequency.x * dot(_DirAB.xy, worldPos.xz) + _Speed.x * _Time.y);

			normal.z -= _DirAB.y * (_Frequency.x * _Amplitude.x)
				* cos(_Frequency.x * dot(_DirAB.xy, worldPos.xz) + _Speed.x * _Time.y);

			normal.y = 1.0 - _Steepness.x * (_Frequency.x * _Amplitude.x)
				* sin(_Frequency.x * dot(_DirAB.xy, worldPos.xz) + _Speed.x * _Time.y);

			return normal;
		}

		half3 Gerstner4Normal(half3 worldPos)
		{
			half3 nrml = half3(0.0, 2.0, 0.0);
			float2 xzVtx = float2(worldPos.x, worldPos.z);

			half4 AB = _Frequency.xxyy * _Amplitude.xxyy * _DirAB.xyzw;
			half4 CD = _Frequency.zzww * _Amplitude.zzww * _DirCD.xyzw;

			half4 dotABCD = _Frequency.xyzw * half4(dot(_DirAB.xy, xzVtx), dot(_DirAB.zw, xzVtx), dot(_DirCD.xy, xzVtx), dot(_DirCD.zw, xzVtx));
			half4 TIME = _Time.yyyy * _Speed;

			half4 COS = cos(dotABCD + TIME);

			nrml.x -= dot(COS, half4(AB.xz, CD.xz));
			nrml.z -= dot(COS, half4(AB.yw, CD.yw));

			nrml.xz *= _GerstnerIntensity;
			nrml = normalize(nrml);

			return nrml;
		}

		//==================================================
		v2f vert(appdata_t v)
		{
			v2f o;
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			o.bumpCoord = v.vertex.xzxz + _Time.xxxx;

			//坐标
			half4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			half3 offset = Gerstner4(worldPos.xyz);
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.vertex.xyz += offset;

			//世界坐标法线
			o.normal = Gerstner4Normal(worldPos.xyz);

			//视线方向
			o.eyeDir.xyz = normalize(_WorldSpaceCameraPos - worldPos.xyz);

			o.screenPos = ComputeScreenPos(o.vertex);

			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col = _Color;
			UNITY_OPAQUE_ALPHA(col.a);

			fixed3 worldNormal = normalize(i.normal);
			fixed3 worldEyeDir = normalize(i.eyeDir);

			half4 bump = tex2D(_Bump, i.bumpCoord.xy);
			half3 bumpNormal = UnpackNormal(bump);

			//worldNormal += normalize(bumpNormal);

			//BlinnPhone模型
			half3 h = normalize(_WorldLightDir.xyz + i.eyeDir);	//半角向量
			float nh = max(0, dot(worldNormal, h));
			float spec = pow(nh, _Kspe);

			float diff = max(0,dot(_WorldLightDir.xyz, worldNormal));

			//
			half3 reflectVec = reflect(-worldEyeDir, worldNormal);
			half3 refractVec = refract(-worldEyeDir, worldNormal, _RefractRatio);

			//fresnel
			float4 fresnelReflectFactor = _FresnelScale + (1 - _FresnelScale)*pow(1 - dot(-worldEyeDir, worldNormal), 5);
			fixed4 colReflect = tex2D(_RefractionTex, i.screenPos);
			fixed4 colRefract = tex2D(_RefractionTex, i.screenPos);

			fixed4 fresnelCol = fresnelReflectFactor * colReflect + (1 - fresnelReflectFactor) * colRefract;

			col.rgb = _Color.rbg + diff * _LightColor.rbg + _LightColor.rbg * _SpeColor.rgb * spec + fresnelCol.rbg;
			col.a = _Color.a;
			return col;
		}

		//==================================================
		struct v2f_simple
		{
			float4 vertex : SV_POSITION;
			float2 bumpCoord : TEXCOORD0;
			float3 worldNormal : TEXCOORD1;
			float3 worldViewDir : TEXCOORD2;
		};

		v2f_simple vert_simple(appdata_t v)
		{
			v2f_simple o;

			//世界坐标
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

			//法线
			o.worldNormal = UnityObjectToWorldNormal(v.normal);

			//指向摄像机方向
			o.worldViewDir = _WorldSpaceCameraPos - worldPos.xyz;

			//指向光源方向
			//o.worldLightDir = UnityWorldSpaceLightDir(worldPos);

			//齐次空间坐标
			o.vertex = UnityObjectToClipPos(v.vertex);

			//纹理坐标
			o.bumpCoord = v.texcoord;

			return o;
		}

		v2f_simple vert_simple_gerstner(appdata_t v)
		{
			v2f_simple o;

			//世界坐标
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			half3 worldPosOffset = Gerstner4(worldPos.xyz);
			worldPos.xyz += worldPosOffset;

			//法线
			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			half3 worldNormalOffset = Gerstner4Normal(worldNormal);
			o.worldNormal = normalize(worldNormal + worldNormalOffset);

			//指向摄像机方向
			//o.worldViewDir = _WorldSpaceCameraPos - worldPos.xyz;
			o.worldViewDir = worldPos.xyz - _WorldSpaceCameraPos;

			//指向光源方向
			//o.worldLightDir = UnityWorldSpaceLightDir(worldPos);

			//齐次空间坐标
			o.vertex = mul(UNITY_MATRIX_VP, worldPos);

			//纹理坐标
			o.bumpCoord = v.texcoord;

			return o;
			
		}
		
		fixed4 frag_simple(v2f_simple i) : SV_Target
		{
			//模拟环境光
			fixed4 baseCol = _Color;

			half3 worldNormal = half3(0, 1, 0);

			//Lambert diff
			float diff = max(0, dot(i.worldNormal, _WorldLightDir.xyz)) * _Idiff;
			fixed4 diffCol = _LightColor * diff; 

			//Blinn-phong 镜面反射
			half3 h = normalize(_WorldLightDir.xyz + i.worldViewDir);	//半角向量
			float nh = max(0, dot(worldNormal, -h));
			float spec = max(0, pow(nh, _Shininess));
			fixed4 specCol = _Kspe * _SpeColor * spec;
			
			//叠加
			baseCol += diffCol + specCol;
			baseCol.a = _Color.a;

			return baseCol;
		}
		
		fixed4 frag_simple_bump(v2f_simple i) : SV_Target
		{
			fixed4 col = _Color;

			fixed4 bumpNormal = tex2D(_Bump, i.bumpCoord + _Time.x);
			i.worldNormal += UnpackNormal(bumpNormal) * _NormalScale;
			i.worldNormal = normalize(i.worldNormal);
			
			float diff = max(0, dot(i.worldNormal, _WorldLightDir.xyz)) * _Idiff;
			fixed4 diffCol = _LightColor * diff;

			half3 h = normalize(i.worldNormal + i.worldViewDir);
			float nh = max(0, dot(i.worldNormal, h));
			float spec = max(0, pow(nh, _Shininess));
			fixed4 specCol = _Kspe * _SpeColor * spec;

			col += diffCol + specCol;
			col.a = _Color.a;
			return col;
		}
		
		fixed4 frag_simple_reflect_skybox(v2f_simple i) : SV_Target
		{
			fixed4 col = _Color;
			
			fixed4 bumpNormal = tex2D(_Bump, i.bumpCoord + _Time.x);
			i.worldNormal += UnpackNormal(bumpNormal)* _NormalScale;
			i.worldNormal = normalize(i.worldNormal);

			//reflect
			half3 reflectDir = reflect(-i.worldViewDir, i.worldNormal);
			fixed4 refleCol = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflectDir);

			//refract
			half3 refractDir = refract(-i.worldViewDir, i.worldNormal, _RefractRatio);
			fixed4 refractCol = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, refractDir);

			//Fresnel
			float reflectionfactor = min(1.0, max(0, _FresnelBias + _FresnelScale * pow(1.0 + dot(normalize(-i.worldViewDir), i.worldNormal), _FresnelPower)));
			fixed4 fresnelCol = lerp(refleCol, refractCol, reflectionfactor);

			//diffuse
			float diff = max(0, dot(i.worldNormal, _WorldLightDir.xyz)) * _Idiff;
			fixed4 diffCol = _LightColor * diff;

			//specal
			half3 h = normalize(i.worldNormal + i.worldViewDir);
			float nh = max(0, dot(i.worldNormal, h));
			float spec = max(0, pow(nh, _Shininess));
			fixed4 specCol = _Kspe * _SpeColor * spec;

			col += diffCol + specCol + fresnelCol;
			col.a = fresnelCol.a;

			return col;
		}

		fixed4 frag_simple_water_gerstner(v2f_simple i) : SV_Target
		{
			fixed4 col = _Color;

			half3 worldNormal = normalize(i.worldNormal);
			half3 worldViewDir = normalize(i.worldViewDir);
			half3 worldLightDir = _WorldLightDir.xyz;

			fixed4 bumpNormal = tex2D(_Bump, i.bumpCoord + _Time.x);
			worldNormal += UnpackNormal(bumpNormal).xyz * _NormalScale;
			worldNormal = normalize(worldNormal);

			//reflect
			half3 reflectDir = reflect(-worldViewDir, worldNormal);
			fixed4 refleCol = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflectDir);

			//refract
			half3 refractDir = refract(-worldViewDir, worldNormal, _RefractRatio);
			fixed4 refractCol = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, refractDir);

			//Fresnel
			float reflectionfactor = min(1.0, max(0, _FresnelBias + _FresnelScale * pow(1.0 + dot(normalize(-worldViewDir), worldNormal), _FresnelPower)));
			fixed4 fresnelCol = lerp(refleCol, refractCol, reflectionfactor);

			//diffuse
			float diff = max(0, dot(worldNormal, worldLightDir)) * _Idiff;
			fixed4 diffCol = _LightColor * diff;

			//specal
			half3 h = normalize(worldLightDir + worldViewDir);
			float nh = max(0, dot(worldNormal, -h));
			float spec = max(0, pow(nh, _Shininess)) * _Kspe;
			fixed4 specCol = _SpeColor * spec * _SpeColor.a;

			col += diffCol + specCol;
			col = lerp(fresnelCol, col , _Color.a);
			col.a = _Color.a;

			return col;
		}
			
ENDCG

	SubShader{
		
		//LOD 500
		GrabPass{ "_RefractionTex" }
		
		/*
		Pass{
			Tags{"RenderType" = "Opaque"}
			LOD 200
			
			CGPROGRAM
			#pragma vertex vert_simple
			#pragma fragment frag_simple_reflect_skybox
			#pragma target 3.0
			ENDCG
		}
		*/
		
		
		Pass{
			Tags{ "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert_simple_gerstner
			#pragma fragment frag_simple_water_gerstner
			#pragma target 2.0
			#pragma multi_compile_fog
			
			ENDCG
		}
		
	}
}
