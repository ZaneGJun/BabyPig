Shader "Game/WateGrid"
{
	Properties
	{
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_Color("Color", color) = (1,1,1,1)
		_OffsetValue("OffsetValue", float) = 0.1
		_OffsetCoefficient("OffsetCoefficient", float) = 0.1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		
		Cull Off
		ZWrite Off
		Lighting Off
		Blend One OneMinusSrcAlpha

		GrabPass
		{
			"_ScreenTexture"
		}

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
				float4 grabPos : TEXCOORD1;
			};

			fixed4 _Color;
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			sampler2D _ScreenTexture;
			float _OffsetCoefficient;
			float _OffsetValue;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 offset = tex2D(_NoiseTex, i.uv + _OffsetValue * _Time.xy);

				i.grabPos.xy += offset.xy * _OffsetCoefficient;
				fixed4 col = tex2Dproj(_ScreenTexture, i.grabPos) * _Color;

				return col;
			}
			ENDCG
		}
	}
}
