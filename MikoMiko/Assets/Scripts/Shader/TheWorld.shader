Shader "Hidden/TheWorld"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _WaveTex("Wave Texture", 2D) = "white" {}
        _Radius("Radius",float) = 1.0
        _ImpactColor("ImpactColor",Color) = (1,1,1,1)
        _ImpactRadius("ImpactRadius",float) = 1.0
        _ImpactRadius1("ImpactRadius1",float) = 1.0
        _Gray("Gray",float) = 0
        _CenterX("x",float) = 0
        _CenterY("y",float) = 0
        _TwistIntensity("_TwistIntensity",float) = 0
        _TwistSpeed("_TwistSpeed",float) = 0
        _WaveIntensity("_TwistIntensity",float) = 0
        _WaveShape("_TwistSpeed",float) = 0


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
                float _Radius;
                fixed4 _ImpactColor;
                float _ImpactRadius;
                float _ImpactRadius1;
                float  _Gray;
                float  _CenterX;
                float  _CenterY;
                sampler2D _NoiseTex;
                sampler2D _WaveTex;
                float _TwistIntensity;
                float _TwistSpeed;
                float _WaveIntensity;
                float _WaveShape;



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

                sampler2D _MainTex;

                float random() {
                    return frac(sin(dot(_Time.xy, fixed2(12.9898, 78.233))) * 43758.5453123);
                }



                float3 RGB2HSV(float3 c)
                {
                    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
                    float d = q.x - min(q.w, q.y);
                    float e = 1.0e-10;
                    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
                }

                float3 HSV2RGB(float3 c)
                {
                        float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                        float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                        return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float twistSpeed = _TwistSpeed;
                    float twistIntensity = _TwistIntensity;
                    float waveShape = _WaveShape;
                    float waveIntensity = _WaveIntensity;
                    float aspect = _ScreenParams.x / _ScreenParams.y;
                    float x = (i.uv.x - 0.5f - _CenterX) * aspect;
                    float y = i.uv.y - 0.5f - _CenterY;
                    float d = x * x + y * y;
                    float sin_theta = saturate( y / sqrt(d));
                    float half_theta = asin(sin_theta) * (step(0,x) - 0.5);


                    float4 noise = tex2D(_NoiseTex, i.uv + _Time.y * twistSpeed);

                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed4 finalColor = col;
                    fixed4 reversedColor = col;

                    float deformFactor = (1 + 0.02 * sin(half_theta * 24) * lerp(0, 0.5, sin(UNITY_TWO_PI * _Time.y * 0.5))
                        + 0.25 * x * sin(1 + half_theta * 6.5) * lerp(0.25, 0.75, sin(UNITY_TWO_PI * _Time.y * 0.2))
                        + 0.1 * x * x * sin(2 + half_theta * 9.5) * lerp(0.25, 0.75, sin(UNITY_TWO_PI * _Time.y * 0.1))
                        );

                    _Radius *= deformFactor;
                    _ImpactRadius *= deformFactor;
                    _ImpactRadius1 *= deformFactor;

                    fixed4 twistedColor = tex2D(_MainTex, i.uv + noise.xy * twistIntensity);
                    fixed4 wave = tex2D(_NoiseTex, half_theta * 2 + noise.xy * waveShape) * waveIntensity;

                    float3 hsvColor = RGB2HSV(twistedColor.rgb);
                    hsvColor.x += lerp(0,0.2,sin(UNITY_TWO_PI * frac(_Time.y * 0.5)));
                    hsvColor.x = frac(hsvColor.x);
                    reversedColor.rgb = 1 - HSV2RGB(hsvColor.rgb) + wave;
                    float rr = _Radius * _Radius;
                    half isGray = step(0.5, _Gray);
                    half insideCircle = step(d, rr);
                    finalColor = lerp(col, reversedColor, insideCircle);
                    fixed3 grayFactor = { 0.299,0.587,0.114 };
                    fixed grayColor = dot(grayFactor, col);
                    finalColor = lerp(finalColor, grayColor, isGray * (1 - insideCircle));

                    //impact wave
                    float power = 5;

                    float t = saturate(d / (_ImpactRadius * _ImpactRadius));
                    fixed4 rim = lerp(0, _ImpactColor, pow(t, power));
                    finalColor += rim * rim.a * step(d, _ImpactRadius * _ImpactRadius);

                    t = saturate(d / (_ImpactRadius1 * _ImpactRadius1));
                    rim = lerp(0, _ImpactColor, pow(t, power));
                    finalColor += rim * rim.a * step(d, _ImpactRadius1 * _ImpactRadius1);

                    return finalColor;
                }
                ENDCG
            }
        }
}