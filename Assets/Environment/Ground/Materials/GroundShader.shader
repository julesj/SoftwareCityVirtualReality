Shader "Custom/GroundShader" {
	Properties {
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		struct Input {
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;

		float2 chess(float3 worldPos) {
			return float2(worldPos.x - floor(worldPos.x), worldPos.z - floor(worldPos.z));
		}

		float emission(float2 chessv, float borderw) {
			if (chessv.x < borderw || chessv.y < borderw) {
				return 1;
			}
			else if (chessv.x > 1 - borderw || chessv.y > 1 - borderw) {
				return 1;
			}
			else {
				return 0;
			}
		}

		bool even(float3 worldPos) {
			return ((int)abs(worldPos.x+1000) + (int)abs(worldPos.z+1000)) % 2 == 0;
		}

		float clamp(float v) {
			if (v < 0) {
				return 0;
			}
			else if (v > 1) {
				return 1;
			}
			return v;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float2 chessv = chess(IN.worldPos);
			float2 chessv2 = chess(IN.worldPos * 2);
			float2 chessv4 = chess(IN.worldPos * 4);

			float3 baseCol = float3(0.2, 0.3, 1);
			float dist = distance(_WorldSpaceCameraPos, IN.worldPos);

			o.Albedo = baseCol / 50;
			float emission1 = emission(chessv, 0.025) * 0.125 / dist / 40;
			float emission2 = emission(chessv2, 0.0125) * 0.25 / dist / 40;
			float emission4 = emission(chessv4, 0.0125) * 0.25 / dist / 40;

			float highlight = sin(_Time[1] + IN.worldPos.x)
				+ sin(_Time[1] / 20 + IN.worldPos.x / 5)
				+ sin(_Time[1] /10 + IN.worldPos.z / 10)
				+ sin(_Time[1] / 20 + IN.worldPos.z / 5);

			float3 borderCol =
				(even(IN.worldPos) ? baseCol / 50 : baseCol / 100) / dist
				+ baseCol * emission1 * clamp(highlight) * 10
				+ baseCol * emission2 
				+ baseCol * emission4;


				
			o.Emission = borderCol;
			o.Metallic = 0;
			o.Smoothness = 0;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
