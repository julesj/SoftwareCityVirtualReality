Shader "Custom/SelectionShader" {
Properties{
	_Color("Color", Color) = (1,1,1,1)
	_MainTex("Base (RGB)", 2D) = "	" {}
}
SubShader{
	Tags{ "RenderType" = "Transparent" }

	CGPROGRAM
	// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert alpha 

	// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0


struct Input {
	float3 viewDir;
	float3 worldPos;
};


fixed4 _Color;

void surf(Input IN, inout SurfaceOutput o) {
	float c = ((sin(_Time[1] * 3) + 1) / 2) * 0.5 + 0.3;
	o.Alpha = c;
	o.Emission = _Color*c;
}
ENDCG
}
FallBack "Diffuse"
}
