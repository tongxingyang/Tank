// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|custl-6464-OUT,olwid-5478-OUT,olcol-2174-RGB;n:type:ShaderForge.SFN_Tex2d,id:4370,x:32523,y:32973,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_4370,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:5478,x:33107,y:33423,ptovrint:False,ptlb:OutLineWidth,ptin:_OutLineWidth,varname:node_5478,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1,max:1;n:type:ShaderForge.SFN_Color,id:2174,x:33264,y:33217,ptovrint:False,ptlb:OutLineColor,ptin:_OutLineColor,varname:node_2174,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_LightVector,id:7902,x:31720,y:32679,varname:node_7902,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:6580,x:31720,y:32834,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:8568,x:31952,y:32754,varname:node_8568,prsc:2,dt:1|A-7902-OUT,B-6580-OUT;n:type:ShaderForge.SFN_Add,id:4643,x:32558,y:32754,varname:node_4643,prsc:2|A-6545-OUT,B-3184-OUT;n:type:ShaderForge.SFN_Multiply,id:7330,x:32777,y:32754,varname:node_7330,prsc:2|A-4643-OUT,B-6545-OUT;n:type:ShaderForge.SFN_Multiply,id:4107,x:32729,y:32958,varname:node_4107,prsc:2|A-7330-OUT,B-4370-RGB;n:type:ShaderForge.SFN_HalfVector,id:6249,x:31731,y:33155,varname:node_6249,prsc:2;n:type:ShaderForge.SFN_Dot,id:5810,x:31963,y:33155,varname:node_5810,prsc:2,dt:1|A-6580-OUT,B-6249-OUT;n:type:ShaderForge.SFN_Slider,id:1056,x:31742,y:32996,ptovrint:False,ptlb:LightRange,ptin:_LightRange,varname:node_1056,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:7.837607,max:10;n:type:ShaderForge.SFN_Power,id:494,x:32131,y:32754,varname:node_494,prsc:2|VAL-8568-OUT,EXP-4882-OUT;n:type:ShaderForge.SFN_Multiply,id:3184,x:32343,y:32754,varname:node_3184,prsc:2|A-494-OUT,B-777-OUT;n:type:ShaderForge.SFN_Slider,id:777,x:32186,y:32969,ptovrint:False,ptlb:LightIntenity,ptin:_LightIntenity,varname:_LigtRange_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:10;n:type:ShaderForge.SFN_Slider,id:6545,x:32235,y:32586,ptovrint:False,ptlb:DarkIntenity,ptin:_DarkIntenity,varname:_LightIntenity_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:10;n:type:ShaderForge.SFN_Power,id:6426,x:32140,y:33155,varname:node_6426,prsc:2|VAL-5810-OUT,EXP-439-OUT;n:type:ShaderForge.SFN_Multiply,id:3109,x:32370,y:33155,varname:node_3109,prsc:2|A-6426-OUT,B-533-OUT;n:type:ShaderForge.SFN_Slider,id:5583,x:31806,y:33533,ptovrint:False,ptlb:SpecRange,ptin:_SpecRange,varname:node_5583,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:100;n:type:ShaderForge.SFN_RemapRange,id:439,x:31963,y:33311,varname:node_439,prsc:2,frmn:0,frmx:100,tomn:100,tomx:0|IN-5583-OUT;n:type:ShaderForge.SFN_Slider,id:533,x:32217,y:33493,ptovrint:False,ptlb:SpecIntenity,ptin:_SpecIntenity,varname:node_533,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3937968,max:10;n:type:ShaderForge.SFN_Add,id:211,x:32816,y:33115,varname:node_211,prsc:2|A-4107-OUT,B-3109-OUT;n:type:ShaderForge.SFN_LightColor,id:6303,x:32640,y:33400,varname:node_6303,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:266,x:32610,y:33274,varname:node_266,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6464,x:32991,y:33115,varname:node_6464,prsc:2|A-211-OUT,B-266-OUT,C-6303-RGB;n:type:ShaderForge.SFN_RemapRange,id:4882,x:32104,y:33016,varname:node_4882,prsc:2,frmn:0,frmx:10,tomn:10,tomx:0|IN-1056-OUT;proporder:4370-5478-2174-1056-777-6545-5583-533;pass:END;sub:END;*/

Shader "Toon_Tank/Tank_test" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _OutLineWidth ("OutLineWidth", Range(0, 1)) = 0.1
        _OutLineColor ("OutLineColor", Color) = (0.5,0.5,0.5,1)
        _LightRange ("LightRange", Range(0, 10)) = 7.837607
        _LightIntenity ("LightIntenity", Range(0, 10)) = 1
        _DarkIntenity ("DarkIntenity", Range(0, 10)) = 0.5
        _SpecRange ("SpecRange", Range(0, 100)) = 1
        _SpecIntenity ("SpecIntenity", Range(0, 10)) = 0.3937968
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
            uniform float _OutLineWidth;
            uniform float4 _OutLineColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_OutLineWidth,1) );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_OutLineColor.rgb,0);
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
            uniform float _LightRange;
            uniform float _LightIntenity;
            uniform float _DarkIntenity;
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
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = (((((_DarkIntenity+(pow(max(0,dot(lightDirection,i.normalDir)),(_LightRange*-1.0+10.0))*_LightIntenity))*_DarkIntenity)*_MainTex_var.rgb)+(pow(max(0,dot(i.normalDir,halfDirection)),(_SpecRange*-1.0+100.0))*_SpecIntenity))*attenuation*_LightColor0.rgb);
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
            uniform float _LightRange;
            uniform float _LightIntenity;
            uniform float _DarkIntenity;
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
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = (((((_DarkIntenity+(pow(max(0,dot(lightDirection,i.normalDir)),(_LightRange*-1.0+10.0))*_LightIntenity))*_DarkIntenity)*_MainTex_var.rgb)+(pow(max(0,dot(i.normalDir,halfDirection)),(_SpecRange*-1.0+100.0))*_SpecIntenity))*attenuation*_LightColor0.rgb);
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
