// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33393,y:32587,varname:node_9361,prsc:2|custl-5602-OUT;n:type:ShaderForge.SFN_LightVector,id:9603,x:32111,y:32489,varname:node_9603,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:525,x:32111,y:32641,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:9072,x:32310,y:32489,varname:node_9072,prsc:2,dt:1|A-9603-OUT,B-525-OUT;n:type:ShaderForge.SFN_Add,id:6168,x:32487,y:32489,varname:node_6168,prsc:2|A-9072-OUT,B-35-OUT;n:type:ShaderForge.SFN_Multiply,id:3188,x:32671,y:32489,varname:node_3188,prsc:2|A-6168-OUT,B-35-OUT;n:type:ShaderForge.SFN_Vector1,id:35,x:32320,y:32727,varname:node_35,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Tex2d,id:9604,x:33101,y:32827,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_9604,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5602,x:33101,y:32603,varname:node_5602,prsc:2|A-739-OUT,B-9604-RGB,C-3394-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:3394,x:33073,y:33011,varname:node_3394,prsc:2;n:type:ShaderForge.SFN_HalfVector,id:9979,x:32108,y:33128,varname:node_9979,prsc:2;n:type:ShaderForge.SFN_Dot,id:3441,x:32301,y:33128,varname:node_3441,prsc:2,dt:1|A-525-OUT,B-9979-OUT;n:type:ShaderForge.SFN_Power,id:1278,x:32481,y:33128,varname:node_1278,prsc:2|VAL-3441-OUT,EXP-7338-OUT;n:type:ShaderForge.SFN_Slider,id:7338,x:32165,y:33303,ptovrint:False,ptlb:SpecRange,ptin:_SpecRange,varname:node_7338,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:12,max:20;n:type:ShaderForge.SFN_Multiply,id:2997,x:32744,y:33157,varname:node_2997,prsc:2|A-1278-OUT,B-3813-OUT;n:type:ShaderForge.SFN_Slider,id:3813,x:32336,y:33424,ptovrint:False,ptlb:SpecIntenity,ptin:_SpecIntenity,varname:_SpecRange_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:10;n:type:ShaderForge.SFN_Add,id:739,x:32855,y:32489,varname:node_739,prsc:2|A-3188-OUT,B-2997-OUT;proporder:9604-7338-3813;pass:END;sub:END;*/

Shader "Toon_Tank/Tank_test_2" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _SpecRange ("SpecRange", Range(0, 20)) = 12
        _SpecIntenity ("SpecIntenity", Range(0, 10)) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _SpecRange;
            uniform float _SpecIntenity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_35 = 0.5;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = ((((max(0,dot(lightDirection,i.normalDir))+node_35)*node_35)+(pow(max(0,dot(i.normalDir,halfDirection)),_SpecRange)*_SpecIntenity))*_MainTex_var.rgb*attenuation);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _SpecRange;
            uniform float _SpecIntenity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_35 = 0.5;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = ((((max(0,dot(lightDirection,i.normalDir))+node_35)*node_35)+(pow(max(0,dot(i.normalDir,halfDirection)),_SpecRange)*_SpecIntenity))*_MainTex_var.rgb*attenuation);
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
