Shader "Ink/Blend"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" { }
    }
    SubShader
    { 
        Blend One OneMinusSrcAlpha
        Cull Back
        ZWrite Off ZTest Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 mainColor = tex2D(_MainTex, i.uv);
                // half alpha = tex2D(_AlphaTex, i.uv).r; 
                // mainColor.a *= alpha;
                return mainColor;
            }
            ENDCG
        }
    }
}
