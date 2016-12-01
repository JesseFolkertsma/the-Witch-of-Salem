Shader "Custom/WaterShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Normal1("Normal Texture 1", 2D) = "bump" {}
		_Normal2("Normal Texture 2", 2D) = "bump" {}
		//_EmissionTex("Emission Texture", 2D) = "white" {}
		//_EmissionCol("Emission Color", Color) = (1,1,1,1)
		//_EmissionMask("Emission Mask", 2D) = "white" {}
		_ScrollSpeedN1("Scroll Speed normal", float) = 0.0
		_ScrollSpeedN2("Scroll Speed normal 2", float) = 0.0
		_ScrollSpeed("Scroll Speed albedo", float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		//sampler2D _EmissionTex;
		//sampler2D _EmissionMask;
		sampler2D _Normal1;
		sampler2D _Normal2;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		//fixed4 _EmissionCol;

		half _ScrollSpeedN1;
		half _ScrollSpeedN2;
		half _ScrollSpeed;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			//Scroll for normal map1
			fixed2 scrollUV = IN.uv_MainTex;
			fixed xScroll = _ScrollSpeedN1 * _Time;
			fixed yScroll = _ScrollSpeedN1 * _Time;
			scrollUV += fixed2(xScroll, yScroll);

			//Scroll for normal map2
			fixed2 scrollUV3 = IN.uv_MainTex;
			fixed xScroll3 = _ScrollSpeedN2 * _Time;
			fixed yScroll3 = _ScrollSpeedN2 * _Time;
			scrollUV3 += fixed2(xScroll3, yScroll3);

			//Scroll for albedo maps
			fixed2 scrollUV2 = IN.uv_MainTex;
			fixed xScroll2 = _ScrollSpeed * _Time;
			fixed yScroll2 = _ScrollSpeed * _Time;
			scrollUV2 += fixed2(xScroll2, yScroll2);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * tex2D(_MainTex, scrollUV2) * _Color;
			//fixed3 e = tex2D(_EmissionTex, scrollUV) * tex2D(_EmissionMask, IN.uv_MainTex) * _EmissionCol;
			fixed3 n = normalize(UnpackNormal(tex2D(_Normal1, scrollUV) + tex2D(_Normal2, scrollUV3)));
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			//o.Emission = e;
			o.Alpha = c.a;
			o.Normal = n;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
