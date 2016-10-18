Shader "Custom/Light Shaft" {
	SubShader{
		Tags{ "RenderType" = "Fade" }
		CGPROGRAM
#pragma surface surf Lambert alpha
	struct Input {
		float4 color : COLOR;
		float3 viewDir;
		float3 worldPos;
	};
	void surf(Input IN, inout SurfaceOutput o) {
		o.Albedo = 1;
		half rim = saturate(dot(normalize(IN.viewDir), o.Normal));
		o.Emission = fixed3(1, 1, 1);
		o.Alpha = 0.01 * pow(rim, 4);// *sin(-_Time[1] * IN.worldPos.x / 10);
			
			// (sin(_Time[1]*IN.worldPos.x) + sin(-_Time[1] *IN.worldPos.y) + sin(IN.worldPos.z/10) + sin(_Time[1]+IN.worldPos.x/100) + sin(200+IN.worldPos.y/120) + sin(_Time[1]+IN.worldPos.z));
	}
	ENDCG
	}
		Fallback "Diffuse"
}