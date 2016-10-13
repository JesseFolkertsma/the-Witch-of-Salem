Shader "Custom/EmissionWithMask" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_EmissionTex("Emission Texture", 2D) = "white" {}
		_EmissionCol("Emission Color", Color) = (1,1,1,1)
		_EmissionMask("Emission Mask", 2D) = "white" {}
		_ScrollSpeed("Scroll Speed", float) = 0.0
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
		sampler2D _EmissionTex;
		sampler2D _EmissionMask;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _EmissionCol;

		half _ScrollSpeed;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed2 scrollUV = IN.uv_MainTex;

			fixed xScroll = _ScrollSpeed * _Time;
			fixed yScroll = _ScrollSpeed * _Time;

			scrollUV += fixed2(xScroll, yScroll);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed3 e = tex2D(_EmissionTex, scrollUV) * tex2D(_EmissionMask, IN.uv_MainTex) * _EmissionCol;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Emission = e;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
