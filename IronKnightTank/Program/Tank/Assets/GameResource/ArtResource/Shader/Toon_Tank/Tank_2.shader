// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|custl-312-OUT,olwid-4321-OUT,olcol-5035-RGB;n:type:ShaderForge.SFN_Tex2d,id:2453,x:32213,y:32757,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_2453,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_LightVector,id:3945,x:31347,y:32491,varname:node_3945,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:1866,x:31347,y:32622,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:7013,x:31552,y:32491,varname:node_7013,prsc:2,dt:1|A-3945-OUT,B-1866-OUT;n:type:ShaderForge.SFN_Posterize,id:1293,x:32287,y:32464,varname:node_1293,prsc:2|IN-42-OUT,STPS-217-OUT;n:type:ShaderForge.SFN_Vector1,id:217,x:32086,y:32630,varname:node_217,prsc:2,v1:3;n:type:ShaderForge.SFN_Vector1,id:2144,x:32363,y:32695,varname:node_2144,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Add,id:2615,x:32572,y:32443,varname:node_2615,prsc:2|A-1293-OUT,B-2144-OUT;n:type:ShaderForge.SFN_Multiply,id:1624,x:32724,y:32641,varname:node_1624,prsc:2|A-2615-OUT,B-2453-RGB;n:type:ShaderForge.SFN_Add,id:655,x:31880,y:32491,varname:node_655,prsc:2|A-809-OUT,B-6151-OUT;n:type:ShaderForge.SFN_Vector1,id:6151,x:31714,y:32682,varname:node_6151,prsc:2,v1:0.4;n:type:ShaderForge.SFN_Multiply,id:809,x:31714,y:32491,varname:node_809,prsc:2|A-7013-OUT,B-6151-OUT;n:type:ShaderForge.SFN_Multiply,id:42,x:32086,y:32491,varname:node_42,prsc:2|A-655-OUT,B-7817-OUT,C-3563-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:7817,x:31880,y:32630,varname:node_7817,prsc:2;n:type:ShaderForge.SFN_HalfVector,id:288,x:31347,y:32788,varname:node_288,prsc:2;n:type:ShaderForge.SFN_Dot,id:7759,x:31558,y:32778,varname:node_7759,prsc:2,dt:1|A-1866-OUT,B-288-OUT;n:type:ShaderForge.SFN_Multiply,id:9247,x:31910,y:32778,varname:node_9247,prsc:2|A-531-OUT,B-9079-OUT;n:type:ShaderForge.SFN_Slider,id:9079,x:31753,y:32950,ptovrint:False,ptlb:Spec_Iten,ptin:_Spec_Iten,varname:node_9079,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:10;n:type:ShaderForge.SFN_Power,id:531,x:31743,y:32778,varname:node_531,prsc:2|VAL-7759-OUT,EXP-7535-OUT;n:type:ShaderForge.SFN_Slider,id:7535,x:31401,y:32951,ptovrint:False,ptlb:Spec_Range,ptin:_Spec_Range,varname:_Spec_Iten_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:13.21818,max:20;n:type:ShaderForge.SFN_Add,id:312,x:32953,y:32752,varname:node_312,prsc:2|A-1624-OUT,B-915-OUT;n:type:ShaderForge.SFN_Multiply,id:915,x:32184,y:32911,varname:node_915,prsc:2|A-3740-RGB,B-9247-OUT;n:type:ShaderForge.SFN_Color,id:3740,x:31796,y:33069,ptovrint:False,ptlb:Spec_Color,ptin:_Spec_Color,varname:node_3740,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:4321,x:32863,y:32941,varname:node_4321,prsc:2,v1:0.0001;n:type:ShaderForge.SFN_Color,id:5035,x:32794,y:33080,ptovrint:False,ptlb:LineColor,ptin:_LineColor,varname:node_5035,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_LightColor,id:406,x:32208,y:33041,varname:node_406,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3563,x:32502,y:33078,varname:node_3563,prsc:2|A-406-RGB,B-406-A,C-4871-OUT;n:type:ShaderForge.SFN_Vector1,id:4871,x:32228,y:33184,varname:node_4871,prsc:2,v1:1.3;proporder:2453-9079-7535-3740-5035;pass:END;sub:END;*/

Shader "Toon_Tank/Tank_2" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Spec_Iten ("Spec_Iten", Range(0, 10)) = 0.5
        _Spec_Range ("Spec_Range", Range(0, 20)) = 13.21818
        _Spec_Color ("Spec_Color", Color) = (0.5,0.5,0.5,1)
        _LineColor ("LineColor", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform float4 _LineColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*0.0001,1) );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_LineColor.rgb,0);
            }
            ENDCG
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
            uniform float _Spec_Iten;
            uniform float _Spec_Range;
            uniform float4 _Spec_Color;
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
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_6151 = 0.4;
                float node_217 = 3.0;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = (((floor((((max(0,dot(lightDirection,i.normalDir))*node_6151)+node_6151)*attenuation*(_LightColor0.rgb*_LightColor0.a*1.3)) * node_217) / (node_217 - 1)+0.5)*_MainTex_var.rgb)+(_Spec_Color.rgb*(pow(max(0,dot(i.normalDir,halfDirection)),_Spec_Range)*_Spec_Iten)));
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
            uniform float _Spec_Iten;
            uniform float _Spec_Range;
            uniform float4 _Spec_Color;
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
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_6151 = 0.4;
                float node_217 = 3.0;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = (((floor((((max(0,dot(lightDirection,i.normalDir))*node_6151)+node_6151)*attenuation*(_LightColor0.rgb*_LightColor0.a*1.3)) * node_217) / (node_217 - 1)+0.5)*_MainTex_var.rgb)+(_Spec_Color.rgb*(pow(max(0,dot(i.normalDir,halfDirection)),_Spec_Range)*_Spec_Iten)));
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
