Shader "Combo Combat/Rock Shader" {
	Properties {
		_AmbientColor ("Ambient Color (Linear)", Color) = (0,0,0,0)
		_SpecularColor("Frontal Specular Color (Linear)", Color) = (.3,.3,.3,0)
		
		_GlossinessFactor ("Glossiness", Range(1.0, 100.0)) = 15
		_FrontalGlossiness ("Frontal Glossiness", Range(1.0, 1000.0)) = 200
		_DiffuseMap ("Diffuse Map", 2D) = "gray" {}
		_BumpMap ("Normal Map", 2D) = "gray" {}
		
		_RimFalloff ("Rim Ramp", 2D) = "black" {}
		_RimIntensity ("Rim intensity", Color) = (.5,.5,.5, 0)
		_RimPower ("Rim Intensity", Range(0.5, 2.0)) = 1.5
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf CandyShader
		#pragma target 3.0

		struct Input {
			half2 uv_DiffuseMap;
		};	

		sampler2D _DiffuseMap;
        sampler2D _BumpMap;
		sampler2D _RimFalloff;

		half4 _AmbientColor;
		half4 _SpecularColor;
		half4 _FrontalColor;
		half4 _RimIntensity;

		half _GlossinessFactor; 
		half _FrontalGlossiness;
		half _RimPower;

		half4 LightingCandyShader(SurfaceOutput s, half3 lightDir, half3 approxview, half atten){
			half cosine = dot(s.Normal, lightDir);
			half3 lightInfluence = lerp(_LightColor0.rgb, 1f, .5f);

			// Base diffuse with shadow atten and an ambient factor
			half3 diffuseColor = s.Albedo * lightInfluence *  (cosine * atten + pow(_AmbientColor.rgb, 2f) * .8f);

			// The rim is straightforward, using a ramp and a little exponential curve.
			// However, it is affected by shadows, so it is more subtle on unlit areas, also considering ambient factor
			half facingRatio = pow(saturate(dot(approxview, s.Normal)), _RimPower);
			half3 rimColor = tex2D(_RimFalloff, float2(0,facingRatio)).rgb * lerp(float3(1f), _AmbientColor.rgb, step(atten, .25f)) * saturate(atten + .25f) * lightInfluence; 

			// Blinn instead of Phong
			half3 h = normalize(lightDir + approxview);

			// Specular Color is transformed to prevent the Gamma correction that Unity applies previously.
			// This is useful to know exactly what is the multiplier to the specular highlights
			half3 spec = pow(saturate(dot(s.Normal, h)), _GlossinessFactor)* pow(_SpecularColor.rgb, .44) * lightInfluence * atten; 

			// Some Frontal reflection to fake environment (could import a cubemap here//TODO)
			half3 frontalBlinn = pow(saturate(dot(normalize(s.Normal + approxview), s.Normal)), _FrontalGlossiness) * _AmbientColor.rgb * _FrontalColor.rgb;

			return half4(diffuseColor + rimColor * _RimIntensity + spec + frontalBlinn, 1f);
		}

		void surf (Input IN, inout SurfaceOutput o) {
		    o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_DiffuseMap));
			o.Albedo = tex2D(_DiffuseMap, IN.uv_DiffuseMap);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
