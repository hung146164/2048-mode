Shader "Custom/RoundedSquareSDF"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint Color", Color) = (1,1,1,1)
        _CornerRadius("Corner Radius", Range(0,0.5)) = 0.1
        _EdgeSoftness("Edge Softness", Range(0,0.1)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;         // Màu Tint từ SpriteRenderer
            float _CornerRadius;   // Độ bo góc
            float _EdgeSoftness;   // Độ mượt của đường bo

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Hàm tính khoảng cách đến biên của 1 hình vuông bo góc (SDF)
            float roundedBoxSDF(float2 p, float2 b, float r)
            {
                float2 d = abs(p) - b + r;
                float outside = length(max(d, 0.0)) - r;
                float inside  = min(max(d.x, d.y), 0.0);
                return outside + inside;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 col = tex2D(_MainTex, uv);

                // Áp dụng màu Tint
                col *= _Color;

                float2 p = uv - 0.5;
                float2 b = float2(0.5 - _CornerRadius, 0.5 - _CornerRadius);
                float dist = roundedBoxSDF(p, b, _CornerRadius);
                float alpha = 1.0 - smoothstep(0.0, _EdgeSoftness, dist);

                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}
