 //Kaan Yamanyar,Levent Seckin
 Shader "Custom/SpiralEffect"
 {
     Properties
     {
         [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
         _Color("Tint", Color) = (1, 1, 1, 1)
         _ColorFill("ColorFill", Range(0, 100)) = 5
         _Location("Location", Range(0, 1)) = 0
         _LineCount("LineCount", Range(1, 20)) = 5
         _LineWidth("LineWidth", Range(0, 20)) = 5

         [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
     }
 
         SubShader
     {
         Tags
     {
         "Queue" = "Transparent"
         "IgnoreProjector" = "True"
         "RenderType" = "Transparent"
         "PreviewType" = "Plane"
         "CanUseSpriteAtlas" = "True"
     }
 
         Cull Off
         Lighting Off
         ZWrite Off
         Blend One OneMinusSrcAlpha
 
         Pass
     {
         CGPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #pragma multi_compile _ PIXELSNAP_ON
 #include "UnityCG.cginc"
 
     struct appdata_t
     {
         float4 vertex   : POSITION;
         float4 color    : COLOR;
         float2 texcoord : TEXCOORD0;
     };
 
     struct v2f
     {
         float4 vertex   : SV_POSITION;
         fixed4 color : COLOR;
         float2 texcoord  : TEXCOORD0;
     };
 
     fixed4 _Color;
 
     v2f vert(appdata_t IN)
     {
         v2f OUT;
         OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
         OUT.texcoord = IN.texcoord;
         OUT.color = IN.color;
 #ifdef PIXELSNAP_ON
         OUT.vertex = UnityPixelSnap(OUT.vertex);
 #endif
         return OUT;
     }
 
     sampler2D _MainTex;
     sampler2D _AlphaTex;
     float _AlphaSplitEnabled;
     float _Location;
     float _LineWidth;
 	 float _ColorFill;
 	 int _LineCount;
     fixed4 SampleSpriteTexture(float2 uv)
     {
         fixed4 color = tex2D(_MainTex, uv);
 
// #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
//         if (_AlphaSplitEnabled)
//             color.a = tex2D(_AlphaTex, uv).r;
// #endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

		if(color.a < 0.2)
		 	return color;

		 _LineWidth *= 0.01;
		 uv.y += _Location;
		 float lineCount = _LineCount;
		 float heightPerLine = 1 / lineCount;
		 int currentLine = uv.y / heightPerLine;
		 float currentPosX = (uv.x / 1) * heightPerLine;
		 float currentPosY = uv.y - currentPosX;

         float minimum = (heightPerLine * currentLine) - (_LineWidth * 0.5);
         float maximum = (heightPerLine * currentLine) + (_LineWidth * 0.5);
		 if(currentPosY > minimum && currentPosY < maximum)
         {
        	float diffMin = abs(currentPosY - minimum);
        	float diffMax = abs(currentPosY - maximum);
        	float diff = (diffMin < diffMax) ? diffMin : diffMax;
        	color.rgb += _Color.rgb * diff * _ColorFill;
         }    		 	    
         return color;
     }
 
     fixed4 frag(v2f IN) : SV_Target
     {
         fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
         c.rgb *= c.a;
 
     return c;
     }
         ENDCG
     }

     }
 }