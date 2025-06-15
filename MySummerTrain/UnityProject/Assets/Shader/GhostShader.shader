Shader "Custom/GhostShader"
{
    Properties
    {
        _Color("Ghost Color", Color) = (0.4,0.6,1,0.2)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Blend One OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color;

            float4 vert(float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }

            float4 frag() : SV_Target
            {
                return float4(LinearToGammaSpace(_Color.rgb), _Color.a);
            }
            ENDCG
        }
    }
}
