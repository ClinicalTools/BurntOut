Shader "Unlit/Stencil" {
	Properties
	{
		_MainTex ("Texture", any) = "" {}
		_Color("Multiplicative color", Color) = (1.0, 1.0, 1.0, 1.0)
		[Header(Stencil)]
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Float) = 8
		[IntRange] _Stencil("Stencil ID", Range(0, 255)) = 1
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp("Stencil Operation", Float) = 0
		[IntRange] _StencilWriteMask("Stencil Write Mask", Range(0, 255)) = 255
		[IntRange] _StencilReadMask("Stencil Read Mask", Range(0, 255)) = 255
		[Header(Other)]
		[Enum(UnityEngine.Rendering.CompareFunction)] _DepthComp("Depth Test", Float) = 8
		[Enum(None,0,Alpha,1,Blue,2,Green,4,Red,8,All,15)] _ColorMask("Color Mask", Float) = 15
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", Float) = 0
	}
	SubShader {

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		ColorMask [_ColorMask]

		Pass {
 			ZTest [_DepthComp] Cull [_CullMode] ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _Color;

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.texcoord) * _Color;
			}
			ENDCG 

		}
	}
	Fallback Off 
}
