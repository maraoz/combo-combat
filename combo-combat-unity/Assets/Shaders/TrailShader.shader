Shader "Combo Combat/Wall Shader" {

    Properties {
        _TrailColor ("Trail Color", Color) = (0.5,0.5,0.5,1)
        _ReflectionColor ("Reflection Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Noise Texture", 2D) = "white" {}
        _HexTex ("Hex Pattern", 2D) = "white" {}
        _BaseTex ("Base Texture", 2D) = "white" {}
        _ReflMap ("Reflection map", CUBE) = "" {}
        _NormalMap ("Normal map", 2D) = "bump" {}
    }

    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha One
        Cull Off Lighting Off ZWrite Off

        CGPROGRAM
        #pragma surface surf TrailShader nolightmap nodirlightmap noambient noforwardadd
        #pragma target 3.0

        // NOTE: This pragma makes the shader available to GL machines, because it seems that
        // even using SM 3.0 as target, it wont let us use more than 8 tex coords.
        #pragma glsl

        half4 _TrailColor;
        half4 _ReflectionColor;

        half4 LightingTrailShader (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half facingRatio = 1f - pow(abs(dot(s.Normal, viewDir)),2f);

            s.Gloss *= facingRatio;

            half4 c;
            c.rgb = s.Albedo* _TrailColor * (1.5f + s.Gloss * 2f) + s.Gloss * _ReflectionColor.rgb * s.Albedo;

            c.a = s.Alpha;
            return c;
        }

        struct Input {
            float4 color : COLOR;
            float2 uv_MainTex;
            float2 uv_HexTex;
            float2 uv_BaseTex;
            float2 uv_NormalMap;
            float3 worldRefl;
            INTERNAL_DATA
        };

        sampler2D _MainTex;
        sampler2D _HexTex;
        sampler2D _BaseTex;
        sampler2D _NormalMap;
        samplerCUBE _ReflMap;

        void surf (Input IN, inout SurfaceOutput o) {
            fixed3 base = tex2D (_BaseTex, IN.uv_BaseTex).rgb;
            fixed3 hex = tex2D (_HexTex, IN.uv_HexTex).rgb;
            fixed3 noise = tex2D(_MainTex, IN.uv_MainTex).rgb;

            o.Albedo = base + hex * noise;
            o.Alpha = IN.color.a - (1f - base) * .3f;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
            o.Gloss = texCUBE(_ReflMap, WorldReflectionVector(IN, o.Normal));
        }

        ENDCG 
    } 

    FallBack "Diffuse"
}
