Shader "CustomEffects/GaussianBlurURP"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    #define E 2.71828f
    #define TWO_PI 6.28318f

    float _Spread;
    int _GridSize;

    float gaussian(int x)
    {
        float sigmaSqu = _Spread * _Spread;
        return (1 / sqrt(TWO_PI * sigmaSqu)) * pow(E, -(x * x) / (2 * sigmaSqu));
    }

    float4 GaussianBlurHorizontal(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        float3 color = 0;
        float gridSum = 0;

        int halfGrid = (_GridSize - 1) / 2;

        for (int x = -halfGrid; x <= halfGrid; ++x)
        {
            float gauss = gaussian(x);
            gridSum += gauss;

            float2 offset = float2(_BlitTexture_TexelSize.x * x, 0.0);
            color += gauss * SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord + offset).rgb;
        }

        return float4(color / gridSum, 1.0);
    }

    float4 GaussianBlurVertical(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        float3 color = 0;
        float gridSum = 0;

        int halfGrid = (_GridSize - 1) / 2;

        for (int y = -halfGrid; y <= halfGrid; ++y)
        {
            float gauss = gaussian(y);
            gridSum += gauss;

            float2 offset = float2(0.0, _BlitTexture_TexelSize.y * y);
            color += gauss * SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord + offset).rgb;
        } 

        return float4(color / gridSum, 1.0);
    }

    ENDHLSL

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off Cull Off ZTest Always

        Pass
        {
            Name "GaussianBlurHorizontal"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment GaussianBlurHorizontal
            ENDHLSL
        }

        Pass
        {
            Name "GaussianBlurVertical"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment GaussianBlurVertical
            ENDHLSL
        }
    }
}