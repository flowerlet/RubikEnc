Shader "ShadowBlade/CubeLambert" {
    Properties {
        _Color ("Color", Color) = (0.9568627,0.9568627,0.9568627,1)
    }
    SubShader {
		Tags{"RenderType"="Opaque"}
        LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		float4 _Color;
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {

			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}

		inline float4 LightingBasicDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten) {
			//float difLight = max(0, dot(s.Normal, lightDir));
			float difLight = dot(s.Normal, lightDir);
			difLight = difLight * 0.55 + 0.45;
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 1);
			col.a = s.Alpha;
			return col;
		}

		ENDCG
    } 
    FallBack "Diffuse"
}