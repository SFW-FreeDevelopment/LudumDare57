Shader "Unlit/MaskedDarkness"
{
    Properties
    {
        _LightMaskTex ("Light Mask", 2D) = "white" {}
        _DarkColor ("Darkness Color", Color) = (0, 0, 0, 0.85)
        _AspectRatio ("Screen Aspect Ratio", Float) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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

            sampler2D _LightMaskTex;
            float4 _DarkColor;
            float _AspectRatio;
            float4 _LightMaskTex_TexelSize; // Unity auto-fills this for tex2D

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
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

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Compute texture aspect correction
                float texAspect = _LightMaskTex_TexelSize.z / _LightMaskTex_TexelSize.w; // RT width / height
                float screenAspect = _AspectRatio; // From C#
                float aspectRatioCorrection = screenAspect / texAspect;

                // Remap to full screen width by scaling and shifting the center square
                // This transforms UVs from the central 1:1 area of the RT to stretch across screen width
                uv.x = (uv.x - 0.5f) * aspectRatioCorrection + 0.5f;

                // Clamp to avoid garbage from overshooting edges
                uv = clamp(uv, 0.001, 0.999);

                float4 sample = tex2D(_LightMaskTex, uv);
                float light = sample.r * sample.a;

                float darknessAlpha = _DarkColor.a * (1.0 - light);
                return fixed4(_DarkColor.rgb, darknessAlpha);
            }
            ENDCG
        }
    }
}
