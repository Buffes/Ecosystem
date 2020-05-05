Shader "Custom/StatusBarColor"
{
	Properties{
	[PerRendererData] _Color("Color", Color) = (1,1,1,1)
	[PerRendererData] _BackgroundColor("BackgroundColor", Color) = (1,1,1,1)
	_MainTex("Main Tex (RGBA)", 2D) = "white" {}
	_Progress("Progress", Range(0.0,1.0)) = 0.0
	}

		SubShader{
			Tags { "Queue" = "Overlay" }
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
			Pass {

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		//uniform float4 _Color;
		uniform float _Progress;

		struct v2f {
			float4 pos : POSITION;
			float2 uv : TEXCOORD0;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
		UNITY_DEFINE_INSTANCED_PROP(fixed4, _BackgroundColor)
		UNITY_DEFINE_INSTANCED_PROP(float, _Fill)
		UNITY_INSTANCING_BUFFER_END(Props)

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_UV(0);

			return o;
		}

		half4 frag(v2f i) : COLOR
		{
			half4 color = tex2D(_MainTex, i.uv);
			float fill = UNITY_ACCESS_INSTANCED_PROP(Props, _Fill);
			color.rgba = i.uv.x < fill ? UNITY_ACCESS_INSTANCED_PROP(Props, _Color).rgba
				: UNITY_ACCESS_INSTANCED_PROP(Props, _BackgroundColor).rgba;
			return color;
		}

		ENDCG

			}
	}
}
