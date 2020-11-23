// Blend buffer and current texture

Shader "Custom/BlendLabShader"
{
    Properties
    {
        // _Color ("Color", Color) = (1,1,1,1)
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _SubTex("Texture", 2D) = "white" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
    }
        SubShader
    {
        Tags {"Queue" = "Transparent"}
        // Background 1000
        // Geometry 2000
        // AlphaTest 2450
        // Transparent 3000
        // Overlay 4000
        // Tags {"Queue" = "Geometry + 1"}

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            SetTexture[_MainTex]
            {
                
                Combine texture DOUBLE

            }
            SetTexture[_SubTex]
            {
                ConstantColor[_Color]
                Combine texture lerp(texture) previous, constant

            }

        }
    }
        FallBack "Diffuse"
}
