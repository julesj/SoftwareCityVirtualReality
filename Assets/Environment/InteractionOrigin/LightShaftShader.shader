Shader "Custom/Light Shaft" {
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
	};
	sampler2D _MainTex;
	void surf(Input IN, inout SurfaceOutput o) {
		
		float3 projectedViewDir = IN.viewDir;
		float3 projectedNormal = o.Normal;
		projectedViewDir[1] = 0;
		projectedNormal[1] = 0;
		half rim = saturate(dot(normalize(projectedViewDir), normalize(projectedNormal)));
		
		float fade = IN.worldPos.y / 3;
		if (fade > 1) {
			fade = 1;
		}
		else if (fade < 0) {
			fade = 0;
		}
		float scale = 20;
		fixed3 col1 = tex2D(_MainTex, float2(_Time[1]/50 + IN.worldPos.x / scale, IN.worldPos.y / scale)).rgb;
		fixed3 col2 = tex2D(_MainTex, float2(IN.worldPos.y / scale, _Time[1] / 100 + IN.worldPos.z / scale)).rgb;
		fixed3 col;
		float z = abs(IN.worldPos.z / 20);
		float x = abs(IN.worldPos.x / 20);
		col = float3(col1[0] * z + col2[0] * x, col1[1] * z + col2[1] * x, col1[2] * z + col2[2] * x);
		o.Albedo = col;
		o.Emission = normalize(col);
		float weight = ((col[0] + col[1] + col[2]) / 3);
		o.Alpha = weight* 0.1 * pow(rim, 10) * fade;
		}
	ENDCG
	}
		Fallback "Diffuse"
}