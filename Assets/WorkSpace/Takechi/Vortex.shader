Shader "Unlit/Vortex"
{
    Properties
    {
        _Vortex_Strength("Vortex_Strength", Float) = 8
        _Vortex_Speed("Vortex_Speed", Float) = 1
        _Noise_Scale("Noise_Scale", Float) = 20
        _Color_1("Color_1", Color) = (0, 0, 0, 0)
        _Color_2("Color_2", Color) = (0, 0, 0, 0)
        _Color_3("Color_3", Color) = (0, 0, 0, 0)
        _Progress("Progress", Range(0, 1)) = 0
        [HideInInspector]_BUILTIN_QueueOffset("Float", Float) = 0
        [HideInInspector]_BUILTIN_QueueControl("Float", Float) = -1
    }
    SubShader
    {
        Tags
        {
            // RenderPipeline: <None>
            "RenderType"="Transparent"
            "BuiltInMaterialType" = "Unlit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="BuiltInUnlitSubTarget"
        }
        Pass
        {
            Name "Pass"
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        ColorMask RGB
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile_fwdbase
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define BUILTIN_TARGET_API 1
        #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Vortex_Strength;
        float _Vortex_Speed;
        float _Noise_Scale;
        float4 _Color_1;
        float4 _Color_2;
        float4 _Color_3;
        float _Progress;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Twirl_float(float2 UV, float2 Center, float Strength, float2 Offset, out float2 Out)
        {
            float2 delta = UV - Center;
            float angle = Strength * length(delta);
            float x = cos(angle) * delta.x - sin(angle) * delta.y;
            float y = sin(angle) * delta.x + cos(angle) * delta.y;
            Out = float2(x + Center.x + Offset.x, y + Center.y + Offset.y);
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A / B;
        }
        
        void Unity_Blend_Overwrite_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
        {
            Out = lerp(Base, Blend, Opacity);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_6ddb0276f1584d4585f58fce2b1067e7_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_6ddb0276f1584d4585f58fce2b1067e7_Out_0_Texture2D.tex, _Property_6ddb0276f1584d4585f58fce2b1067e7_Out_0_Texture2D.samplerstate, _Property_6ddb0276f1584d4585f58fce2b1067e7_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_R_4_Float = _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_RGBA_0_Vector4.r;
            float _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_G_5_Float = _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_RGBA_0_Vector4.g;
            float _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_B_6_Float = _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_RGBA_0_Vector4.b;
            float _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_A_7_Float = _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_RGBA_0_Vector4.a;
            float4 _Property_de06dca325584423b6d46217386acb7e_Out_0_Vector4 = _Color_1;
            float4 _Property_8a4dc5e675c84a1e843c0261f274c4ea_Out_0_Vector4 = _Color_2;
            float _Float_ac58f0e587834a6c80565ca3f3bb51e0_Out_0_Float = 0.5;
            float _Property_4ed2383bfb1a4938b60dca89f33006d1_Out_0_Float = _Vortex_Strength;
            float _Property_73d8620f6ddf46bfb1992429abf78dfc_Out_0_Float = _Vortex_Speed;
            float _Multiply_6650913b8e294eee9928b01edcd63363_Out_2_Float;
            Unity_Multiply_float_float(_Property_73d8620f6ddf46bfb1992429abf78dfc_Out_0_Float, IN.TimeParameters.x, _Multiply_6650913b8e294eee9928b01edcd63363_Out_2_Float);
            float2 _Twirl_df3f96ff07b74abaa0e6543310fecef5_Out_4_Vector2;
            Unity_Twirl_float(IN.uv0.xy, float2 (0.5, 0.5), _Property_4ed2383bfb1a4938b60dca89f33006d1_Out_0_Float, (_Multiply_6650913b8e294eee9928b01edcd63363_Out_2_Float.xx), _Twirl_df3f96ff07b74abaa0e6543310fecef5_Out_4_Vector2);
            float _Property_ca47b688939b40339b4cb7b923fe62c3_Out_0_Float = _Noise_Scale;
            float _SimpleNoise_bd60d799c57e4dfd9c5a7022f3f06d12_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_Twirl_df3f96ff07b74abaa0e6543310fecef5_Out_4_Vector2, _Property_ca47b688939b40339b4cb7b923fe62c3_Out_0_Float, _SimpleNoise_bd60d799c57e4dfd9c5a7022f3f06d12_Out_2_Float);
            float _Smoothstep_51cda3dfeb0547a3ae6ea76e3da2a697_Out_3_Float;
            Unity_Smoothstep_float(0, _Float_ac58f0e587834a6c80565ca3f3bb51e0_Out_0_Float, _SimpleNoise_bd60d799c57e4dfd9c5a7022f3f06d12_Out_2_Float, _Smoothstep_51cda3dfeb0547a3ae6ea76e3da2a697_Out_3_Float);
            float4 _Lerp_5fd1a4d540df4849a9237189dd92923a_Out_3_Vector4;
            Unity_Lerp_float4(_Property_de06dca325584423b6d46217386acb7e_Out_0_Vector4, _Property_8a4dc5e675c84a1e843c0261f274c4ea_Out_0_Vector4, (_Smoothstep_51cda3dfeb0547a3ae6ea76e3da2a697_Out_3_Float.xxxx), _Lerp_5fd1a4d540df4849a9237189dd92923a_Out_3_Vector4);
            float4 _Property_40943f1ff3b64e948b23d15aa0fd3c93_Out_0_Vector4 = _Color_3;
            float _Smoothstep_7ea012e1880146ee9fa0695fcf6323cb_Out_3_Float;
            Unity_Smoothstep_float(_Float_ac58f0e587834a6c80565ca3f3bb51e0_Out_0_Float, 1, _SimpleNoise_bd60d799c57e4dfd9c5a7022f3f06d12_Out_2_Float, _Smoothstep_7ea012e1880146ee9fa0695fcf6323cb_Out_3_Float);
            float4 _Lerp_e5a651b7001c4810aa2ec455e682c07d_Out_3_Vector4;
            Unity_Lerp_float4(_Lerp_5fd1a4d540df4849a9237189dd92923a_Out_3_Vector4, _Property_40943f1ff3b64e948b23d15aa0fd3c93_Out_0_Vector4, (_Smoothstep_7ea012e1880146ee9fa0695fcf6323cb_Out_3_Float.xxxx), _Lerp_e5a651b7001c4810aa2ec455e682c07d_Out_3_Vector4);
            float4 _Property_8e9fad854fe149c8bc1fa540a7e7bbaf_Out_0_Vector4 = _Color_1;
            float4 _Property_909e4e430f37428187ef5fa553460dd9_Out_0_Vector4 = _Color_2;
            float4 _Add_6a16405a2f09402da3b1f9cd80f201bd_Out_2_Vector4;
            Unity_Add_float4(_Property_8e9fad854fe149c8bc1fa540a7e7bbaf_Out_0_Vector4, _Property_909e4e430f37428187ef5fa553460dd9_Out_0_Vector4, _Add_6a16405a2f09402da3b1f9cd80f201bd_Out_2_Vector4);
            float4 _Property_5e5ca72b9c014a54972acfb350504524_Out_0_Vector4 = _Color_3;
            float4 _Add_c06a2bc740734b94872ef794afd4da31_Out_2_Vector4;
            Unity_Add_float4(_Add_6a16405a2f09402da3b1f9cd80f201bd_Out_2_Vector4, _Property_5e5ca72b9c014a54972acfb350504524_Out_0_Vector4, _Add_c06a2bc740734b94872ef794afd4da31_Out_2_Vector4);
            float4 _Divide_b10452e66deb464e881e10e39c48ca7d_Out_2_Vector4;
            Unity_Divide_float4(_Add_c06a2bc740734b94872ef794afd4da31_Out_2_Vector4, float4(3, 3, 3, 3), _Divide_b10452e66deb464e881e10e39c48ca7d_Out_2_Vector4);
            float _Property_0ae8836da4a14eb1a6f5a933bf032cd7_Out_0_Float = _Progress;
            float4 _Blend_1715262ccafa4fbca0f3693e0cdabf45_Out_2_Vector4;
            Unity_Blend_Overwrite_float4(_Lerp_e5a651b7001c4810aa2ec455e682c07d_Out_3_Vector4, _Divide_b10452e66deb464e881e10e39c48ca7d_Out_2_Vector4, _Blend_1715262ccafa4fbca0f3693e0cdabf45_Out_2_Vector4, _Property_0ae8836da4a14eb1a6f5a933bf032cd7_Out_0_Float);
            float4 _Multiply_e98b918281744f7d8a32fb1e344df90f_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_RGBA_0_Vector4, _Blend_1715262ccafa4fbca0f3693e0cdabf45_Out_2_Vector4, _Multiply_e98b918281744f7d8a32fb1e344df90f_Out_2_Vector4);
            surface.BaseColor = (_Multiply_e98b918281744f7d8a32fb1e344df90f_Out_2_Vector4.xyz);
            surface.Alpha = _SampleTexture2D_feee7eb98e934b6f9b43efdc27b22209_A_7_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if UNITY_SHOULD_SAMPLE_SH
            #if !defined(LIGHTMAP_ON)
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if UNITY_SHOULD_SAMPLE_SH
            #if !defined(LIGHTMAP_ON)
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.Rendering.BuiltIn.ShaderGraph.BuiltInUnlitGUI" ""
    FallBack "Hidden/Shader Graph/FallbackError"
}
