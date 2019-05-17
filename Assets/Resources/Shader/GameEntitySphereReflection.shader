#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


//////////////////////////////////////////////////////////////////////////
//
//   FileName : GameEntitySphereReflection.shader
//     Author : Chiyer
// CreateTime : 2014-03-12
//       Desc :
//
//////////////////////////////////////////////////////////////////////////
Shader "Game-X/GameEntity/SphereReflection" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_ReflectTex("Reflect", 2D) = "black" {}
		_ColorFactor("_Color Factor", Range(0.0, 2.0)) = 0.9
		_ReflectFactor("Reflect Factor", Range(0.0, 2.0)) = 1
		_ReflectPower("Reflect Power", Range(1.0, 10.0)) = 1
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_Alpha("Alpha",Range(0,1)) = 0.4
		_BlinkTime("_BlinkTime",Range(0,1)) = 0
		_BlinkColor("Blink Color", Color) = (1,1,1,1)
	}
	SubShader{
		Pass{
			Tags{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}
			Lighting Off
			// 源RGB*源A + 背景RGB*(1-源A)    
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag     
#include "UnityCG.cginc"

			struct vsIn{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};
			struct vsOut{
				float4  vertex : SV_POSITION;
				float2  uv : TEXCOORD0;
				float2  reflect : TEXCOORD1;
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			sampler2D _ReflectTex;
			float4 _ReflectTex_ST;
			fixed _ColorFactor;
			fixed _ReflectFactor;
			fixed _ReflectPower;
			fixed _Cutoff;
			fixed4 _BlinkColor;
			fixed _Alpha;
			fixed _BlinkTime;

			vsOut vert(vsIn v){
				vsOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				//model -> view ,so scale change it. so should resize the normal by the build-in variable : unity_Scale
				o.reflect = mul((float3x3)UNITY_MATRIX_MV, v.normal * 1.0).xy * 0.5 + 0.5;
				return o;
			}
			fixed4 frag(vsOut In) : COLOR{
				fixed4 c;
				fixed4 albedo = tex2D(_MainTex, In.uv) * _Color;
				//fixed3 ambient = fixed3(1,1,1) + (UNITY_LIGHTMODEL_AMBIENT.rgb - fixed3(0.25,0.25,0.25)) * 4;
				fixed3 ambient = fixed3(0.45, 0.45, 0.45);
				fixed3 ref = tex2D(_ReflectTex, In.reflect);

				if (_BlinkTime >0){
					c.rgb = albedo.rgb + _BlinkColor * _Alpha * _Color.a;
				}
				else{
					c.rgb = albedo.rgb * ambient * _ColorFactor + pow(ref, _ReflectPower) * _ReflectFactor *_Color.a;
				}
				//fixed3 color1 = albedo.rgb + _BlinkColor * _Alpha * _Color.a; // _BlinkTime=1
				//fixed3 color2 = albedo.rgb * ambient * _ColorFactor + pow(ref, _ReflectPower) * _ReflectFactor *_Color.a; //_BlinkTime=0
				//c.rgb = lerp(color2, color1, _BlinkTime*2);

				c.a = albedo.a * _Color.a;
				clip(c.a - _Cutoff);
				return c;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
