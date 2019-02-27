Shader "Custom/ExtendedToonShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Specular ("Specular", Range(0, 1)) = 0.0
        _Specular_Power ("Specular Power", Float) = 0.0
        _Normal ("Normal Map", 2D) = "black" {}
        _Emission ("Emission Map", 2D) = "black" {}
        _Threshold ("Threshold", Range(-1, 1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Toon fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Normal;
        };

        fixed4 _Color;
        sampler2D _Normal;
        sampler2D _Emission;
        float _Specular;
        float _Specular_Power;
        float _Threshold;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Specular = _Specular;
            o.Gloss = _Specular_Power;
            o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));
            o.Emission = tex2D(_Emission, IN.uv_MainTex);
            o.Alpha = c.a;
        }

        half4 LightingToon(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        {
            float NdotL = dot(s.Normal, lightDir);
            float diffuse = step(_Threshold, NdotL) * step(0.5, saturate(atten));

            float3 diffuse_color = diffuse * s.Albedo * _LightColor0;

            float NdotH = dot(s.Normal, (normalize(lightDir + viewDir)));

            float specular = pow(saturate(NdotH), s.Gloss);

            float3 specular_color = specular * float3(1,1,1) * _LightColor0;

            return half4(diffuse_color , s.Alpha);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
