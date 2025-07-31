Shader "Ink/Splatmask"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "black" {}
        _InkColor("Painter Color", Color) = (1,0,1,1)
    }
        SubShader
    {
        Cull Back ZWrite Off ZTest Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag=
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float3 _SplatPos;
            float _Radius;
            float _Hardness;
            float _Strength;
            float4 _InkColor;
            float4 _PainterColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            float2 convNDC(float2 uv)
            {
                return uv.xy * 2 - 1;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyzw;
                o.vertex = float4(convNDC(v.uv) * float2(1, _ProjectionParams.x), 0, 1);
                return o;
            }

            // Calculate the splat effect
            float mask(float3 pos, float3 center, float radius, float hardness)
            {
                float dist = distance(pos, center);
                return smoothstep(radius, radius * hardness, dist);
            }

            // Fragment shader
            float4 frag (v2f i) : SV_Target
            {
                // Get texture color
                float4 col = tex2D(_MainTex, i.uv);
                // Calculate splat effect
                float m = mask(i.worldPos, _SplatPos, _Radius, _Hardness) * _Strength;
                // Blend texture color with splat color
                return lerp(col, _InkColor, m);
            }
            ENDCG
        }
    }
}
