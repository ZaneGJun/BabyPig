Shader "UI/Game/SpriteDefault_LightDisturbed"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		_LightTex("LightTex", 2D) = "white" {}
		_speed("Speed",Float) = 1
		_dirX("DirX", Range(-1,1)) = 1
		_dirY("DirY", Range(-1,1)) = 1
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex SpriteVert
			#pragma fragment frag

			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			
			#include "UnityCG.cginc"
			#include "UnitySprites.cginc"

			sampler2D _LightTex;
			float _speed;
			fixed _dirX;
			fixed _dirY;
			
			fixed4 frag (v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				float2 lightUV = IN.texcoord;
				lightUV.x += _speed * _dirX * _Time.x;
				lightUV.y += _speed * _dirY * _Time.y;
				fixed4 lightCol = tex2D(_LightTex, lightUV);

				fixed4 addColor = fixed4(lightCol.r*lightCol.a, lightCol.g*lightCol.a, lightCol.b*lightCol.a, lightCol.a);
				c.rgb += addColor.rgb;

				c.rgb *= c.a;
				return c;
			}
			ENDCG
		}
	}
}
