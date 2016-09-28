Shader "Custom/Challange1" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Smoothness ("Smoothness", Range(0,1)) = 0.5
		_MetallicTex("Metallic", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "white" {}
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

		struct Input {
			float2 uv_MainTex;
		};

		half _Smoothness;
		half _EmissionI;
		half _NormalI;
		fixed4 _EmissionCol;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = _Color;
			o.Metallic = tex2D(_MetallicTex, IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex) * _NormalI);
			o.Emission = tex2D(_EmissionTex, IN.uv_MainTex) * _EmissionCol * _EmissionI;
			// Metallic and smoothness come from slider variables
			o.Smoothness = _Smoothness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
