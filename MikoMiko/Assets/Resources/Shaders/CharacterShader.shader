// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33659,y:32750,varname:node_3138,prsc:2|custl-8860-OUT,olwid-8288-OUT,olcol-2613-RGB;n:type:ShaderForge.SFN_LightVector,id:5939,x:31916,y:32917,varname:node_5939,prsc:2;n:type:ShaderForge.SFN_Dot,id:4527,x:32225,y:32968,varname:node_4527,prsc:2,dt:0|A-5939-OUT,B-4845-OUT;n:type:ShaderForge.SFN_NormalVector,id:4845,x:31959,y:33100,prsc:2,pt:True;n:type:ShaderForge.SFN_Step,id:9451,x:32565,y:33073,varname:node_9451,prsc:2|A-1966-OUT,B-6193-OUT;n:type:ShaderForge.SFN_Slider,id:1966,x:32128,y:33362,ptovrint:False,ptlb:node_1966,ptin:_node_1966,varname:node_1966,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1695652,max:1;n:type:ShaderForge.SFN_Color,id:1809,x:32508,y:33285,ptovrint:False,ptlb:node_1809,ptin:_node_1809,varname:node_1809,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.9256335,c3:0.8537736,c4:1;n:type:ShaderForge.SFN_Multiply,id:1328,x:32893,y:33095,varname:node_1328,prsc:2|A-9451-OUT,B-1809-RGB;n:type:ShaderForge.SFN_RemapRange,id:6773,x:32749,y:33331,varname:node_6773,prsc:2,frmn:0,frmx:1,tomn:1,tomx:0|IN-9451-OUT;n:type:ShaderForge.SFN_Color,id:6994,x:32674,y:33599,ptovrint:False,ptlb:node_6994,ptin:_node_6994,varname:node_6994,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.02055892,c2:0.1000413,c3:0.2075472,c4:1;n:type:ShaderForge.SFN_Multiply,id:8753,x:32969,y:33396,varname:node_8753,prsc:2|A-6773-OUT,B-6994-RGB;n:type:ShaderForge.SFN_Add,id:5247,x:33142,y:32962,varname:node_5247,prsc:2|A-1328-OUT,B-8753-OUT;n:type:ShaderForge.SFN_Tex2d,id:7085,x:32712,y:32739,ptovrint:False,ptlb:_Tex,ptin:__Tex,varname:node_7085,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8860,x:33372,y:32766,varname:node_8860,prsc:2|A-7085-RGB,B-5247-OUT,C-9105-RGB;n:type:ShaderForge.SFN_LightColor,id:9105,x:33204,y:33165,varname:node_9105,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:385,x:32196,y:32623,varname:node_385,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6193,x:32390,y:32878,varname:node_6193,prsc:2|A-4527-OUT,B-385-OUT;n:type:ShaderForge.SFN_Slider,id:8288,x:33287,y:33331,ptovrint:False,ptlb:node_8288,ptin:_node_8288,varname:node_8288,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.04395115,max:1;n:type:ShaderForge.SFN_Color,id:2613,x:33385,y:33093,ptovrint:False,ptlb:node_2613,ptin:_node_2613,varname:node_2613,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2735849,c2:0.01935742,c3:0.01935742,c4:0.5686275;proporder:1966-1809-6994-7085-8288-2613;pass:END;sub:END;*/

Shader "Shader Forge/testshader" {
    Properties {
        _node_1966 ("node_1966", Range(0, 1)) = 0.1695652
        _node_1809 ("node_1809", Color) = (1,0.9256335,0.8537736,1)
        _node_6994 ("node_6994", Color) = (0.02055892,0.1000413,0.2075472,1)
        __Tex ("_Tex", 2D) = "white" {}
        _node_8288 ("node_8288", Range(0, 1)) = 0.04395115
        _node_2613 ("node_2613", Color) = (0.2735849,0.01935742,0.01935742,0.5686275)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma target 3.0
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _node_8288)
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_2613)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                float _node_8288_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_8288 );
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_node_8288_var,1) );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float4 _node_2613_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_2613 );
                return fixed4(_node_2613_var.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma target 3.0
            uniform sampler2D __Tex; uniform float4 __Tex_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _node_1966)
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_1809)
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_6994)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 __Tex_var = tex2D(__Tex,TRANSFORM_TEX(i.uv0, __Tex));
                float _node_1966_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_1966 );
                float node_9451 = step(_node_1966_var,(dot(lightDirection,normalDirection)*attenuation));
                float4 _node_1809_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_1809 );
                float4 _node_6994_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_6994 );
                float3 finalColor = (__Tex_var.rgb*((node_9451*_node_1809_var.rgb)+((node_9451*-1.0+1.0)*_node_6994_var.rgb))*_LightColor0.rgb);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma target 3.0
            uniform sampler2D __Tex; uniform float4 __Tex_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _node_1966)
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_1809)
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_6994)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 __Tex_var = tex2D(__Tex,TRANSFORM_TEX(i.uv0, __Tex));
                float _node_1966_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_1966 );
                float node_9451 = step(_node_1966_var,(dot(lightDirection,normalDirection)*attenuation));
                float4 _node_1809_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_1809 );
                float4 _node_6994_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_6994 );
                float3 finalColor = (__Tex_var.rgb*((node_9451*_node_1809_var.rgb)+((node_9451*-1.0+1.0)*_node_6994_var.rgb))*_LightColor0.rgb);
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
