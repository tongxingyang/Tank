// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33601,y:32647,varname:node_9361,prsc:2|custl-7157-OUT,olwid-1490-OUT,olcol-3214-RGB;n:type:ShaderForge.SFN_Tex2d,id:8663,x:32396,y:32992,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_LightVector,id:2177,x:31469,y:32575,varname:node_2177,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:3036,x:31469,y:32712,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:283,x:31649,y:32712,varname:node_283,prsc:2,dt:1|A-2177-OUT,B-3036-OUT;n:type:ShaderForge.SFN_Multiply,id:2497,x:31636,y:32442,varname:node_2497,prsc:2|A-1502-OUT,B-868-OUT;n:type:ShaderForge.SFN_Multiply,id:7157,x:33145,y:32539,varname:node_7157,prsc:2|A-6239-OUT,B-4468-OUT,C-8663-RGB,D-3369-RGB;n:type:ShaderForge.SFN_LightAttenuation,id:4468,x:32396,y:33163,varname:node_4468,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3369,x:32396,y:33296,varname:node_3369,prsc:2;n:type:ShaderForge.SFN_Add,id:6239,x:31894,y:32445,varname:node_6239,prsc:2|A-2497-OUT,B-3579-OUT;n:type:ShaderForge.SFN_Desaturate,id:868,x:31465,y:32442,varname:node_868,prsc:2|COL-2177-OUT;n:type:ShaderForge.SFN_Slider,id:1502,x:31308,y:32357,ptovrint:False,ptlb:Lightness,ptin:_Lightness,varname:_node_1502,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-2,cur:5.901449,max:10;n:type:ShaderForge.SFN_Vector1,id:1258,x:31922,y:32930,varname:node_1258,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:3579,x:31855,y:32712,varname:node_3579,prsc:2|A-6692-OUT,B-1258-OUT;n:type:ShaderForge.SFN_Slider,id:1490,x:33020,y:32993,ptovrint:False,ptlb:Outline_Width,ptin:_Outline_Width,varname:node_1490,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:3214,x:33177,y:33105,ptovrint:False,ptlb:Outline_Color,ptin:_Outline_Color,varname:node_3214,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:6692,x:31739,y:32896,varname:node_6692,prsc:2|A-283-OUT,B-1258-OUT;proporder:8663-1502-1490-3214;pass:END;sub:END;*/

Shader "Toon_Tank/Ame_Toon_Tank" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Lightness ("Lightness", Range(-2, 10)) = 5.901449
        _Outline_Width ("Outline_Width", Range(0, 1)) = 0
        _Outline_Color ("Outline_Color", Color) = (0.5,0.5,0.5,1)
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
            uniform float _Outline_Width;
            uniform float4 _Outline_Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_Outline_Width,1) );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_Outline_Color.rgb,0);
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
            uniform float _Lightness;
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
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_1258 = 0.5;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = (((_Lightness*dot(lightDirection,float3(0.3,0.59,0.11)))+((max(0,dot(lightDirection,i.normalDir))+node_1258)*node_1258))*attenuation*_MainTex_var.rgb*_LightColor0.rgb);
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
            uniform float _Lightness;
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
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_1258 = 0.5;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = (((_Lightness*dot(lightDirection,float3(0.3,0.59,0.11)))+((max(0,dot(lightDirection,i.normalDir))+node_1258)*node_1258))*attenuation*_MainTex_var.rgb*_LightColor0.rgb);
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
