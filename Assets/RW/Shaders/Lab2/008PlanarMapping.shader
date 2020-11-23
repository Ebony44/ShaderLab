Shader "Custom/008PlanarMapping"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
        
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        
        Pass
        {
            CGPROGRAM
            
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                
                //o.position = UnityObjectToClipPos(v.vertex);
                // o.uv = v.vertex.xz;
                //o.uv = TRANSFORM_TEXT(v.vertex.xz, _MainTex);


                //calculate the position in clip space to render the object
                o.position = UnityObjectToClipPos(v.vertex);
                //calculate world position of vertex
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                //change UVs based on tiling and offset of texture
                o.uv = TRANSFORM_TEX(worldPos.xz, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                //read texture at uv position
                fixed4 col = tex2D(_MainTex, i.uv);
                //multiply texture color with tint color
                col *= _Color;
                return col;
            }
            ENDCG



        }

        
    }
    FallBack "Standard"
}
