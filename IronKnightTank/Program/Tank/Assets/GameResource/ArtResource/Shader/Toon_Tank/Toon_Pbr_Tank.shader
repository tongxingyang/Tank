// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:Mobile/Bumped Diffuse,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33601,y:32647,varname:node_9361,prsc:2|custl-2488-OUT,olwid-1490-OUT,olcol-3214-RGB;n:type:ShaderForge.SFN_Tex2d,id:8663,x:32396,y:32992,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_LightVector,id:2177,x:31373,y:32577,varname:node_2177,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:3036,x:31373,y:32709,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:283,x:31550,y:32577,varname:node_283,prsc:2,dt:0|A-2177-OUT,B-3036-OUT;n:type:ShaderForge.SFN_Multiply,id:2497,x:31550,y:32435,varname:node_2497,prsc:2|A-1502-OUT,B-868-OUT;n:type:ShaderForge.SFN_Multiply,id:7157,x:32723,y:32578,varname:node_7157,prsc:2|A-6239-OUT,B-4468-OUT,C-7641-OUT,D-3369-RGB;n:type:ShaderForge.SFN_LightAttenuation,id:4468,x:32396,y:33163,varname:node_4468,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3369,x:32396,y:33296,varname:node_3369,prsc:2;n:type:ShaderForge.SFN_Add,id:6239,x:32396,y:32581,varname:node_6239,prsc:2|A-2497-OUT,B-3579-OUT,C-6647-OUT;n:type:ShaderForge.SFN_Desaturate,id:868,x:31373,y:32435,varname:node_868,prsc:2|COL-2177-OUT;n:type:ShaderForge.SFN_Slider,id:1502,x:31216,y:32342,ptovrint:False,ptlb:Lightness,ptin:_Lightness,varname:_node_1502,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-2,cur:3.218337,max:10;n:type:ShaderForge.SFN_Vector1,id:1258,x:31684,y:32748,varname:node_1258,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:3579,x:31952,y:32577,varname:node_3579,prsc:2|A-6692-OUT,B-6516-OUT;n:type:ShaderForge.SFN_Slider,id:1490,x:33602,y:33320,ptovrint:False,ptlb:Outline_Width,ptin:_Outline_Width,varname:node_1490,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:3214,x:33602,y:33141,ptovrint:False,ptlb:Outline_Color,ptin:_Outline_Color,varname:node_3214,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:6692,x:31739,y:32577,varname:node_6692,prsc:2|A-283-OUT,B-1258-OUT;n:type:ShaderForge.SFN_Multiply,id:2488,x:33080,y:32559,varname:node_2488,prsc:2|A-5182-RGB,B-6404-OUT;n:type:ShaderForge.SFN_Color,id:5182,x:32908,y:32420,ptovrint:False,ptlb:MainColor,ptin:_MainColor,varname:node_5182,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Desaturate,id:7641,x:32396,y:32805,varname:node_7641,prsc:2|COL-8663-RGB;n:type:ShaderForge.SFN_Add,id:6404,x:32908,y:32578,varname:node_6404,prsc:2|A-7157-OUT,B-5450-OUT;n:type:ShaderForge.SFN_HalfVector,id:8880,x:31373,y:32873,varname:node_8880,prsc:2;n:type:ShaderForge.SFN_Dot,id:3838,x:31542,y:32873,varname:node_3838,prsc:2,dt:1|A-3036-OUT,B-8880-OUT;n:type:ShaderForge.SFN_Power,id:3438,x:31727,y:32873,varname:node_3438,prsc:2|VAL-3838-OUT,EXP-4346-OUT;n:type:ShaderForge.SFN_Exp,id:4346,x:31727,y:33021,varname:node_4346,prsc:2,et:1|IN-6683-OUT;n:type:ShaderForge.SFN_Slider,id:6683,x:31373,y:33050,ptovrint:False,ptlb:Spec_Rage,ptin:_Spec_Rage,varname:node_6683,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2.148709,max:10;n:type:ShaderForge.SFN_Multiply,id:1650,x:31926,y:32873,varname:node_1650,prsc:2|A-3438-OUT,B-1901-OUT;n:type:ShaderForge.SFN_Slider,id:1901,x:31570,y:33199,ptovrint:False,ptlb:Spec_Inten,ptin:_Spec_Inten,varname:_node_6683_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3386827,max:10;n:type:ShaderForge.SFN_Multiply,id:6647,x:32188,y:32873,varname:node_6647,prsc:2|A-1650-OUT,B-9985-RGB,C-2066-RGB;n:type:ShaderForge.SFN_Tex2d,id:9985,x:31926,y:33028,ptovrint:False,ptlb:GlossTex,ptin:_GlossTex,varname:node_9985,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:6516,x:31950,y:32777,ptovrint:False,ptlb:Shadow,ptin:_Shadow,varname:node_6516,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Multiply,id:5450,x:32723,y:32751,varname:node_5450,prsc:2|A-7641-OUT,B-6516-OUT;n:type:ShaderForge.SFN_Color,id:2066,x:32188,y:33064,ptovrint:False,ptlb:Spec_Color,ptin:_Spec_Color,varname:node_2066,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:8663-1502-1490-3214-5182-6683-1901-9985-6516-2066;pass:END;sub:END;*/

