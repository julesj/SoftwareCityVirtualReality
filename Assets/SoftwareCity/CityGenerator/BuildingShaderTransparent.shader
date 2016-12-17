Shader "Custom/BuildingShader Transparent" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "true" }

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf BlinnPhong alpha fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float3 worldPos;
		};

		fixed4 _Color;

		float clamp01(float val) {
			if (val > 1) {
				return 1;
			}
			else if (val < 0) {
				return 0;
			}
			return val;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			float dist = distance(_WorldSpaceCameraPos, IN.worldPos);
			fixed4 c = _Color;
			o.Albedo = c.rgb;
			o.Alpha = (1 - clamp01(IN.worldPos.y))/5 + (0.5 - (1-clamp01(dist / 2)) * 0.5);
		}

		ENDCG
	}
	FallBack "Diffuse"
}
