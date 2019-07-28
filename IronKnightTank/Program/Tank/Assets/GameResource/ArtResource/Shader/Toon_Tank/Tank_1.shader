// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33272,y:32524,varname:node_9361,prsc:2|custl-8650-OUT;n:type:ShaderForge.SFN_LightVector,id:4712,x:32030,y:32497,varname:node_4712,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:3191,x:32030,y:32649,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:6745,x:32240,y:32497,varname:node_6745,prsc:2,dt:1|A-4712-OUT,B-3191-OUT;n:type:ShaderForge.SFN_Multiply,id:2360,x:32620,y:32497,varname:node_2360,prsc:2|A-3040-OUT,B-9489-OUT;n:type:ShaderForge.SFN_Add,id:3040,x:32433,y:32497,varname:node_3040,prsc:2|A-6745-OUT,B-9489-OUT;n:type:ShaderForge.SFN_Vector1,id:9489,x:32240,y:32649,varname:node_9489,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:7012,x:32808,y:32497,varname:node_7012,prsc:2|A-2360-OUT,B-6951-RGB,C-3566-RGB;n:type:ShaderForge.SFN_Tex2d,id:6951,x:32620,y:32648,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_6951,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:3566,x:32620,y:32852,ptovrint:False,ptlb:MainColor,ptin:_MainColor,varname:node_3566,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_HalfVector,id:9286,x:32037,y:33107,varname:node_9286,prsc:2;n:type:ShaderForge.SFN_Dot,id:8234,x:32253,y:33107,varname:node_8234,prsc:2,dt:1|A-3191-OUT,B-9286-OUT;n:type:ShaderForge.SFN_Power,id:5968,x:32419,y:33107,varname:node_5968,prsc:2|VAL-8234-OUT,EXP-8882-OUT;n:type:ShaderForge.SFN_Slider,id:8882,x:32096,y:33285,ptovrint:False,ptlb:SpecRange,ptin:_SpecRange,varname:node_8882,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:10,max:20;n:type:ShaderForge.SFN_Multiply,id:9792,x:32620,y:33107,varname:node_9792,prsc:2|A-5968-OUT,B-641-OUT;n:type:ShaderForge.SFN_Slider,id:641,x:32463,y:33287,ptovrint:False,ptlb:SpecIntenity,ptin:_SpecIntenity,varname:_SpecRange_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:8;n:type:ShaderForge.SFN_Multiply,id:5931,x:32838,y:33107,varname:node_5931,prsc:2|A-9792-OUT,B-5488-RGB;n:type:ShaderForge.SFN_LightColor,id:5488,x:32838,y:33292,varname:node_5488,prsc:2;n:type:ShaderForge.SFN_Add,id:5090,x:33011,y:33107,varname:node_5090,prsc:2|A-7012-OUT,B-5931-OUT;n:type:ShaderForge.SFN_Multiply,id:8650,x:33113,y:32844,varname:node_8650,prsc:2|A-5090-OUT,B-5092-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:5092,x:33113,y:32964,varname:node_5092,prsc:2;proporder:6951-3566-8882-641;pass:END;sub:END;*/

Shader "Toon_Tank/Tank_1" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainColor ("MainColor", Color) = (0.5,0.5,0.5,1)
        _SpecRange ("SpecRange", Range(0, 20)) = 10
        _SpecIntenity ("SpecIntenity", Range(0, 8)) = 0.5
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
            uniform float4 _MainColor;
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
                float node_9489 = 0.5;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = (((((max(0,dot(lightDirection,i.normalDir))+node_9489)*node_9489)*_MainTex_var.rgb*_MainColor.rgb)+((pow(max(0,dot(i.normalDir,halfDirection)),_SpecRange)*_SpecIntenity)*_LightColor0.rgb))*attenuation);
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
            uniform float4 _MainColor;
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
                float node_9489 = 0.5;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = (((((max(0,dot(lightDirection,i.normalDir))+node_9489)*node_9489)*_MainTex_var.rgb*_MainColor.rgb)+((pow(max(0,dot(i.normalDir,halfDirection)),_SpecRange)*_SpecIntenity)*_LightColor0.rgb))*attenuation);
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
