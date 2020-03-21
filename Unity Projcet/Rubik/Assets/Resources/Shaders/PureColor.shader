Shader "ShadowBlade/PureColor"
{
	Properties
	{
		_Color("MainColor", Color) = (1, 1, 1, 1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4 vert(float4 vertex : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			float4 _Color;

			float4 frag() : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}
