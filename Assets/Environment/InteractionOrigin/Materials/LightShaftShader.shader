Shader "Custom/Light Shaft" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader{
		Tags{ "Queue" = "Fade" "RenderType" = "Transparent" "IgnoreProjector" = "true" }
		Lighting Off
		CGPROGRAM
#pragma surface surf Lambert alpha
	struct Input {
		float4 color : COLOR;
		float3 viewDir;
		float3 worldPos;
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
		
		float3 projectedViewDir = IN.viewDir;
		float3 projectedNormal = o.Normal;
		projectedViewDir[1] = 0;
		projectedNormal[1] = 0;
		half rim = dot(normalize(projectedViewDir), normalize(projectedNormal));
		
		float fade = clamp((IN.worldPos.y - 1) / 1000);
		
		
		float3 col = float3(0.6, 0.6, 1);
		o.Emission = col;
		o.Alpha = (pow(rim, 10)) * fade;
		}
	ENDCG
	}
		Fallback "Diffuse"
}