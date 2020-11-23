Shader "Custom/009InterpolatingShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _SecondaryColor("Secondary Color", Color) = (1,1,1,1)
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

            };
            struct v2f
            {
                float4 position : SV_POSITION;
            };

            v2f vert(appdata v) 
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(v2f i) : SV_TARGET
            {
                fixed4 col = _Color + _SecondaryColor * _BlendIntensity;
                return col;
            }

            ENDCG
        }

    }
    FallBack "Diffuse"
}
