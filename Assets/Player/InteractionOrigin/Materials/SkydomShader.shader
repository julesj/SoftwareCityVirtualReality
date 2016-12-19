Shader "Custom/Skydom" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader{
		Tags{ "RenderType" = "Fade" "Queue" = "Transparent" "IgnoreProjector" = "true" }
		Fog{ Mode Off }
		Lighting Off
		CGPROGRAM
#pragma surface surf Lambert alpha
	struct Input {
		float4 color : COLOR;
		float3 viewDir;
		float3 worldPos;
		float2 uv_MainTex;
	};
	sampler2D _MainTex;
	
	float clamp(float v) {
		if (v < 0) {
			return 0;
		}
		else if (v > 1) {
			return 1;
		}
		return v;
	}
	
	void surf(Input IN, inout SurfaceOutput o) {
		float fadeBottom = clamp(IN.worldPos.y / 200);
		float fadeTop = 1 - clamp((IN.worldPos.y-100) / 180);
		
		float3 c1 = tex2D(_MainTex, IN.uv_MainTex + float2(_Time[1] / 50, 0)).rgb;
		float3 c2 = tex2D(_MainTex, IN.uv_MainTex  + float2(- _Time[1] / 100, 0)).rgb;
		float3 c3 = tex2D(_MainTex, IN.uv_MainTex*2 + float2(0, - _Time[1] / 50)).rgb;

		float3 c = (c1 + c2 + c3)/3;
		o.Albedo = saturate(c);
		o.Emission = saturate(c)*0.6f;
		o.Alpha = fadeBottom * fadeTop * c;
	}
	
	ENDCG
	}
		Fallback "Diffuse"
}