Shader "Custom/Skydom" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader{
		Tags{ "RenderType" = "Fade" }
		CGPROGRAM
#pragma surface surf Lambert alpha
	struct Input {
		float4 color : COLOR;
		float3 viewDir;
		float3 worldPos;
		float2 uv_MainTex;
	};
	sampler2D _MainTex;
	void surf(Input IN, inout SurfaceOutput o) {
		float fade = IN.worldPos.y / 200;
		if (fade > 1) {
			fade = 1;
		}
		else if (fade < 0) {
			fade = 0;
		}
		float3 c1 = tex2D(_MainTex, IN.uv_MainTex + float2(_Time[1] / 50, 0)).rgb;
		float3 c2 = tex2D(_MainTex, IN.uv_MainTex  + float2(- _Time[1] / 100, 0)).rgb;
		float3 c3 = tex2D(_MainTex, IN.uv_MainTex*2 + float2(0, - _Time[1] / 50)).rgb;

		float3 c = (c1 + c2 + c3)/3;
		o.Albedo = c;
		o.Emission = saturate(c);
		o.Alpha = fade*c*0.7*(1+o.Normal[1])*2;
		}
	ENDCG
	}
		Fallback "Diffuse"
}