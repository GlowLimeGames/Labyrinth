Shader "Upft/Background" {
	Properties {
		_MainColor ("Base (RGB)", Color) = (0,0,0,0)
		_WaveColor ("Wave (RGB)", Color) = (1,1,1,1)
		_Speed ("Speed (float)", float) = 50
		_Band ("Band (float)", float) = 1
		_Division ("Division (float)", float) = 1
	}
	
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		float4 _MainColor;
		float4 _WaveColor;
		float _Speed;
		float _Band;
		float _Division;

		struct Input {
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float4 c = _WaveColor;
			float p = clamp(sin(_Time * _Speed + IN.worldPos.y * _Division) * _Band - (_Band - 1),0, 1);
			o.Albedo = c.rgb * p + _MainColor.rgb * (1-p);
			o.Alpha = c.a;
			o.Emission = o.Albedo;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
