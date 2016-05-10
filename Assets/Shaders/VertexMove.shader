Shader "Custom/Digi Extrusion" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      struct Input {
          float2 uv_MainTex;
      };
      void vert (inout appdata_full v) {
          v.vertex.xyz += v.normal * (sin(_Time.w*3)*0.08-0.07);
		  
      }
      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;

		  if(sin(_Time.w*2)>0.9)
		  {
		  o.Albedo = float3(0,0,1);
		  }



      }
      ENDCG
    } 
    Fallback "Diffuse"
  }