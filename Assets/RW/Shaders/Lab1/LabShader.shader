// input 2 textures

Shader "Custom/LabShader"
{
    Properties
    {
        // _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _SubTex("Texture", 2D) = "white" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Pass
        {
            SetTexture[_MainTex]
            {
                Combine texture DOUBLE

            }
            SetTexture[_SubTex]
            {
                Combine texture * previous
            }

        }
    }
    FallBack "Diffuse"
}
