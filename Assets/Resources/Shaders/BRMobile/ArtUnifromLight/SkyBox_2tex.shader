Shader "BRMobile/ArtUnifromLight/SkyBox_2tex"
{
	Properties
	{
		_Color ("Sky", color) = (1.0, 1.0, 1.0, 0.0)
		_Cloud1Color ("Cloud1", color) = (1.0, 1.0, 1.0, 1.0)
		_Cloud2Color ("Cloud2", color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Sky Texture", 2D) = "black" { }
		_CloudTex ("Cloud Texture", 2D) = "black" { }
		_Speed ("(x)cloud1_speed (y)cloud2_speed (z)cloud_top_tile (w)cloud_top_offset", Vector) = (0.01, 0.005, 0.0001, 0.5)
		_TopLinear ("Top(x)start_result (y)-end_result (z)start (w)end", Vector) = (0.0, 0.0, 0.0, 0.9)
		_SunParam ("(x)SunMin (y)SunMax (z)SunAlpha (w)Max", Vector) = (0.01, 0.04, 0.4, 0.01)
		_FogParam ("Fog(x)start_result (y)-end_result (z)start (w)end", Vector) = (0.0, 0.25, 0.0, 0.0)
		_SpeedVerti ("(x)cloud1_SpeedVertical (y)cloud2_SpeedVertical (z) (w)", Vector) = (0.0, 0.0, 0.0, 0.0)
		[HideInInspector]__NULL_SpeedMin_SpeedMax_CloudTile ("", Vector) = (-0.03, 0.03, 5.0, 0.0)
	}
	SubShader
	{
		Tags { "RenderType" = "Background" "Queue" = "AlphaTest" "ShaderType" = "BlinnPhongRefFresVertLit" "ShaderNameTag" = "BPRefSkyBox" }
		LOD 500

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile __ OPTIMIZE

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "BRMobileUniformLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float4 cloudUV : TEXCOORD1;
				#ifndef OPTIMIZE
					float3 sun_pos : TEXCOORD2;
				#endif
				#ifdef FOG_LINEAR
					half3 fog_factor : TEXCOORD3;//x-top_factor , y-fog Factor , z-sun Factor
				#else
					half fog_factor : TEXCOORD3;
				#endif
			};

			float4 _Speed, _CloudTex_ST, _TopLinear;
			sampler2D _MainTex, _CloudTex;
			half4 _Color, _Cloud2Color, _Cloud1Color, _FogParam, _SpeedVerti;
			#ifndef OPTIMIZE
				half4 _SunParam;
			#endif
			v2f vert(appdata v)
			{
				v2f o;
				float4 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0f));
				o.pos = mul(UNITY_MATRIX_VP, worldPos);
				float3 worldPosN = normalize(worldPos);

				#if UNITY_REVERSED_Z
					o.pos.z = o.pos.w * 0.000001h;
				#else
					o.pos.z = o.pos.w * 0.999999h;
				#endif

				#ifdef FOG_LINEAR
					o.fog_factor.y = SkyBoxFactor(worldPosN.y, _FogParam.x, _FogParam.y);
					#ifdef FOG_LINEAR
						o.fog_factor.z = 0.0h;
						if (custom_sunIntencity > 0.0h)
						{
							half3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
							half3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
							o.fog_factor.z = SkyBoxSunFactor(lightDir, viewDir);
						}
					#endif
				#endif

				o.uv.xy = v.texcoord.xy;
				o.uv.zw = worldPos.xz * _Speed.z + _Speed.w;
				o.cloudUV.yw = v.texcoord.yy + _SpeedVerti.xy * _Time.y;
				o.cloudUV.xz = v.texcoord.xx * _CloudTex_ST.xy + _CloudTex_ST.zw + _Speed.xy * _Time.y;
				//data
				#ifndef OPTIMIZE
					o.sun_pos.xyz = _WorldSpaceLightPos0.xyz - worldPosN;
				#endif
				//top_factor
				o.fog_factor.x = saturate(worldPosN.y * _TopLinear.x + _TopLinear.y);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				//cloud1
				half cloud1_factor = tex2D(_CloudTex, i.cloudUV.xy).r * _Cloud1Color.a;
				//top
				half cloud3_factor = tex2D(_CloudTex, i.uv.zw).b * _Cloud1Color.a;
				cloud1_factor = lerp(cloud3_factor, cloud1_factor, i.fog_factor.x);
				//result
				half3 sky = tex2D(_MainTex, i.uv.xy).rgb * _Color.rgb;
				half3 result = sky;
				#ifndef OPTIMIZE
					half cloud2_factor = tex2D(_CloudTex, i.cloudUV.zw).g * _Cloud2Color.a;
					result = lerp(sky, _Cloud2Color, cloud2_factor);
					half cloud = max(cloud1_factor, cloud2_factor);
				#endif
				result = lerp(result, _Cloud1Color, cloud1_factor);
				
				//ACES
				result = ACES(result);

				//fog
				#ifdef FOG_LINEAR
					result = SkyBoxFog(result, i.fog_factor.y);
					#ifndef OPTIMIZE
							result = SkyBoxSunLightFog(result, i.fog_factor.z, cloud);
					#endif
				#endif
				
				//sun
				#ifndef OPTIMIZE
					half sun_pos = (1.0h - smoothstep(_SunParam.x, _SunParam.y, dot(i.sun_pos, i.sun_pos)));
					result += sun_pos * _LightColor0.rgb * saturate(cloud * _SunParam.z);
				#endif
				
				return half4(result, 1.0h);
			}
			ENDCG

		}
	}
	SubShader
	{
		Tags { "RenderType" = "Background" "Queue" = "AlphaTest" "ShaderType" = "BlinnPhongRefFresVertLit" "ShaderNameTag" = "BPRefSkyBox" }

		LOD 300

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile __ OPTIMIZE

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "BRMobileUniformLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float4 cloudUV : TEXCOORD1;
				#ifndef OPTIMIZE
					float3 sun_pos : TEXCOORD2;
				#endif
				#ifdef FOG_LINEAR
					half3 fog_factor : TEXCOORD3;//x-top_factor , y-fog Factor , z-sun Factor
				#else
					half fog_factor : TEXCOORD3;
				#endif
			};

			float4 _Speed, _CloudTex_ST, _TopLinear;
			sampler2D _MainTex, _CloudTex;
			half4 _Color, _Cloud1Color, _FogParam, _SpeedVerti;
			#ifdef OPTIMIZE
				#define custom_ACES_Offset 1.2h
			#else
				half4 _SunParam;
			#endif
			v2f vert(appdata v)
			{
				v2f o;
				float4 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0f));
				o.pos = mul(UNITY_MATRIX_VP, worldPos);
				float3 worldPosN = normalize(worldPos);

				#if UNITY_REVERSED_Z
					o.pos.z = o.pos.w * 0.000001h;
				#else
					o.pos.z = o.pos.w * 0.999999h;
				#endif

				#ifdef FOG_LINEAR
					o.fog_factor.y = SkyBoxFactor(worldPosN.y, _FogParam.x, _FogParam.y);
					#ifdef FOG_LINEAR
						o.fog_factor.z = 0.0h;
						if (custom_sunIntencity > 0.0h)
						{
							half3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
							half3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
							o.fog_factor.z = SkyBoxSunFactor(lightDir, viewDir);
						}
					#endif
				#endif

				o.uv.xy = v.uv.xy;
				o.uv.zw = worldPos.xz * _Speed.z + _Speed.w;
				o.cloudUV.yw = v.uv.yy + _SpeedVerti.xy * _Time.y;
				o.cloudUV.xz = v.uv.xx * _CloudTex_ST.xy + _CloudTex_ST.zw + (_Speed.xy * _Time.y);
				//data
				#ifndef OPTIMIZE
					o.sun_pos.xyz = _WorldSpaceLightPos0.xyz - worldPosN;
				#endif
				//top_factor
				o.fog_factor.x = saturate(worldPosN.y * _TopLinear.x + _TopLinear.y);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				//cloud1
				half cloud1_factor = tex2D(_CloudTex, i.cloudUV.xy).r * _Cloud1Color.a;
				//top
				half cloud3_factor = tex2D(_CloudTex, i.uv.zw).b * _Cloud1Color.a;
				cloud1_factor = lerp(cloud3_factor, cloud1_factor, i.fog_factor.x);
				//result
				half3 sky = tex2D(_MainTex, i.uv.xy).rgb * _Color.rgb;
				half3 result = lerp(sky, _Cloud1Color, cloud1_factor);
				//ACES
				#ifdef OPTIMIZE
					result *= custom_ACES_Offset;
				#else
					result = ACES(result);
				#endif
				//fog
				#ifdef FOG_LINEAR
					result = SkyBoxFog(result, i.fog_factor.y);
					#ifndef OPTIMIZE
						result = SkyBoxSunLightFog(result, i.fog_factor.z, cloud1_factor);
					#endif
				#endif
				//sun
				#ifndef OPTIMIZE
					half sun_pos = (1.0h - smoothstep(_SunParam.x, _SunParam.y, dot(i.sun_pos, i.sun_pos)));
					result += sun_pos * _LightColor0.rgb * saturate(cloud1_factor * _SunParam.z);
				#endif

				return half4(result, 1.0h);
			}
			ENDCG

		}
	}
	SubShader
	{
		Tags { "RenderType" = "Background" "Queue" = "AlphaTest" "ShaderType" = "BlinnPhongRefFresVertLit" "ShaderNameTag" = "BPRefSkyBox" }
		LOD 200

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "BRMobileUniformLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 cloudUV : TEXCOORD1;
				#ifdef FOG_LINEAR
					fixed fog_factor : TEXCOORD2;
				#endif
			};

			float4 _Speed, _CloudTex_ST;
			sampler2D _MainTex, _CloudTex;
			half4 _Color, _Cloud1Color, _FogParam;


			v2f vert(appdata v)
			{
				v2f o;
				float4 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
				o.pos = mul(UNITY_MATRIX_VP, worldPos);

				#if UNITY_REVERSED_Z
					o.pos.z = o.pos.w * 0.000001f;
				#else
					o.pos.z = o.pos.w * 0.999999f;
				#endif

				#ifdef FOG_LINEAR
					fixed height = normalize(worldPos).y;
					o.fog_factor = SkyBoxFactor(height, _FogParam.x, _FogParam.y);
				#endif

				o.uv.xy = v.uv.xy;
				o.cloudUV.yw = v.uv.yy;
				o.cloudUV.xz = v.uv.xx * _CloudTex_ST.xy + _CloudTex_ST.zw;

				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 sky = tex2D(_MainTex, i.uv.xy).rgb * _Color.rgb;
				fixed cloud1 = tex2D(_CloudTex, i.cloudUV.xy).r;
				fixed3 result = lerp(sky, _Cloud1Color, cloud1 * _Cloud1Color.a);

				//fog
				#ifdef FOG_LINEAR
					result = SkyBoxFog(result, i.fog_factor);
				#endif

				return fixed4(result, 1.0h);
			}
			ENDCG

		}
	}
	CustomEditor "FF_TA.SkyBox_2texGui"
}

