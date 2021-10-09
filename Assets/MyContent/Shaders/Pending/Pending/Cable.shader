// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New/Cable"
{
	Properties
	{
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.5
		_Metallic("Metallic", Range( 0 , 1)) = 0.5
		_PowerLight("PowerLight", Range( 0 , 1)) = 0
		_LightColor("LightColor", Color) = (1,1,1,0)
		_TexturePattern("Texture Pattern", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TexturePattern;
		uniform float4 _LightColor;
		uniform float _PowerLight;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color12 = IsGammaSpace() ? float4(0.5019608,0.5019608,1,1) : float4(0.2158605,0.2158605,1,1);
			float4 color13 = IsGammaSpace() ? float4(0,0,1,1) : float4(0,0,1,1);
			float2 uv_TexCoord21 = i.uv_texcoord * float2( 1,10 );
			float TexturePatternReference24 = tex2D( _TexturePattern, uv_TexCoord21 ).r;
			float4 lerpResult14 = lerp( color12 , color13 , TexturePatternReference24);
			o.Normal = lerpResult14.rgb;
			float4 color19 = IsGammaSpace() ? float4(0.3018868,0.3018868,0.3018868,0) : float4(0.07417665,0.07417665,0.07417665,0);
			float4 color23 = IsGammaSpace() ? float4(0.6509434,0.6509434,0.6509434,0) : float4(0.3812781,0.3812781,0.3812781,0);
			float4 lerpResult26 = lerp( color19 , color23 , TexturePatternReference24);
			o.Albedo = lerpResult26.rgb;
			float4 color31 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 lerpResult34 = lerp( color31 , _LightColor , TexturePatternReference24);
			o.Emission = ( lerpResult34 * _PowerLight ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
0;71;1365;580;2610.822;308.4402;2.871618;True;True
Node;AmplifyShaderEditor.Vector2Node;22;-2238.563,408.9057;Inherit;False;Constant;_Tiling;Tiling;2;0;Create;True;0;0;False;0;1,10;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-2002.564,391.9057;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-1723.349,363.7617;Inherit;True;Property;_TexturePattern;Texture Pattern;4;0;Create;True;0;0;False;0;-1;05417d6077ff59a49a57bc237b0eb306;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-1381.058,387.8883;Inherit;False;TexturePatternReference;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;33;-766.4686,493.4091;Inherit;False;24;TexturePatternReference;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;-718.1037,324.9876;Inherit;False;Property;_LightColor;LightColor;3;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;31;-715.1046,160.0786;Inherit;False;Constant;_Metal;Metal;2;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-1172.245,-221.757;Inherit;False;Constant;_NormalBase;NormalBase;2;0;Create;True;0;0;False;0;0.5019608,0.5019608,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;13;-1168.472,-58.38451;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;0,0,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-524.8025,-278.3329;Inherit;False;Constant;_MetalLight;MetalLight;2;0;Create;True;0;0;False;0;0.6509434,0.6509434,0.6509434,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;34;-399.6466,421.6908;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-403.5705,619.404;Inherit;False;Property;_Smoothness;Smoothness;0;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;19;-507.2078,-440.0186;Inherit;False;Constant;_MetalColor;MetalColor;2;0;Create;True;0;0;False;0;0.3018868,0.3018868,0.3018868,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;28;-573.1674,-109.9114;Inherit;False;24;TexturePatternReference;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;-1260.484,129.6734;Inherit;False;24;TexturePatternReference;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-499.6414,110.3463;Inherit;False;Property;_PowerLight;PowerLight;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;14;-800.8409,24.97752;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-212.6414,177.3463;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-406.6632,538.6843;Inherit;False;Property;_Metallic;Metallic;1;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;26;-206.3453,-181.6297;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;37;-59.63025,625.2051;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;New/Cable;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;22;0
WireConnection;18;1;21;0
WireConnection;24;0;18;1
WireConnection;34;0;31;0
WireConnection;34;1;32;0
WireConnection;34;2;33;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;14;2;25;0
WireConnection;36;0;34;0
WireConnection;36;1;35;0
WireConnection;26;0;19;0
WireConnection;26;1;23;0
WireConnection;26;2;28;0
WireConnection;37;0;2;0
WireConnection;0;0;26;0
WireConnection;0;1;14;0
WireConnection;0;2;36;0
WireConnection;0;3;29;0
WireConnection;0;4;37;0
ASEEND*/
//CHKSM=27F022493EF8707BB270DCC76F33CBA0E79DCB6E