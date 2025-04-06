Shader "Custom/UILightMaskFixed"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,0.8)
        _HolePosition ("Hole Position", Vector) = (0.5, 0.5, 0, 0)
        _HoleRadius ("Hole Radius", Float) = 0.2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 screenUV : TEXCOORD0;
            };

            fixed4 _Color;
            float2 _HolePosition;
            float _HoleRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenUV = o.vertex.xy / o.vertex.w;
                o.screenUV = 0.5f * (float2(o.screenUV.x + 1.0, o.screenUV.y + 1.0));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.screenUV;

                // Get screen aspect correction
                float aspect = _ScreenParams.x / _ScreenParams.y;

                // Offset from hole center
                float2 delta = uv - _HolePosition;

                // Correct X distance by aspect
                delta.x *= aspect;

                float dist = length(delta);

                if (dist < _HoleRadius)
                    return fixed4(0, 0, 0, 0); // fully transparent in the hole
                else
                    return _Color; // black overlay outside
            }
            ENDCG
        }
    }
}