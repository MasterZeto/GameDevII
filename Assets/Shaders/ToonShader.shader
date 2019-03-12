Shader "Custom/ToonShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Metallic ("Metallic Map", 2D) = "black" {}
        _Normal ("Normal Map", 2D) = "black" {}
        _Emission ("Emission Map", 2D) = "black" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Threshold ("Threshold", Range(0, 1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf ToonLighting fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        sampler2D _Metallic;
        sampler2D _Normal;
        sampler2D _Emission;
        fixed4 _Color;
        float _Threshold;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = tex2D (_Metallic, IN.uv_MainTex);
            o.Normal = IN.Normal + tex2D (_Normal, IN.uv_MainTex);
            o.Emission = tex2D (_Emission, IN.uv_MainTex);
            o.Alpha = c.a;
        }

        half4 LightingToonLighting (SurfaceOutput s, half3 lightDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            half4 c;
            c.rgb = s.Albedo * _LightColor0 * step(_Threshold, NdotL) * atten;
            c.a = s.Alpha;
            return c;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
