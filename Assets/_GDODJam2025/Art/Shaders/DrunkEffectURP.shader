Shader "Hidden/DrunkEffectURP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TimeValue ("Time", Float) = 0
        _WaveStrength ("Wave Strength", Float) = 0.1
        _Speed ("Speed", Float) = 1.0
        _Distortion ("Distortion", Float) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Name "DrunkEffectURP"
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;

            float _TimeValue;
            float _WaveStrength;
            float _Speed;
            float _Distortion;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float2 Distort(float2 uv)
            {
                float waveX = sin(uv.y * 10 + _TimeValue * _Speed) * _WaveStrength;
                float waveY = cos(uv.x * 10 + _TimeValue * _Speed) * _WaveStrength;
                return uv + float2(waveX, waveY);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = Distort(IN.uv);
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

                // Simple Chromatic Aberration
                half4 colR = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + _Distortion * float2(0.01, 0.0));
                half4 colB = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - _Distortion * float2(0.01, 0.0));
                col.r = colR.r;
                col.b = colB.b;

                return col;
            }
            ENDHLSL
        }
    }
}
