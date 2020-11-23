Shader "Custom/BendShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _BendAngle("Bend Angle",Float) = 0
            // _BendTopPoint("Bend Top Point", Int) = 0
            // _BendBottomPoint("Bend Bottom Point", Int) = 0
            _PinchingPoint("Pinching Point", Int) = 0 // 0,1,2, 3,4,5
            _Length("Buffer Length", Int) = 2
        
        _TargetVector("Target Vector", Vector) = (0,0,0)
        _TargetVectorRelative("Target Vector Relative", Vector) = (0,0,0)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        // ZWrite Off
        // Blend SrcAlpha OneMinusSrcAlpha
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma target 4.5

            #pragma enable_d3d11_debug_symbols

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _BendAngle;
            int _PinchingPoint;
            float3 _TargetVector;

            float top;
            float bottom;
            
            float4x4 axisToMesh;
            float4x4 meshToAxis;

            RWStructuredBuffer<float> buffer : register(u1);
            // RWStructuredBuffer<float3> buffer : register(u1);
            // RWStructuredBuffer<float4> buffer : register(u1);


            float PrintValue(float2 vCoords, float fValue, float fMaxDigits, float fDecimalPlaces)
            {
                if ((vCoords.y < 0.0) || (vCoords.y >= 1.0)) return 0.0;
                bool bNeg = (fValue < 0.0);
                fValue = abs(fValue);
                float fBiggestIndex = max(floor(log2(abs(fValue)) / log2(10.0)), 0.0);
                float fDigitIndex = fMaxDigits - floor(vCoords.x);
                float fCharBin = 0.0;
                if (fDigitIndex > (-fDecimalPlaces - 1.01))
                {
                    if (fDigitIndex > fBiggestIndex)
                    {
                        if ((bNeg) && (fDigitIndex < (fBiggestIndex + 1.5))) fCharBin = 1792.0;
                    }
                    else
                    {
                        if (fDigitIndex == -1.0)
                        {
                            if (fDecimalPlaces > 0.0) fCharBin = 2.0;
                        }
                        else
                        {
                            float fReducedRangeValue = fValue;
                            if (fDigitIndex < 0.0) { fReducedRangeValue = frac(fValue); fDigitIndex += 1.0; }
                            float fDigitValue = (abs(fReducedRangeValue / (pow(10.0, fDigitIndex))));
                            int x = int(floor(fDigitValue - 10.0 * floor(fDigitValue / 10.0)));
                            fCharBin = 
                                x == 0 ? 480599.0 : x == 1 ? 139810.0 : 
                                x == 2 ? 476951.0 : x == 3 ? 476999.0 : 
                                x == 4 ? 350020.0 : x == 5 ? 464711.0 : x == 6 ? 464727.0 : x == 7 ? 476228.0 : x == 8 ? 481111.0 : x == 9 ? 481095.0 : 0.0;
                        }
                    }
                }
                float result = (fCharBin / pow(2.0, floor(frac(vCoords.x) * 4.0) + (floor(vCoords.y * 5.0) * 4.0)));
                return floor(result - 2.0 * floor(result / 2.0));
            }

            float4x4 GetMeshToAxisSpace(float4x4 axis, float4x4 dataTarget)
            {
                return mul(axis, dataTarget);
            }


            v2f vert (appdata v)
            {
                const float PI = 3.14159;

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                // buffer[0] = o.worldPos;

                // TODO: adjust value of testPoint and pinchingPoint

                /*if (v.vertex.y <= 0)
                {
                    top = o.uv.y;
                }
                if (v.vertex.y <= 1)
                {
                    bottom = o.uv.y;
                }*/

                if (o.uv.x >= 0.7 && o.uv.y >= 0.7)
                {
                    float4 testPoint = (0.7, 0.6, 0.4, 1.0);
                    float4 pinchingPoint = mul(testPoint, v.vertex);
                    
                    // buffer[0] = pinchingPoint;

                    float angleRadians = radians(float(_BendAngle) );
                    float scale = 1 / angleRadians;

                    
                    float rotation = pinchingPoint.y * angleRadians;
                    // buffer[0] = rotation;

                    buffer[0] = PI - rotation;
                    float c = cos(PI - rotation);
                    // buffer[0] = c;
                    float s = sin(PI - rotation);
                    testPoint.xy = float2
                        (
                            (scale * c) + scale - (testPoint.x * c),
                            (scale * s) - (testPoint.x * s)
                            );

                    o.vertex = mul(testPoint, pinchingPoint);
                }
                


                // buffer[1] = o.worldPos;

                // float3 baseWorldPos = unity_ObjectToWorld._m03_m13_m23;
                // buffer[0] = baseWorldPos;

                if (o.uv.x <= 0.9
                    && o.uv.y <= 0.9)
                    // && o.uv.x <= 0.96
                    // && o.uv.y <= 0.96)
                {
                    // buffer[0] = UnityObjectToClipPos(o.vertex);

                    // buffer[0] = o.worldPos;
                    
                    // buffer2[0] = o.worldPos;

                    // buffer[0] = UnityObjectToViewPos(o.vertex);
                }
                
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                // _PinchingPoint = 4;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                
                const float PI = 3.14159;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                //float4 p = float4(1.0, 0.2, 0.321, 0.789);
                //buffer[0] = p;

                float dx = _MainTex_ST.x * 0.5;
                float4 maximum = tex2D(_MainTex, i.uv + float2(-dx, -dx));
                maximum = max(maximum, tex2D(_MainTex, i.uv + float2(dx, -dx)));
                maximum = max(maximum, tex2D(_MainTex, i.uv + float2(-dx, dx)));
                maximum = max(maximum, tex2D(_MainTex, i.uv + float2(dx, dx)));

                return tex2D(_MainTex, i.uv);
                
                // buffer[0] = maximum;
                
                //float number = 123.0;
                // float value = i.number;

                if (i.uv.x > 0.85 && i.uv.y > 0.5)
                {

                    // float2 font = float2 (36.0, 45.0);
                    // float2 position = float2(0.72, 0.9);
                    // float3 base = PrintValue((i.uv - position) * font, number, 6.0, 2.0).xxx;
                    
                    


                    // float2 font = float2 (24.0, 30.0);
                    // float2 position = float2(_ScreenParams.x - 250.0, 15.0);
                    // float3 base = PrintValue((i.vertex.xy - position) / font, 123.0, 6.0, 2.0).xxx;
                    // float4 tempBase = float4(base, 1.0);
                    
                    // buffer[0] = WorldSpaceViewDir(tempBase);
                    // buffer[0] = ObjSpaceViewDir(tempBase);

                    // tempBase.r = 0.5;
                    // return tempBase;

                }
                else return tex2D(_MainTex, i.uv);

                // return col;
            }
            
            
            void TestCalculateWorldPos(inout appdata_full v)
            {
                // unity_WorldToObject
                float3 n = normalize(mul(unity_ObjectToWorld, v.normal).xyz);
                float3 worldSpace = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                // v.texcoord.xy = float2(dot(worldSpace, uDirection), dot(worldSpace, vDirection));

            }
            ENDCG
        }
    }
}
