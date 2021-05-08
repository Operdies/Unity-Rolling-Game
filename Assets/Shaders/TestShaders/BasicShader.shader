Shader "TestShaders/HoleShader"
{
    Properties
    {
        _MainTex ("Hole Texture", 2D) = "white" {}
        _Color ("MainColor", Color) = (1,1,1,1)
        _Transparency ("Transparency", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"  }
        LOD 100

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color; 
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                
                fixed4 holes = tex2D(_MainTex, i.uv);                                       
                holes.a = 1 - (length(holes.rgb) / 3);
                return holes * _Color;
            }
            ENDCG
        }
    }
}
