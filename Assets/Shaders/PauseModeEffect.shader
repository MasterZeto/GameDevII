Shader "Hidden/PauseModeEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Intensity ("Intensity", Float) = 0.0
        _Brightness ("Brightness", Range(-1, 1)) = 0.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _Color;
            float _Intensity;
            float _Brightness;

            float3 rgb_to_hsv(float3 c)
            {
                float4 k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, k.wz), float4(c.gb, k.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float3 hsv_to_rgb(float3 c)
            {
                float4 k = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + k.xyz) * 6.0 - k.www);
                return c.z * lerp(k.xxx, clamp(p - k.xxx, 0.0, 1.0), c.y);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float y = (0.299 * col.r) + (0.587 * col.g) + (0.114 * col.b);
                // just invert the colors
                float3 hsv_col = rgb_to_hsv((_Color * y).rgb);
                hsv_col.y = hsv_col.y * _Intensity;
                hsv_col.z = clamp(hsv_col.z + _Brightness, 0, 1);
                col.rgb = hsv_to_rgb(hsv_col); //clamp(y + _Intensity, 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
