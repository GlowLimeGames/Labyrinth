Shader "Upft/Hex" {
	Properties {
		_MainTex ("Unuse", 2D) = "white" {}
		_Color ("Base (RGB)", Color) = (1,1,1,1)
		}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		half4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = _Color;
			o.Albedo = lerp(c.rgb, 1, (1 - IN.uv_MainTex.y) * 0.5f);
			o.Alpha = c.a;
			o.Emission = o.Albedo;
		}
		ENDCG
	} 
	FallBack "Self-Illumin/Diffuse"
}
