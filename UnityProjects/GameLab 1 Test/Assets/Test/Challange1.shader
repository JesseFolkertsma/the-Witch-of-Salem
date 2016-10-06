Shader "Custom/Challange1" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Smoothness ("Smoothness", 2D) = "white" {}
		_MetallicTex("Metallic", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "bump" {}
		_NormalI ("Normal Intensity", Range(0,1)) = 0.5
		_EmissionTex ("Emission Texture", 2D) = "white" {}
		_EmissionCol ("Emission Color", Color) = (1,1,1,1)
		_EmissionI ("Emission Intensity", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MetallicTex;
		sampler2D _NormalTex;
		sampler2D _EmissionTex;
		sampler2D _SmoothnessTex;

		struct Input {
			float2 uv_NormalTex;
			float2 uv_EmissionTex;
			float2 uv_MetallicTex;
			float2 uv_SmoothnessTex;
		};

		half _EmissionI;
		half _NormalI;
		fixed4 _EmissionCol;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed3 m = tex2D(_MetallicTex, IN.uv_MetallicTex);
			fixed3 e = tex2D(_EmissionTex, IN.uv_EmissionTex);
			fixed3 s = tex2D(_SmoothnessTex, IN.uv_SmoothnessTex);
			fixed3 n = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex) * _NormalI);
			o.Albedo = _Color;
			o.Metallic = m;
			o.Normal = n;
			o.Emission = e * _EmissionCol * _EmissionI;
			o.Smoothness = s;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
