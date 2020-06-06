Shader "Hidden/RadialBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SampleDist("sampleDist",float) = 0
        _SampleStrength("sampleStrength",float) = 0
        _CenterX("x",float) = 0
        _CenterY("y",float) = 0
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
            float _SampleDist;
            float _SampleStrength;
            float  _CenterX;
            float  _CenterY;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 sum = col;
                float2 dir = 0.5 - i.uv + float2(_CenterX,_CenterY);
                float dist = length(dir);
                dir /= dist;
                float samples[10] =
                {
                   -0.08,
                   -0.05,
                   -0.03,
                   -0.02,
                   -0.01,
                   0.01,
                   0.02,
                   0.03,
                   0.05,
                   0.08
                };
                for (int it = 0; it < 10; it++) {
                    sum += tex2D(_MainTex, i.uv + dir * samples[it] * _SampleDist);
                }
                sum /= 11;
                float t = saturate(dist * _SampleStrength);
                float4 blur = lerp(col, sum, t);
                return blur;
            }
            ENDCG
        }
    }
}
