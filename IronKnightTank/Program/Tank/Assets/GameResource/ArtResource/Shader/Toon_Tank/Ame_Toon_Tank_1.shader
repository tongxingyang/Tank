// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|custl-8456-OUT;n:type:ShaderForge.SFN_LightVector,id:7459,x:31921,y:32733,varname:node_7459,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:3043,x:31921,y:32882,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:6364,x:32101,y:32733,varname:node_6364,prsc:2,dt:0|A-7459-OUT,B-3043-OUT;n:type:ShaderForge.SFN_Add,id:9434,x:32291,y:32733,varname:node_9434,prsc:2|A-6364-OUT,B-1426-OUT;n:type:ShaderForge.SFN_Vector1,id:1426,x:32101,y:32908,varname:node_1426,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Tex2d,id:7237,x:32822,y:32911,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_7237,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:685,x:32822,y:32735,varname:node_685,prsc:2|A-9434-OUT,B-7237-RGB;n:type:ShaderForge.SFN_Step,id:1944,x:32471,y:32584,varname:node_1944,prsc:2|A-5623-OUT,B-9434-OUT;n:type:ShaderForge.SFN_Vector1,id:5623,x:32291,y:32584,varname:node_5623,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Step,id:7641,x:32477,y:32913,varname:node_7641,prsc:2|A-5666-OUT,B-9434-OUT;n:type:ShaderForge.SFN_Vector1,id:5666,x:32291,y:32872,varname:node_5666,prsc:2,v1:0.4;n:type:ShaderForge.SFN_Step,id:803,x:32657,y:32584,varname:node_803,prsc:2|A-7641-OUT,B-1944-OUT;n:type:ShaderForge.SFN_OneMinus,id:9987,x:32822,y:32584,varname:node_9987,prsc:2|IN-803-OUT;n:type:ShaderForge.SFN_TexCoord,id:9917,x:31918,y:33163,varname:node_9917,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Frac,id:8787,x:32293,y:33163,varname:node_8787,prsc:2|IN-1469-OUT;n:type:ShaderForge.SFN_Multiply,id:1469,x:32105,y:33163,varname:node_1469,prsc:2|A-3543-OUT,B-9917-V;n:type:ShaderForge.SFN_Vector1,id:3543,x:31918,y:33072,varname:node_3543,prsc:2,v1:160;n:type:ShaderForge.SFN_Multiply,id:8103,x:32822,y:33119,varname:node_8103,prsc:2|A-7237-RGB,B-7261-OUT;n:type:ShaderForge.SFN_Lerp,id:8456,x:33041,y:33100,varname:node_8456,prsc:2|A-685-OUT,B-8103-OUT,T-9987-OUT;n:type:ShaderForge.SFN_Step,id:7261,x:32477,y:33163,varname:node_7261,prsc:2|A-7124-OUT,B-8787-OUT;n:type:ShaderForge.SFN_Vector1,id:7124,x:32293,y:33051,varname:node_7124,prsc:2,v1:0.5;proporder:7237;pass:END;sub:END;*/

Shader "Toon_Tank/Ame_Toon_Tank_1" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
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
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float node_9434 = (dot(lightDirection,i.normalDir)+0.5);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = lerp((node_9434*_MainTex_var.rgb),(_MainTex_var.rgb*step(0.5,frac((160.0*i.uv0.g)))),(1.0 - step(step(0.4,node_9434),step(0.5,node_9434))));
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
            #pragma multi_compile_fwdadd_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
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
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float node_9434 = (dot(lightDirection,i.normalDir)+0.5);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = lerp((node_9434*_MainTex_var.rgb),(_MainTex_var.rgb*step(0.5,frac((160.0*i.uv0.g)))),(1.0 - step(step(0.4,node_9434),step(0.5,node_9434))));
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
