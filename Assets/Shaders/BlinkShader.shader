Shader "Spine/Skeleton Flash" {
	Properties{
		_Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.1
		_MainTex("Texture to blend", 2D) = "black" {}
	_Flash("Flash", Range(0,1)) = 0
		_FlashColor("Flash color", Color) = (1,1,1,1)
	}
		SubShader{
		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Lighting Off

		Pass{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
	uniform float _Flash;
	uniform float4 _FlashColor;

	float4 frag(v2f_img i) : COLOR{
		float4 t = tex2D(_MainTex, i.uv);
		if (_Flash != 0) {
			t.r = t.a;
			t.g = t.a;
			t.b = t.a;
			t = t * _FlashColor;
		}
		return t;
	}
		ENDCG
	}
	}
}