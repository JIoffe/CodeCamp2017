// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "JI/SolidOutline"
{
	Properties
	{
		_Color("Outline Color", Color) = (1,1,1,1)
		_Thickness("Outline Thickness", Float) = 0.1

		[Space(20)]
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"LessEqual"
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		ZTest[_ZTest]
		Cull[_Cull]
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 norm : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float4 _Color;
			float _Thickness;
			
			v2f vert (appdata v)
			{
				v2f o;
				half3 norm = mul((half3x3)UNITY_MATRIX_IT_MV, v.norm);
				//

				//o.vertex.xyz += norm * _Thickness;
				//float4 displacement = 
				o.vertex = mul(UNITY_MATRIX_MV, v.vertex);
				o.vertex.xyz += normalize(norm) * _Thickness;
				//o.vertex = mul(UNITY_MATRIX_P, o.vertex + float4(0, _Thickness, 0, 1));
				o.vertex = mul(UNITY_MATRIX_P, o.vertex);
				//o.vertex = UnityObjectToClipPos(v.vertex + float4(0,_Thickness,0,1));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}
