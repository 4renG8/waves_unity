// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/Spawn Effect" {
	Properties {

		_DistortionTex("Distortion texture", 2D) = "black" {}
		_OffsetFactor("Offset Factor", Float) = 0.1
		_DistortionSpread("Distortion Spread", Float) = 10
		_AnimationDamper("Animation Damper", Float) = 20
		_Step("Step", Float) = 0.5
		_WaveDamper("Wave Damper", Float) = 30
		_BeltDamper("Wave Damper", Float) = 5
	}

	SubShader {
		Tags { 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _DistortionTex;
			uniform float _OffsetFactor;
			uniform float _DistortionSpread;
			uniform float _AnimationDamper;
			uniform float _Step;
			uniform float _WaveDamper;
			uniform float _BeltDamper;

			struct VertexInput {
				float4 pos : POSITION;
				float2 texcoord0 : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float4 color : COLOR;
			};

			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 color : COLOR;
			};

			VertexOutput vert(VertexInput v) {
				VertexOutput o;
				o.uv0 = v.texcoord0;
				o.uv1 = v.texcoord1;
				o.color = v.color;
				o.pos = mul(UNITY_MATRIX_MVP, v.pos);
				return o;
			}

			float4 frag(VertexOutput i) : COLOR {
				float  part = 1 - _Step;
				float2 offset = float2(
					tex2D(_DistortionTex, float2(i.uv1.y / _DistortionSpread, _Time[2] / _AnimationDamper)).r,
					tex2D(_DistortionTex, float2(_Time[2] / _AnimationDamper, i.uv1.x / _DistortionSpread)).g
					);
				offset -= 0.5;
				offset *= part;
				float beltOffset = offset.y / _WaveDamper;
				float glowIntensity = -abs((i.uv0.y - beltOffset) * _BeltDamper + 1 - (_Step * (_BeltDamper + 2))) + 1;
				half4 color = i.color;
				float percent = (1 + i.uv0.y - _Step + beltOffset) / 2;
				color.a = clamp(percent < 0.5 ? 1 : 0, 0, color.a);
				half4 outColor = (tex2D(_MainTex, (i.uv0 + offset / _OffsetFactor))) * color;
				outColor.rgb += clamp(1 * glowIntensity, 0, 1);
				return outColor;
			}

		ENDCG
		}
	}
}
