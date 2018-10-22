Shader "Test/Shadow/ShadowCatch"
{
	SubShader
	{
		Tags { "RenderType"="Opaque" }
        //LOD 100

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
				float4 vertex : SV_POSITION;
				float2 depth: TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.depth = o.vertex.zw;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				float depth = i.depth.x / i.depth.y;
				//depth范围[-1,1]
				fixed4 col = EncodeFloatRGBA(depth);
				return col;
			}
			ENDCG
		}
	}
}
