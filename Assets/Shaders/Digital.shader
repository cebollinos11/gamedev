Shader "Pablo/DigitalCamera" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_CRTTex("CRT texture", 2D) = "white" {}
		

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
					sampler2D _CRTTex;
					fixed4 frag(v2f IN) : SV_Target
					{
						fixed4 c =  tex2D(_MainTex, IN.texcoord) * IN.color;
						
						fixed4 crt = tex2D(_CRTTex, IN.texcoord + half2(_Time.z*0.3*0, -_Time.z*0.3));
						
						//c.r = lerp(c.r, crt.r,0.5);
						//c.b

						//c.rgb = lerp(c.rgb, crt.rgb, 0.2);

						c.g = lerp(c.g,crt.g,0.15);

						c.rgb *= c.a;
						return c;
					}
						ENDCG

				}
		}
}