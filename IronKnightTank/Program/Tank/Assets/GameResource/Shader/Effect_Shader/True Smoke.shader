// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33318,y:32224,varname:node_4013,prsc:2|emission-4466-OUT,alpha-4227-OUT;n:type:ShaderForge.SFN_Rotator,id:380,x:31803,y:32359,varname:node_380,prsc:2|UVIN-7550-UVOUT,ANG-7102-OUT;n:type:ShaderForge.SFN_TexCoord,id:7550,x:31568,y:32284,varname:node_7550,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_VertexColor,id:588,x:31129,y:32645,varname:node_588,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7102,x:31576,y:32578,varname:node_7102,prsc:2|A-7396-OUT,B-588-R;n:type:ShaderForge.SFN_Tex2d,id:3357,x:32196,y:32379,ptovrint:False,ptlb:Texture_smoke,ptin:_Texture_smoke,varname:node_3357,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-380-UVOUT;n:type:ShaderForge.SFN_Multiply,id:4466,x:32804,y:32388,varname:node_4466,prsc:2|A-3357-RGB,B-5684-OUT;n:type:ShaderForge.SFN_Tex2d,id:3376,x:32199,y:32886,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_3376,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2617-UVOUT;n:type:ShaderForge.SFN_Multiply,id:4227,x:32776,y:32704,varname:node_4227,prsc:2|A-3357-A,B-588-A;n:type:ShaderForge.SFN_Rotator,id:2617,x:31984,y:32886,varname:node_2617,prsc:2|UVIN-7971-UVOUT,ANG-2042-OUT;n:type:ShaderForge.SFN_TexCoord,id:7971,x:31760,y:32809,varname:node_7971,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:2042,x:31760,y:33016,ptovrint:False,ptlb:01_光照方向,ptin:_01_,varname:node_2042,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:302,x:31129,y:32469,ptovrint:False,ptlb:02_随机系数,ptin:_02_,cmnt:数值越大随机程度越高,varname:node_302,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Abs,id:7396,x:31307,y:32469,varname:node_7396,prsc:2|IN-302-OUT;n:type:ShaderForge.SFN_Power,id:5684,x:32477,y:32904,varname:node_5684,prsc:2|VAL-3376-R,EXP-8938-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8938,x:32256,y:33120,ptovrint:False,ptlb:03_明暗对比,ptin:_03_,varname:node_8938,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;proporder:3357-3376-2042-302-8938;pass:END;sub:END;*/

Shader "Shader Forge/NewShader" {
    Properties {
        _Texture_smoke ("Texture_smoke", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _01_ ("01_光照方向", Float ) = 0
        _02_ ("02_随机系数", Float ) = 1
        _03_ ("03_明暗对比", Float ) = 0.5
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
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Texture_smoke; uniform float4 _Texture_smoke_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _01_;
            uniform float _02_;
            uniform float _03_;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float node_380_ang = (abs(_02_)*i.vertexColor.r);
                float node_380_spd = 1.0;
                float node_380_cos = cos(node_380_spd*node_380_ang);
                float node_380_sin = sin(node_380_spd*node_380_ang);
                float2 node_380_piv = float2(0.5,0.5);
                float2 node_380 = (mul(i.uv0-node_380_piv,float2x2( node_380_cos, -node_380_sin, node_380_sin, node_380_cos))+node_380_piv);
                float4 _Texture_smoke_var = tex2D(_Texture_smoke,TRANSFORM_TEX(node_380, _Texture_smoke));
                float node_2617_ang = _01_;
                float node_2617_spd = 1.0;
                float node_2617_cos = cos(node_2617_spd*node_2617_ang);
                float node_2617_sin = sin(node_2617_spd*node_2617_ang);
                float2 node_2617_piv = float2(0.5,0.5);
                float2 node_2617 = (mul(i.uv0-node_2617_piv,float2x2( node_2617_cos, -node_2617_sin, node_2617_sin, node_2617_cos))+node_2617_piv);
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(node_2617, _Mask));
                float3 emissive = (_Texture_smoke_var.rgb*pow(_Mask_var.r,_03_));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(_Texture_smoke_var.a*i.vertexColor.a));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
