// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Created/Toon/Celshalding Billboard"
{
	Properties
	{
		_Cutoffangle2("Cut off angle 2", Range( -1 , 1)) = 0.203293
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Cutoffangle1("Cut off angle 1", Range( -1 , 1)) = -0.124017
		_Albedo("Albedo", 2D) = "white" {}
		_Cutoffangle("Cut off angle", Range( -1 , 1)) = -0.3688092
		_ColorAlbedo("Color Albedo", Color) = (1,1,1,0)
		[Header(CelShalding)]
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _ColorAlbedo;
		uniform float _Cutoffangle;
		uniform float _Cutoffangle1;
		uniform float _Cutoffangle2;
		uniform float _Cutoff = 0.5;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_objectlightDir = normalize( ObjSpaceLightDir( ase_vertex4Pos ) );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float dotResult12_g5 = dot( -ase_objectlightDir , ase_vertexNormal );
			float smoothstepResult14_g5 = smoothstep( ( _Cutoffangle + 0.01 ) , ( _Cutoffangle - 0.01 ) , dotResult12_g5);
			float dotResult36_g5 = dot( -ase_objectlightDir , ase_vertexNormal );
			float smoothstepResult38_g5 = smoothstep( ( _Cutoffangle1 + 0.01 ) , ( _Cutoffangle1 - 0.01 ) , dotResult36_g5);
			float dotResult52_g5 = dot( -ase_objectlightDir , ase_vertexNormal );
			float smoothstepResult54_g5 = smoothstep( ( _Cutoffangle2 + 0.01 ) , ( _Cutoffangle2 - 0.01 ) , dotResult52_g5);
			float4 blendOpSrc316 = ( tex2D( _Albedo, uv_Albedo ) + _ColorAlbedo );
			float4 blendOpDest316 = ( ( ( _LightColor0 * smoothstepResult14_g5 ) + ( _LightColor0 * ( 1.0 - smoothstepResult14_g5 ) * 0.3516801 ) ) * ( ( _LightColor0 * smoothstepResult38_g5 ) + ( _LightColor0 * ( 1.0 - smoothstepResult38_g5 ) * 0.5404183 ) ) * ( ( _LightColor0 * smoothstepResult54_g5 ) + ( _LightColor0 * ( 1.0 - smoothstepResult54_g5 ) * 0.4295608 ) ) );
			c.rgb = ( ( saturate( ( blendOpSrc316 * blendOpDest316 ) )) / 1.5 ).rgb;
			c.a = 1;
			clip( tex2D( _Albedo, uv_Albedo ).a - _Cutoff );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				float3 worldNormal : TEXCOORD1;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
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
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
7;29;1352;692;1181.791;511.6704;3.155;True;False
Node;AmplifyShaderEditor.SamplerNode;306;-861.4153,140.1725;Float;True;Property;_Albedo;Albedo;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;308;-814.0145,323.9727;Float;False;Property;_ColorAlbedo;Color Albedo;2;0;1,1,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FunctionNode;324;-359.2233,398.5426;Float;False;CelShalding;3;;5;0;1;COLOR
Node;AmplifyShaderEditor.CommentaryNode;318;70.96758,96.1018;Float;False;445.5153;236.875;ASE dobles all your emission color automatically;2;317;315;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;326;-490.6821,180.8388;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.CommentaryNode;319;-230.7457,104.6132;Float;False;246.2002;151.3;Screen;1;316;;1,1,1,1;0;0
Node;AmplifyShaderEditor.BlendOpsNode;316;-185.1459,147.0124;Float;False;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;315;102.1544,232.367;Float;False;Constant;_DivEmission;DivEmission;3;0;1.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;317;370.4011,153.4672;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;325;265.2521,396.8006;Float;True;Property;_Alpha;Alpha;1;0;None;True;0;False;white;Auto;False;Instance;306;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;746.3,111.6001;Float;False;True;6;Float;ASEMaterialInspector;0;0;CustomLighting;Created/Toon/Celshalding Billboard;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Custom;0.5;True;True;0;True;Transparent;AlphaTest;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;5;10;25;True;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0.01;0,0,0,1;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;14;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;326;0;306;0
WireConnection;326;1;308;0
WireConnection;316;0;326;0
WireConnection;316;1;324;0
WireConnection;317;0;316;0
WireConnection;317;1;315;0
WireConnection;0;2;317;0
WireConnection;0;10;325;4
ASEEND*/
//CHKSM=9ED5299818E1B426B46A16ACE9900332C361FC0E