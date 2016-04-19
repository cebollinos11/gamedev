Shader "Pablo/Water/Half" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_WaterLevel("Water Level",Range(0.0,1.0)) = 1.0
			_WaterAmplitude("Water Amplitude", Range(0, 0.04)) = 0.02
		

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

					float _WaterLevel;
					
					
					v2f vert(appdata_t IN)
					{
						v2f OUT;
						OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex*float4(1, 1, 1, 1));
						OUT.texcoord = IN.texcoord;
						OUT.color = IN.color;
/*
#ifdef PIXELSNAP_ON                 
						OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif
						'*/
						return OUT;
					}

					sampler2D _MainTex;
					float _WaterAmplitude;
					fixed4 frag(v2f IN) : SV_Target
					{
						fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;

						if (IN.texcoord.y <= _WaterLevel)
						{
							float diff = sin(_Time.x * 45 + IN.texcoord.y * 90) *  _WaterAmplitude;
							
							c = tex2D(_MainTex, float2(IN.texcoord.x - diff, IN.texcoord.y + diff)) * IN.color;
							//c.b = 1.0;
							
						}
						
						c.rgb *= c.a;
						return c;
					}
						ENDCG

				}
		}
}