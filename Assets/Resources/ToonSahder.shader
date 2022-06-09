Shader "Unlit/PracticeShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,0)
        _Gloss("Gloss", Float) = 1
        //_MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL; 
            };

            struct VertexOutput
            {
                float4 clipSpacePos : SV_POSITION;
                float4 normal : TEXCOORD0; 
                float3 worldPos : TEXCORD1;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;

            float4 _Color; 
            float _Gloss; 

            float Posterize(float steps, float value) {
                return (value * steps) / steps; 
            }


            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
                o.clipSpacePos = UnityObjectToClipPos(v.vertex);;
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float4 frag(VertexOutput o) : SV_Target
            {
                float3 normal = normalize(o.normal); //interpolated

                //Lighting

                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 lightColor = _LightColor0.rgb;

                //Direct Light
                float3 lightFalloff = saturate ( dot (lightDir, normal)); 
                lightFalloff = Posterize(16, lightFalloff);
                float3 directDiffuseLight = lightColor * lightFalloff;
                
                //AmbientLight
                float3 ambientLight = float3 (0.2, 0.35, 0.4);

                //Direct specular light
                
                float3 CampPos = _WorldSpaceCameraPos; 
                float3 fragToCam = CampPos - o.worldPos;
                float3 viewDir = normalize(fragToCam);

                float3 viewReflect = reflect(-viewDir, normal);
                float specularFalloff = saturate ( dot (viewReflect, lightDir));
                specularFalloff = Posterize(4, specularFalloff);
                specularFalloff = pow(specularFalloff, _Gloss);
                float3 directSpecular = specularFalloff * lightColor;

                
                // Composite 
                float3 diffuseLight = ambientLight + directDiffuseLight;
                float3 finalDiffuseColor = diffuseLight * _Color.rgb + directSpecular;

                return float4 (finalDiffuseColor, 0);
            }
            ENDCG
        }
    }
}
