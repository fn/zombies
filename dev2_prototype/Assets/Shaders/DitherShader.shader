// Shader to apply "Ordered Dithering" as a Post Procesing effect.

Shader "Unlit/DitherShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorCount("Color Count", Integer) = 16 // The amount of colors available in our palette.
        _Scale("Dither Scale", Range(0.0, 1.0)) = 0.15
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGINCLUDE
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

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
        ENDCG

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            int _ColorCount;
            float _Scale;

            // Taken from - https://en.wikipedia.org/wiki/Ordered_dithering
            static const int thresholdMap[4 * 4] = {
                0, 8, 2, 10,
                12, 4, 14, 6,
                3, 11, 1, 9,
                15, 7, 13, 5
            };

            float GetThresholdForPixel(float2 pixel) {
                return float(thresholdMap[(pixel.x % 4) + (pixel.y % 4) * 4]) * (0.0625f) - 0.5f;
            }

            // Dithering runs as apart of our fragment shader.
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample from our current texture.
                float4 col = tex2D(_MainTex, i.uv);

                // Get the pixel's screen coordiantes.
                float2 pixel = i.uv * _ScreenParams.xy;

                // Get our dithered output.
                fixed4 output = col + _Scale * GetThresholdForPixel(pixel);

                // Clamp the color to our palette.
                output.rgb = floor((_ColorCount - 1.0) * output.rgb + 0.5) / (_ColorCount - 1.0);

                return output;
            }
            ENDCG
        }
    }
}