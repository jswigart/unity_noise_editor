// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/GUITextureClip_ChannelControl"
{
    Properties 
    { 
        _MainTex ("Texture", Any) = "white" {} 
    }
 
    SubShader 
    {
 
        Tags { "ForceSupported" = "True" }
 
        Lighting Off 
        Blend SrcAlpha OneMinusSrcAlpha 
        Cull Off 
        ZWrite Off 
        ZTest Always
 
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #pragma shader_feature SCALE_CHANNEL
 
 
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                half4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float2 clipUV : TEXCOORD1;
            };
 
            sampler2D _MainTex;
            sampler2D _GUIClipTexture;
 
            uniform float4 _MainTex_ST;
            uniform fixed4 _Color;
            uniform float4x4 unity_GUIClipTextureMatrix;
 
            // Ints pointing to the channel to represent: 0-black - 1-red - 2-green - 3-blue - 4-alpha - 5-white
            uniform int shuffleRed = 1, shuffleGreen = 2, shuffleBlue = 3, shuffleAlpha = 4;
 
            // Scale values for the individual channels
#if SCALE_CHANNEL
            uniform float scaleRed = 1, scaleGreen = 1, scaleBlue = 1, scaleAlpha = 1;
#else
            uniform float scaleColor = 1;
#endif
 
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
 
                float4 eyePos = mul(UNITY_MATRIX_MV, v.vertex);
                o.clipUV = mul(unity_GUIClipTextureMatrix, eyePos);
 
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }
 
            half shuffleChannel (half4 srcCol, int shuffle)
            {
                if (shuffle <= 0)
                    return 0;
                if (shuffle == 1)
                    return srcCol.r;
                if (shuffle == 2)
                    return srcCol.g;
                if (shuffle == 3)
                    return srcCol.b;
                if (shuffle == 4)
                    return srcCol.a;
                return 1;
            }
 
            half4 frag (v2f i) : SV_Target
            {
                half4 rawCol = tex2D(_MainTex, i.texcoord);
 
#if SCALE_CHANNEL
                half4 shuffledCol = half4 ( shuffleChannel (rawCol, shuffleRed) * scaleRed, 
                                    shuffleChannel (rawCol, shuffleGreen) * scaleGreen, 
                                    shuffleChannel (rawCol, shuffleBlue) * scaleBlue, 
                                    shuffleChannel (rawCol, shuffleAlpha) * scaleAlpha);
#else
                half4 shuffledCol = half4 ( shuffleChannel (rawCol, shuffleRed) * scaleColor, 
                                            shuffleChannel (rawCol, shuffleGreen) * scaleColor, 
                                            shuffleChannel (rawCol, shuffleBlue) * scaleColor, 
                                            shuffleChannel (rawCol, shuffleAlpha) * scaleColor);
#endif
                 
                half4 col = shuffledCol * i.color;
                col.a *= 2.0f * tex2D(_GUIClipTexture, i.clipUV).a;
                return col;
            }
            ENDCG
        }
    }
}