Shader "Toon_Tank/Toon_Pbr_Tank" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Lightness ("Lightness", Range(-2, 10)) = 3.218337
        _Outline_Width ("Outline_Width", Range(0, 1)) = 0
        _Outline_Color ("Outline_Color", Color) = (0.5,0.5,0.5,1)
        _MainColor ("MainColor", Color) = (0.5,0.5,0.5,1)
        _Spec_Rage ("Spec_Rage", Range(0, 10)) = 2.148709
        _Spec_Inten ("Spec_Inten", Range(0, 10)) = 0.3386827
        _GlossTex ("GlossTex", 2D) = "white" {}
        _Shadow ("Shadow", Range(0, 1)) = 0.5
        _Spec_Color ("Spec_Color", Color) = (0.5,0.5,0.5,1)
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
            uniform float4 _MainColor;
            uniform float _Spec_Rage;
            uniform float _Spec_Inten;
            uniform sampler2D _GlossTex; uniform float4 _GlossTex_ST;
            uniform float _Shadow;
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
                float4 _GlossTex_var = tex2D(_GlossTex,TRANSFORM_TEX(i.uv0, _GlossTex));
                float3 node_6647 = ((pow(max(0,dot(i.normalDir,halfDirection)),exp2(_Spec_Rage))*_Spec_Inten)*_GlossTex_var.rgb*_Spec_Color.rgb);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_7641 = dot(_MainTex_var.rgb,float3(0.3,0.59,0.11));
                float3 node_2488 = (_MainColor.rgb*((((_Lightness*dot(lightDirection,float3(0.3,0.59,0.11)))+((dot(lightDirection,i.normalDir)+0.5)*_Shadow)+node_6647)*attenuation*node_7641*_LightColor0.rgb)+(node_7641*_Shadow)));
                float3 finalColor = node_2488;
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
            uniform float4 _MainColor;
            uniform float _Spec_Rage;
            uniform float _Spec_Inten;
            uniform sampler2D _GlossTex; uniform float4 _GlossTex_ST;
            uniform float _Shadow;
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
                float4 _GlossTex_var = tex2D(_GlossTex,TRANSFORM_TEX(i.uv0, _GlossTex));
                float3 node_6647 = ((pow(max(0,dot(i.normalDir,halfDirection)),exp2(_Spec_Rage))*_Spec_Inten)*_GlossTex_var.rgb*_Spec_Color.rgb);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_7641 = dot(_MainTex_var.rgb,float3(0.3,0.59,0.11));
                float3 node_2488 = (_MainColor.rgb*((((_Lightness*dot(lightDirection,float3(0.3,0.59,0.11)))+((dot(lightDirection,i.normalDir)+0.5)*_Shadow)+node_6647)*attenuation*node_7641*_LightColor0.rgb)+(node_7641*_Shadow)));
                float3 finalColor = node_2488;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Mobile/Bumped Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
