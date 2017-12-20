Shader "JI/Particles/ColoredAdditive"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)

		[Space(20)]
		[Header(Blending)]
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("SrcBlend", Float) = 1 //"One"
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("DestBlend", Float) = 0 //"Zero"
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp("BlendOp", Float) = 0 //"Add"

		[Space(20)]
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"LessEqual"
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
		Cull[_Cull]
		ZTest[_ZTest]
		ZWrite Off
		Blend[_SrcBlend][_DstBlend]
		BlendOp[_BlendOp]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color * tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}
}
