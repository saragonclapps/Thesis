// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Created/waterfall"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Albedo("Albedo", 2D) = "white" {}
		_ColorAlbedo("Color Albedo", Color) = (0,0,0,0)
		_Normal("Normal", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_Force("Force", Range( 0 , 10)) = 0
		_Speed("Speed", Range( 0 , 1)) = 1
		_Tilling("Tilling", Vector) = (0,0,0,0)
		_Reflection("Reflection", Range( 0.001 , 1)) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend One Zero , SrcAlpha OneMinusSrcAlpha
		BlendOp Add , Add
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 texcoord_0;
		};

		uniform sampler2D _Normal;
		uniform float2 _Tilling;
		uniform float _Speed;
		uniform float _Force;
		uniform sampler2D _Albedo;
		uniform float4 _ColorAlbedo;
		uniform float _Reflection;
		uniform float _Opacity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime2 = _Time.y * _Speed;
			float2 appendResult63 = (float2(0.0 , ( mulTime2 * _Force )));
			o.texcoord_0.xy = v.texcoord.xy * _Tilling + appendResult63;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = tex2D( _Normal, i.texcoord_0 ).rgb;
			float4 tex2DNode16 = tex2D( _Albedo, i.texcoord_0 );
			float4 temp_output_66_0 = ( tex2DNode16 * _ColorAlbedo );
			o.Albedo = temp_output_66_0.xyz;
			o.Emission = ( temp_output_66_0 * 0.3 ).xyz;
			o.Metallic = tex2DNode16.r;
			o.Smoothness = _Reflection;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma only_renderers d3d9 d3d11 glcore gles gles3 d3d11_9x 
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
7;29;1352;692;1143.44;78.96815;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;1;-2270.566,32.83717;Float;False;Property;_Speed;Speed;5;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;62;-2102.942,125.9844;Float;False;Property;_Force;Force;4;0;0;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;2;-1934.566,37.83719;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-1742.942,71.98444;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;63;-1576.587,111.7808;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.Vector2Node;65;-1731.634,-79.51279;Float;False;Property;_Tilling;Tilling;6;0;0,0;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1433.437,3.634311;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;16;-1119.466,22.48747;Float;True;Property;_Albedo;Albedo;0;0;Assets/Art/Shaders/Texture/Water.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Image;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;68;-1036.884,-159.3597;Float;False;Property;_ColorAlbedo;Color Albedo;1;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;69;-802.8589,346.8754;Float;False;Constant;_ColorForce;ColorForce;2;0;0.3;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-724.4454,66.29453;Float;False;2;2;0;FLOAT4;0.0;False;1;COLOR;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-513.8635,165.272;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;73;-1111.015,216.3015;Float;True;Property;_Normal;Normal;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Image;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;27;-796.4545,528.7285;Float;False;Property;_Opacity;Opacity;3;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;33;-801.1076,445.5417;Float;False;Property;_Reflection;Reflection;7;0;1;0.001;1;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-281.3229,88.15987;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Created/waterfall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;False;True;False;False;False;False;False;False;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;2;SrcAlpha;OneMinusSrcAlpha;Add;Add;1;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;61;0;2;0
WireConnection;61;1;62;0
WireConnection;63;1;61;0
WireConnection;14;0;65;0
WireConnection;14;1;63;0
WireConnection;16;1;14;0
WireConnection;66;0;16;0
WireConnection;66;1;68;0
WireConnection;70;0;66;0
WireConnection;70;1;69;0
WireConnection;73;1;14;0
WireConnection;0;0;66;0
WireConnection;0;1;73;0
WireConnection;0;2;70;0
WireConnection;0;3;16;1
WireConnection;0;4;33;0
WireConnection;0;9;27;0
ASEEND*/
//CHKSM=3727ACD5C2F8B5120C19B6A8279FE48FA046182E