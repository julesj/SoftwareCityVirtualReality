Shader "Custom/SelectionBeamShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "	" {}
	}
	SubShader {
		Tags { "RenderType"="Fade" "IgnoreProjector" = "true" }
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha 

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldPos;
		};


		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			float distanceFade = 1-IN.uv_MainTex.y;

			fixed4 c = _Color * tex2D(_MainTex, float2(_Time[1] / 50 + IN.worldPos.x / 10, IN.worldPos.y / 10)).rgba;

			o.Alpha =  (pow((sin(IN.uv_MainTex.y * 500 - 30 * _Time[1]) + 1) / 2, 1) * (c[0] + c[1] + c[2]) / 3);
			o.Emission = c * 5;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
