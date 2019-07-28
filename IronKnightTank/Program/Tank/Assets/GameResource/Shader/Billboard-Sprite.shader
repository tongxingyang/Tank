// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Tank/Billboard-Sprite"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		_ScaleX("Scale X", Float) = 1.0
		_ScaleY("Scale Y", Float) = 1.0
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
	[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment SpriteFrag
		#pragma target 2.0
		#pragma multi_compile_instancing
		#pragma multi_compile _ PIXELSNAP_ON
		#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		#include "UnitySprites.cginc"
		uniform float _ScaleX;
		uniform float _ScaleY;
		struct vTof
		{
			float4 vertex   : SV_POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		vTof vert(appdata_t IN)
		{
			vTof OUT;

			UNITY_SETUP_INSTANCE_ID(IN);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

			#ifdef UNITY_INSTANCING_ENABLED
					IN.vertex.xy *= _Flip.xy;
			#endif

			//OUT.vertex = UnityObjectToClipPos(IN.vertex);
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color * _Color * _RendererColor;

			//#ifdef PIXELSNAP_ON
			//		OUT.vertex = UnityPixelSnap(OUT.vertex);
			//#endif

			OUT.vertex = mul(UNITY_MATRIX_P,
				mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
				+ float4(IN.vertex.x, IN.vertex.y, 0.0, 0.0)
				* float4(_ScaleX, _ScaleY, 1.0, 1.0));


			return OUT;
		}




		ENDCG
	}
	}
}
