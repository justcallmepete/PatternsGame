﻿Shader "Custom/GuardVision"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,0,0,0)
		_LineThickness("LineThickness", Range (0,5)) = 2
		_LineDistance("LineDistance", Range (0,10)) = 0.5
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent"}
		LOD 100
		ZWrite off
		BlendOp Max
		Blend SrcAlpha One 

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"		

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(3)
				float4 vertex : SV_POSITION;
				float4 objVertex: TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _LineThickness;
			float _LineDistance;

			v2f vert (appdata v)
			{
				v2f o;
				o.objVertex = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				col = _Color * cos((i.objVertex.z) * _LineDistance)*_LineThickness;
				return col;
			}
			ENDCG
		}
	}
}