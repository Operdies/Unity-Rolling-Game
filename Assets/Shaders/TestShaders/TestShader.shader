Shader "Unlit/TestShader"
{
    Properties
    {
        _MainTex ("MyCoolTexture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1,1,1,1)
        _Transparency("Transparency", Range(0.0, 0.5)) = 0.25
        _CutoutThresh("Cutout Threshold", Range(0.0, 0.5)) = .2
        _PulsateStrength("Pulsate Strength", Range(0.0, 3.0)) = 1.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"  }
        LOD 100

        // Avoid writing to the depth buffer (for transparent objects)
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag            

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TintColor;
            float4 _OutlineColor;
            float _Transparency;
            float _CutoutThresh;

            float _PulsateStrength;

            v2f vert (appdata v)
            {                   
                v.vertex *= (abs(_SinTime.w) + 1) * _PulsateStrength;
                v2f o;                

                o.vertex = UnityObjectToClipPos(v.vertex);                
                o.uv =  TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i)  :  SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
                col.a = 1 - _Transparency;
                clip(col.b - _CutoutThresh);
                return col;
            }
            ENDCG
        }      
    }
}
