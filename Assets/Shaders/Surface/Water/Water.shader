// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Created/water"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Albedo("Albedo", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_Color("Color", Color) = (0.5294118,0.4941176,0.8392157,1)
		_Normal("Normal", 2D) = "white" {}
		_Force("Force", Range( 0 , 1)) = 0
		_Speed("Speed", Range( 0 , 1)) = 1
		_MaxTilling("MaxTilling", Range( 0 , 50)) = 0
		_MinTilling("MinTilling", Range( 0 , 50)) = 0
		_Reflection("Reflection", Range( 0.001 , 1)) = 1
		_speedWave("speedWave", Float) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
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
		uniform float _MinTilling;
		uniform float _MaxTilling;
		uniform float _Speed;
		uniform float _Force;
		uniform float _speedWave;
		uniform sampler2D _Albedo;
		uniform float4 _Color;
		uniform float _Reflection;
		uniform float _Opacity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime2 = _Time.y * _Speed;
			float temp_output_8_0 = ( cos( mulTime2 ) * _Force );
			float2 temp_cast_0 = (( _MinTilling + ( ( _MaxTilling - _MinTilling ) * temp_output_8_0 ) )).xx;
			float2 temp_cast_1 = (temp_output_8_0).xx;
			o.texcoord_0.xy = v.texcoord.xy * temp_cast_0 + temp_cast_1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime51 = _Time.y * _speedWave;
			float temp_output_53_0 = sin( mulTime51 );
			float clampResult54 = clamp( temp_output_53_0 , 0.3 , 0.7 );
			float4 lerpResult46 = lerp( tex2D( _Normal, i.texcoord_0 ) , float4(0.427451,0.353,1,0) , clampResult54);
			o.Normal = lerpResult46.rgb;
			o.Albedo = ( ( tex2D( _Albedo, i.texcoord_0 ).r * _Color ) * temp_output_53_0 ).rgb;
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
7;29;1352;692;1376.745;199.2803;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;1;-2417.302,91.66717;Float;False;Property;_Speed;Speed;5;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;2;-2081.302,96.66717;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.CosOpNode;5;-1880.993,97.14767;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;4;-2198.992,-342.8524;Float;False;Property;_MinTilling;MinTilling;7;0;0;0;50;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;3;-2272.115,-132.4132;Float;False;Property;_MaxTilling;MaxTilling;6;0;0;0;50;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;6;-2031.719,248.0393;Float;False;Property;_Force;Force;4;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;-1915.362,-126.2492;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1708.72,97.03927;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1719.362,-73.24918;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-1575.362,-122.2492;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;52;-1044.316,-344.3118;Float;False;Property;_speedWave;speedWave;9;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1501.173,47.46427;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;51;-853.47,-340.6487;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;16;-1119.466,22.48747;Float;True;Property;_Albedo;Albedo;0;0;Assets/Art/Shaders/Texture/Water.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;17;-1069.588,-156.181;Float;False;Property;_Color;Color;2;0;0.5294118,0.4941176,0.8392157,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SinOpNode;53;-777.3162,-191.3118;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;32;-1114.07,225.0242;Float;True;Property;_Normal;Normal;3;0;Assets/Art/Shaders/Texture/WaterNormal.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;40;-1035.575,415.8412;Float;False;Constant;_Base;Base;10;0;0.427451,0.353,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;54;-717.7451,152.7197;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.3;False;2;FLOAT;0.7;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-740.9249,48.83514;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;46;-706.4314,271.5314;Float;False;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;27;-1080.111,666.2589;Float;False;Property;_Opacity;Opacity;1;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-622.1852,-69.45701;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;33;-1084.764,583.0727;Float;False;Property;_Reflection;Reflection;8;0;1;0.001;1;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-281.3229,88.15987;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Created/water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;False;True;False;False;False;False;False;False;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;2;SrcAlpha;OneMinusSrcAlpha;Add;Add;1;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;5;0;2;0
WireConnection;7;0;3;0
WireConnection;7;1;4;0
WireConnection;8;0;5;0
WireConnection;8;1;6;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;10;0;4;0
WireConnection;10;1;9;0
WireConnection;14;0;10;0
WireConnection;14;1;8;0
WireConnection;51;0;52;0
WireConnection;16;1;14;0
WireConnection;53;0;51;0
WireConnection;32;1;14;0
WireConnection;54;0;53;0
WireConnection;19;0;16;1
WireConnection;19;1;17;0
WireConnection;46;0;32;0
WireConnection;46;1;40;0
WireConnection;46;2;54;0
WireConnection;38;0;19;0
WireConnection;38;1;53;0
WireConnection;0;0;38;0
WireConnection;0;1;46;0
WireConnection;0;4;33;0
WireConnection;0;9;27;0
ASEEND*/
//CHKSM=187AE041290858CD3C11D096808BFF950EC0013E