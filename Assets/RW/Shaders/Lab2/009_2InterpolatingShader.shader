Shader "Custom/009_2InterpolatingShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "White" {}
        _SecondaryTex("Secondary Texture", 2D) = "White" {}
        _BlendIntensity("Blend Intensity", Range(0,1)) = 0
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}

        Pass
        {
            CGPROGRAM
            //include useful shader functions
            #include "UnityCG.cginc"

            //define vertex and fragment shader
            #pragma vertex vert
            #pragma fragment frag

            //the value that's used to blend between the colors
            float _BlendIntensity;
            fixed4 _Color;
            fixed4 _SecondaryColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

            };
            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            fixed4 frag(v2f i) : SV_TARGET
            {

                float2 main_uv = TRANSFORM_TEX(i.uv, _MainTex);
                float2 secondary_uv = TRANSFORM_TEX(i.uv, _SecondaryTex);

                fixed4 main_color = tex2D(_MainTex, main_uv);
                fixed4 secondary_color = tex2D(_SecondaryTex, secondary_uv);

                //fixed4 col = _Color + _SecondaryColor * _BlendIntensity;
                fixed4 col = lerp(main_color, secondary_color, _BlendIntensity);

                return col;
            }

            ENDCG
        }

    }
        FallBack "Diffuse"
}
