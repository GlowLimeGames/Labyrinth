Shader "Upft/HexBack" {
	Properties {
		_MainTex("Unuse", 2D) = "white" {}
		_BaseColor ("Base Color (Color)", Color) = (1,1,1,1)
		_ProgressColor ("Progress Color (Color)", Color) = (0,1,0,1)
		_Progress ("Progress (float)", Range(0,1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		float4 _BaseColor;
		float4 _ProgressColor;
		float _Progress;		

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float4 c = _BaseColor;
			float2 tang = IN.uv_MainTex - float2(0.5,0.5);
			if(atan2 (tang.x, tang.y) + 3.1415 < _Progress * 3.1415 * 2){
				c = _ProgressColor;
			}
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = o.Albedo;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
