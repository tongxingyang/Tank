// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33244,y:32285,varname:node_9361,prsc:2|emission-9703-OUT,alpha-9116-OUT;n:type:ShaderForge.SFN_VertexColor,id:1409,x:31537,y:32731,varname:node_1409,prsc:2;n:type:ShaderForge.SFN_Abs,id:4643,x:31537,y:32580,varname:node_4643,prsc:2|IN-5985-OUT;n:type:ShaderForge.SFN_Multiply,id:4354,x:31757,y:32580,varname:node_4354,prsc:2|A-4643-OUT,B-1409-R;n:type:ShaderForge.SFN_ValueProperty,id:5985,x:31537,y:32508,ptovrint:False,ptlb:Random_Value,ptin:_Random_Value,varname:node_5985,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Rotator,id:164,x:31952,y:32580,varname:node_164,prsc:2|UVIN-8882-UVOUT,ANG-4354-OUT;n:type:ShaderForge.SFN_TexCoord,id:8882,x:31757,y:32420,varname:node_8882,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:9241,x:32179,y:32580,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_9241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-164-UVOUT;n:type:ShaderForge.SFN_Multiply,id:9116,x:32850,y:32719,varname:node_9116,prsc:2|A-9241-A,B-1409-A;n:type:ShaderForge.SFN_Multiply,id:9703,x:32533,y:32585,varname:node_9703,prsc:2|A-8890-OUT,B-6519-OUT,C-6612-RGB;n:type:ShaderForge.SFN_Rotator,id:8726,x:31957,y:33040,varname:node_8726,prsc:2|UVIN-8882-UVOUT,ANG-4869-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4869,x:31545,y:33072,ptovrint:False,ptlb:Light_Angle,ptin:_Light_Angle,varname:_Random_Value_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:1645,x:32163,y:33040,ptovrint:False,ptlb:MainMask,ptin:_MainMask,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:53c9ad8af3994e94bb8acbd4d417da23,ntxv:0,isnm:False|UVIN-8726-UVOUT;n:type:ShaderForge.SFN_Power,id:6519,x:32430,y:33036,varname:node_6519,prsc:2|VAL-1645-R,EXP-9342-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9342,x:32153,y:33254,ptovrint:False,ptlb:Contrast,ptin:_Contrast,varname:node_9342,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Desaturate,id:8890,x:32359,y:32458,varname:node_8890,prsc:2|COL-9241-RGB;n:type:ShaderForge.SFN_Color,id:6612,x:32179,y:32803,ptovrint:False,ptlb:Smoke_Color,ptin:_Smoke_Color,varname:node_6612,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8529412,c2:0.6985294,c3:0.5330882,c4:1;proporder:6612-9241-1645-5985-4869-9342;pass:END;sub:END;*/

Shader "FX_Shader/Fx_Smoke_Light_Shader" {
    Properties {
        [HDR]_Smoke_Color ("Smoke_Color", Color) = (0.8529412,0.6985294,0.5330882,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _MainMask ("MainMask", 2D) = "white" {}
        _Random_Value ("Random_Value", Float ) = 1
        _Light_Angle ("Light_Angle", Float ) = 1
        _Contrast ("Contrast", Float ) = 0.5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Random_Value;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Light_Angle;
            uniform sampler2D _MainMask; uniform float4 _MainMask_ST;
            uniform float _Contrast;
            uniform float4 _Smoke_Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float node_164_ang = (abs(_Random_Value)*i.vertexColor.r);
                float node_164_spd = 1.0;
                float node_164_cos = cos(node_164_spd*node_164_ang);
                float node_164_sin = sin(node_164_spd*node_164_ang);
                float2 node_164_piv = float2(0.5,0.5);
                float2 node_164 = (mul(i.uv0-node_164_piv,float2x2( node_164_cos, -node_164_sin, node_164_sin, node_164_cos))+node_164_piv);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_164, _MainTex));
                float node_8726_ang = _Light_Angle;
                float node_8726_spd = 1.0;
                float node_8726_cos = cos(node_8726_spd*node_8726_ang);
                float node_8726_sin = sin(node_8726_spd*node_8726_ang);
                float2 node_8726_piv = float2(0.5,0.5);
                float2 node_8726 = (mul(i.uv0-node_8726_piv,float2x2( node_8726_cos, -node_8726_sin, node_8726_sin, node_8726_cos))+node_8726_piv);
                float4 _MainMask_var = tex2D(_MainMask,TRANSFORM_TEX(node_8726, _MainMask));
                float3 emissive = (dot(_MainTex_var.rgb,float3(0.3,0.59,0.11))*pow(_MainMask_var.r,_Contrast)*_Smoke_Color.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,(_MainTex_var.a*i.vertexColor.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
