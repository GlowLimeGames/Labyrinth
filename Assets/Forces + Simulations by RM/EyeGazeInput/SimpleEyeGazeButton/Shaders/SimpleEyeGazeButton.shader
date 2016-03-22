Shader "Upft/SimpleEyeGazeButton" {
Properties {
	_MainTex("BackGround Color (RGB) Trans (A)", 2D) = "black" {}
	_FrameTex ("Frame BG Color (RGB) Trans (A)", 2D) = "black" {}
	_FrameFillTex ("Frame Fill Color (RGB) Trans (A)", 2D) = "white" {}
	_FrameMask ("Frame Fill Mask (A)", 2D) = "white" {}
	_Transition ("Fill Transition (Gray)", 2D) = "white"{}
	_Fill("Fill (0-1)", Range(0,1)) = 0
	_FillBlur("Blur (0-1)", Range(0.1,1)) = 0.1
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _FrameTex;
			sampler2D _FrameFillTex;
			sampler2D _FrameMask;
			sampler2D _Transition;
			float4 _MainTex_ST;
			
			fixed _Fill;
			fixed _FillBlur;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 addTex(fixed4 base, fixed4 add){
				fixed4 col;
				col.rgb = lerp(base.rgb, add.rgb, add.a);
				col.a = base.a + (1-base.a) * add.a;
					
				return col;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				col = addTex(col, tex2D(_FrameTex, i.texcoord));
				fixed m = clamp((_Fill * (1+_FillBlur) - tex2D(_Transition, i.texcoord).r * 0.999) / _FillBlur, 0, 1);
				fixed4 tex = tex2D(_FrameMask, i.texcoord).a * tex2D(_FrameFillTex, i.texcoord);
				tex.a *= m;
				col = addTex(col, tex);
				return col;
			}
		ENDCG
	}
}

}
