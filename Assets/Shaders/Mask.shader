Shader "Pablo/Mask" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_MaskColor("Mask Color", Color) = (1, 1, 1, 1)
			//_MaskAmount("Mask Amount", Range(0.0, 1.0)) = 0.0
			_MaskAmount("Mask Amount", Range(0.0, 1.0)) = 0.0
			//[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

	}
	SubShader
		{
			Tags{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}
			Cull Off
			Lighting Off
			ZWrite Off
			Fog{ Mode Off }
			Blend One OneMinusSrcAlpha
			Pass
				{
					CGPROGRAM
#pragma vertex vert             
#pragma fragment frag             
//#pragma multi_compile DUMMY PIXELSNAP_ON             
#include "UnityCG.cginc"             
					struct appdata_t
					{
						float4 vertex   : POSITION;
						float4 color    : COLOR;
						float2 texcoord : TEXCOORD0;
					};

					struct v2f
					{
						float4 vertex   : SV_POSITION;
						fixed4 color : COLOR;
						half2 texcoord  : TEXCOORD0;
					};

					
					fixed4 _MaskColor;
					float _MaskAmount;
					
					v2f vert(appdata_t IN)
					{
						v2f OUT;
						OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
						OUT.texcoord = IN.texcoord;
						OUT.color = IN.color ;
/*
#ifdef PIXELSNAP_ON                 
						OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif
						'*/
						return OUT;
					}

					sampler2D _MainTex;
					fixed4 frag(v2f IN) : SV_Target
					{
						fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
						float x = _MaskAmount;
						if (c.r < x && IN.texcoord.y<_MaskAmount)
							//c.rgb = lerp(c.rgb, _MaskColor.rgb , x);
						{
							c.r = 0.0;
							c.b = 0.0;
							c.g = 0.0;
						}
							
						c.rgb *= c.a;
						return c;
					}
						ENDCG

				}
		}
}