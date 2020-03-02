// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Created/Framecolor"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TilingX("Tiling X", Range( 1 , 1.5)) = 1
		_TilingY("Tiling Y", Range( 1 , 1.5)) = 1
		_BlackFrame("BlackFrame", 2D) = "white" {}
		_WhiteFrame("WhiteFrame", 2D) = "white" {}
		_Colorframe("Color frame", Color) = (1,0,0,0)

	}

	SubShader
	{
		LOD 0

		
		
		ZTest LEqual
		Cull Off
		ZWrite On

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			

			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float _TilingX;
			uniform float _TilingY;
			uniform sampler2D _BlackFrame;
			uniform sampler2D _WhiteFrame;
			uniform float4 _Colorframe;


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float TilingX30 = _TilingX;
				float TilingY31 = _TilingY;
				float2 appendResult10 = (float2(TilingX30 , TilingY31));
				float2 appendResult19 = (float2(-( ( TilingX30 - 1.0 ) / 2.0 ) , -( ( TilingY31 - 1.0 ) / 2.0 )));
				float2 uv06 = i.uv.xy * appendResult10 + appendResult19;
				float2 TextureScalePosition80 = uv06;
				

				finalColor = ( ( tex2D( _MainTex, TextureScalePosition80 ) * tex2D( _BlackFrame, TextureScalePosition80 ).r ) + ( tex2D( _WhiteFrame, TextureScalePosition80 ) * _Colorframe ) );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17700
0;71;1365;580;2812.62;747.9515;2.928758;True;True
Node;AmplifyShaderEditor.RangedFloatNode;8;-2439.088,-384.8221;Inherit;False;Property;_TilingX;Tiling X;0;0;Create;True;0;0;False;0;1;1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-2439.088,-308.8221;Inherit;False;Property;_TilingY;Tiling Y;1;0;Create;True;0;0;False;0;1;1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-2092.268,-271.4445;Inherit;False;TilingY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-2096.387,-418.2843;Inherit;False;TilingX;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;33;-2630.139,330.7031;Inherit;False;31;TilingY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-2624.486,95.78247;Inherit;False;30;TilingX;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2678.264,200.3246;Inherit;False;Constant;_ReferenceValueTiling;ReferenceValueTiling;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;15;-2333.044,267.9381;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2191.097,217.407;Inherit;False;Constant;_Middle;Middle;1;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;14;-2334.097,157.4071;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;-2028.097,155.407;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;18;-2026.097,266.407;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;22;-1895.944,266.9988;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;23;-1900.944,154.9989;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-1615.412,191.4572;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-1766.817,-359.1877;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1455,-31.80807;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;-1136.07,-35.22482;Inherit;False;TextureScalePosition;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;85;-1148.96,488.3536;Inherit;False;80;TextureScalePosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-566.8992,-74.71565;Inherit;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;81;-698.7906,29.26769;Inherit;False;80;TextureScalePosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;130;-698.0853,280.2002;Inherit;True;Property;_BlackFrame;BlackFrame;2;0;Create;True;0;0;False;0;-1;4ec3b3f6362ec834893ec89930718fe5;4ec3b3f6362ec834893ec89930718fe5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-306.5828,9.185967;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;134;-699.8356,520.6195;Inherit;True;Property;_WhiteFrame;WhiteFrame;3;0;Create;True;0;0;False;0;-1;4ec3b3f6362ec834893ec89930718fe5;4ec3b3f6362ec834893ec89930718fe5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;137;-603.4069,722.0298;Inherit;False;Property;_Colorframe;Color frame;4;0;Create;True;0;0;False;0;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;9.787159,272.8192;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-255.8303,616.1649;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;185.2694,586.8799;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;399.5465,442.129;Float;False;True;-1;2;ASEMaterialInspector;0;2;Created/Framecolor;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;0;False;-1;True;0;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;31;0;9;0
WireConnection;30;0;8;0
WireConnection;15;0;33;0
WireConnection;15;1;12;0
WireConnection;14;0;32;0
WireConnection;14;1;12;0
WireConnection;16;0;14;0
WireConnection;16;1;17;0
WireConnection;18;0;15;0
WireConnection;18;1;17;0
WireConnection;22;0;18;0
WireConnection;23;0;16;0
WireConnection;19;0;23;0
WireConnection;19;1;22;0
WireConnection;10;0;30;0
WireConnection;10;1;31;0
WireConnection;6;0;10;0
WireConnection;6;1;19;0
WireConnection;80;0;6;0
WireConnection;130;1;85;0
WireConnection;2;0;1;0
WireConnection;2;1;81;0
WireConnection;134;1;85;0
WireConnection;131;0;2;0
WireConnection;131;1;130;1
WireConnection;136;0;134;0
WireConnection;136;1;137;0
WireConnection;135;0;131;0
WireConnection;135;1;136;0
WireConnection;0;0;135;0
ASEEND*/
//CHKSM=FBA24BD9654DBCFCB178A5FECC66A8C57B17472C