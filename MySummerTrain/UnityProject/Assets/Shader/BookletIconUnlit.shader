Shader "Custom/BookletIconUnlit"
{
    Properties
    {
        _BgColor("Background Color", Color) = (0.2,0.2,0.2,1)
        _StripeColor("Stripe Color", Color) = (0.4,0.4,0.4,1)
        _StripeWidth("Stripe Width", float) = 0

        _Color0("Row 0 Color", Color) = (1,0,0,1)
        _Style0("Row 0 Style (0-3)", Range(0,3)) = 1

        _Color1("Row 1 Color", Color) = (0,1,0,1)
        _Style1("Row 1 Style (0-3)", Range(0,3)) = 1

        _Color2("Row 2 Color", Color) = (0,0,1,1)
        _Style2("Row 2 Style (0-3)", Range(0,3)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            // No lighting, just output emissive color
            ZWrite On
            Cull Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //–– Uniforms ––
            fixed4 _BgColor;
            fixed4 _StripeColor;
            float  _StripeWidth;

            fixed4 _Color0;
            int    _Style0;

            fixed4 _Color1;
            int    _Style1;

            fixed4 _Color2;
            int    _Style2;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv     : TEXCOORD0;
                float2 obj    : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = v.uv;
                o.obj    = v.vertex.xz;
                return o;
            }

            // Helper: pick color for one region stripe, given uv.x, region color, mode, and background color
            fixed4 GetRegionColor(float x, fixed4 regionColor, int mode, fixed4 bgColor)
            {
                // left third: x < 1/3
                // mid third: x ∈ [1/3, 2/3)
                // right third: x >= 2/3

                if (mode == 0) // L: left = BG, mid+right = regionColor
                {
                    if (x < (1.0 / 3.0)) 
                        return bgColor;
                    else 
                        return regionColor;
                }
                else if (mode == 1) // M: all = regionColor
                {
                    return regionColor;
                }
                else if (mode == 2) // R: left+mid = regionColor, right = BG
                {
                    if (x >= (2.0 / 3.0)) 
                        return bgColor;
                    else 
                        return regionColor;
                }
                else // mode == 3 (B): entire stripe = BG
                {
                    return bgColor;
                }
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 outCol = _StripeWidth ? (lerp(_BgColor, _StripeColor, step(0.5, frac((i.obj.x + i.obj.y) / _StripeWidth))))
                                             : _BgColor;

                // Determine which horizontal stripe (by uv.y)
                if (uv.y >= 0.25 && uv.y < 0.50)
                {
                    // Region A stripe
                    outCol = GetRegionColor(uv.x, _Color0, _Style0, _BgColor);
                }
                else if (uv.y >= 0.50 && uv.y < 0.75)
                {
                    // Region B stripe
                    outCol = GetRegionColor(uv.x, _Color1, _Style1, _BgColor);
                }
                else if (uv.y >= 0.75 && uv.y <= 1.0)
                {
                    // Region C stripe
                    outCol = GetRegionColor(uv.x, _Color2, _Style2, _BgColor);
                }
                // else uv.y < 0.25 → stays background

                outCol.rgb = LinearToGammaSpace(outCol.rgb);
                return outCol;
            }
            ENDCG
        }
    }
}
