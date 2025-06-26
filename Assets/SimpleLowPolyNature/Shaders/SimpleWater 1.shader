// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LowPoly/URP_SimpleWater"
{
    Properties
    {
       _WaterNormal("Water Normal", 2D) = "bump" {}
       _NormalScale("Normal Scale", Float) = 1.0
       _DeepColor("Deep Color", Color) = (0.1, 0.2, 0.9, 1)
       _ShallowColor("Shallow Color", Color) = (0.3, 0.7, 1, 1)
       _WaterDepth("Water Depth", Range(0, 10)) = 1
       _WaterFalloff("Water Falloff", Range(0, 10)) = 2
       _WaterSpecular("Water Specular", Range(0, 1)) = 0.5
       _WaterSmoothness("Water Smoothness", Range(0, 1)) = 0.8
       _Distortion("Distortion", Range(0, 1)) = 0.1
       _Foam("Foam", 2D) = "white" {}
       _FoamDepth("Foam Depth", Range(0, 2)) = 0.5
       _FoamFalloff("Foam Falloff", Range(0, 10)) = 5
       _FoamSpecular("Foam Specular", Range(0, 1)) = 0.1
       _FoamSmoothness("Foam Smoothness", Range(0, 1)) = 0.2
       _WavesAmplitude("Waves Amplitude", Float) = 0.01
       _WavesAmount("Waves Amount", Float) = 8.87
    }

    SubShader
    {
        Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            
            // This Pass is for transparent objects
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // URP Core Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            // Struct for vertex shader input
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 texcoord     : TEXCOORD0;
            };

            // Struct for data passed from vertex to fragment shader
            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float4 screenPos    : TEXCOORD0;
                float2 uv           : TEXCOORD1;
                float3 positionWS   : TEXCOORD2;
                float3 normalWS     : TEXCOORD3;
            };

            // Declare Textures and Samplers for URP
            TEXTURE2D(_WaterNormal);        SAMPLER(sampler_WaterNormal);
            TEXTURE2D(_Foam);               SAMPLER(sampler_Foam);
            
            // To get screen color and depth, URP uses these textures
            // Make sure to enable "Depth Texture" and "Opaque Texture" in your URP Asset
            TEXTURE2D(_CameraOpaqueTexture); SAMPLER(sampler_CameraOpaqueTexture);
            TEXTURE2D(_CameraDepthTexture);  SAMPLER(sampler_CameraDepthTexture);

            // CBUFFER for shader properties
            CBUFFER_START(UnityPerMaterial)
                float4 _WaterNormal_ST;
                float4 _Foam_ST;
                half _NormalScale;
                half4 _DeepColor;
                half4 _ShallowColor;
                half _WaterDepth;
                half _WaterFalloff;
                half _WaterSpecular;
                half _WaterSmoothness;
                half _FoamDepth;
                half _FoamFalloff;
                half _FoamSpecular;
                half _FoamSmoothness;
                half _Distortion;
                half _WavesAmplitude;
                half _WavesAmount;
            CBUFFER_END
            
            // Custom function to blend normal maps, similar to BlendNormals
            half3 BlendNormalMaps(half3 n1, half3 n2)
            {
                return normalize(half3(n1.xy + n2.xy, n1.z * n2.z));
            }

            // Vertex Shader
            Varyings vert(Attributes IN)
            {
                Varyings OUT = (Varyings)0;
                
                // Vertex wave animation
                float3 pos = IN.positionOS.xyz;
                pos += (sin(((_WavesAmount * pos.z) + _Time.y)) * IN.normalOS) * _WavesAmplitude;
                
                OUT.positionWS = TransformObjectToWorld(pos);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.texcoord;

                // Calculate screen position for sampling opaque and depth textures
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);

                return OUT;
            }

            // Fragment Shader
            half4 frag(Varyings IN) : SV_Target
            {
                // -- Normal Mapping --
                float2 uv_WaterNormal = IN.uv * _WaterNormal_ST.xy + _WaterNormal_ST.zw;
                float2 panner1 = (uv_WaterNormal + _Time.y * float2(-0.03, 0.0));
                float2 panner2 = (uv_WaterNormal + _Time.y * float2(0.04, 0.04));

                half3 normalMap1 = UnpackNormalScale(SAMPLE_TEXTURE2D(_WaterNormal, sampler_WaterNormal, panner1), _NormalScale);
                half3 normalMap2 = UnpackNormalScale(SAMPLE_TEXTURE2D(_WaterNormal, sampler_WaterNormal, panner2), _NormalScale);
                half3 blendedNormalTS = BlendNormalMaps(normalMap1, normalMap2);
                
                // Transform tangent space normal to world space
                float3 normalWS = TransformTangentToWorld(blendedNormalTS, CreateTangentToWorld(IN.normalWS, float4(1,0,0,1), IN.normalWS));

                // -- Depth Calculation --
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
                
                // Get depth of the geometry behind the water
                float sceneRawDepth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, screenUV).r;
                float sceneEyeDepth = LinearEyeDepth(sceneRawDepth, _ZBufferParams);
                
                // Get depth of the water surface
                float surfaceEyeDepth = IN.screenPos.w;
                
                // Difference in depth
                float depthDifference = sceneEyeDepth - surfaceEyeDepth;

                // -- Color based on Depth --
                half depthFactor = saturate(pow(depthDifference / _WaterDepth, _WaterFalloff));
                half4 waterColor = lerp(_DeepColor, _ShallowColor, depthFactor);

                // -- Foam --
                float2 uv_Foam = IN.uv * _Foam_ST.xy + _Foam_ST.zw;
                float2 foamPanner = (uv_Foam + _Time.y * float2(-0.01, 0.01));
                half foamTex = SAMPLE_TEXTURE2D(_Foam, sampler_Foam, foamPanner).r;

                // Calculate foam amount based on depth
                half foamDepthFactor = 1.0 - saturate(depthDifference / _FoamDepth);
                foamDepthFactor = pow(foamDepthFactor, _FoamFalloff);
                half foamAmount = foamDepthFactor * foamTex;

                // Mix water color with foam (foam is white)
                waterColor = lerp(waterColor, half4(1,1,1,1), foamAmount);
                
                // -- Refraction (Distortion) --
                // Use the blended normal to distort the screen UVs
                float2 distortionUV = screenUV + (normalWS.xy * _Distortion);
                half4 sceneColor = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, distortionUV);
                
                // Blend the water color with the refracted scene color based on depth
                // In this setup, deeper water is more opaque (less refraction)
                half3 finalColor = lerp(sceneColor.rgb, waterColor.rgb, waterColor.a);
                
                // -- Lighting --
                // Prepare data for URP PBR lighting
                half3 viewDirectionWS = SafeNormalize(GetCameraPositionWS() - IN.positionWS);
                Light mainLight = GetMainLight();
                half3 lightDirectionWS = mainLight.direction;
                half3 lightColor = mainLight.color;
                half atten = mainLight.shadowAttenuation * mainLight.distanceAttenuation;
                
                // Lerp smoothness and specular based on foam amount
                half smoothness = lerp(_WaterSmoothness, _FoamSmoothness, foamAmount);
                half3 specularColor = lerp(half3(_WaterSpecular, _WaterSpecular, _WaterSpecular), half3(_FoamSpecular, _FoamSpecular, _FoamSpecular), foamAmount);
                
                // URP PBR lighting calculation (Simplified Blinn-Phong for specular)
                half3 halfVec = SafeNormalize(lightDirectionWS + viewDirectionWS);
                half NdotH = saturate(dot(normalWS, halfVec));
                half spec = pow(NdotH, smoothness * 100.0) * _WaterSpecular;
                half3 specularReflection = spec * specularColor * lightColor;

                half NdotL = saturate(dot(normalWS, lightDirectionWS));
                half3 diffuse = finalColor * NdotL * lightColor * atten;

                // Combine diffuse, specular, and add simple ambient light
                half3 finalLitColor = diffuse + specularReflection + (finalColor * GetMainLight().color * 0.1);

                return half4(finalLitColor, waterColor.a);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
    CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
}
