Shader "Custom/Shade" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_EmissionTexture ("Emission Texture", 2D) = "white"{}
		_EmissionColor ("Emission Color", Color) = (1,0,0,1)
		_EmissionI ("Emission Intentisty", Range(0,1)) = 0.0
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
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

		sampler2D _MainTex;
		sampler2D _EmissionTexture;

		struct Input {
			float2 uv_MainTex;
			float2 uv_EmissionTexture;
		};

		half _Glossiness;
		half _Metallic;
		half _EmissionI;
		fixed4 _Color;
		fixed4 _EmissionColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 a = tex2D(_EmissionTexture, IN.uv_EmissionTexture) * _EmissionColor * _EmissionI;
			o.Albedo = c.rgb;
			o.Emission = a.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			//o.Emission = _EmissionColor * _EmissionI;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
