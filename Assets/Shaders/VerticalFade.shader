Shader "JI/VerticalFade"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		[Header(Fade Effect)]
		_clipFeatherColor("Clip Feather Color", Color) = (1,1,1,1)
		_clipFeather ("Clip Feather", Float) = 0.2
		_clipRangeTop ("Clip Range Top", Float) = 0.0
		_clipRangeBottom ("Clip Range Bottom", Float) = 0.0
		_shouldFade ("Should Fade", Float) = 1.0

		[Space(20)]
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" "PerformanceChecks" = "False" }
		LOD 100
		Cull[_Cull]

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
				float4 worldPos: TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float _shouldFade;
			float4 _MainTex_ST;
			float _clipRangeTop;
			float _clipRangeBottom;
			float4 _clipFeatherColor;
			float _clipFeather;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				if(_shouldFade < 0.5)
					return tex2D(_MainTex, i.uv);

				if (i.worldPos.y < _clipRangeBottom)
					discard;

				if (i.worldPos.y > _clipRangeTop)
					discard;

				float d = min(abs(i.worldPos.y - _clipRangeTop), abs(i.worldPos.y - _clipRangeBottom));
				float s = saturate(d / _clipFeather);
				s = smoothstep(0, 1, s);

				float4 diffuse = tex2D(_MainTex, i.uv);

				return lerp(_clipFeatherColor, diffuse, s);
			}
			ENDCG
		}
	}
}
