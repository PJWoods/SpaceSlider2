 //Kaan Yamanyar,Levent Seckin
 Shader "Custom/BaseBlockShader"
 {
     Properties
     {
         [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
         _Color("Tint", Color) = (1, 1, 1, 1)
         _ColorFill("ColorFill", Range(0, 5)) = 0.5
         _ShineLocation("ShineLocation", Range(-1, 2)) = 0
         _ShineWidth("ShineWidth", Range(0, 1)) = 0
         _EffectWidth("EffectWidth", Range(0, 0.5)) = 0
     
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
     float _ShineLocation;
     float _ShineWidth;
 	 float _EffectWidth;
 	 float _ColorFill;
     fixed4 SampleSpriteTexture(float2 uv)
     {
         fixed4 color = tex2D(_MainTex, uv);
 
 #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
         if (_AlphaSplitEnabled)
             color.a = tex2D(_AlphaTex, uv).r;
 #endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

		 float center = 0.5;
		 float currentX = abs(center - uv.x); float currentY = abs(center - uv.y);
		 if(currentX < center - _EffectWidth && currentY < center - _EffectWidth)
		 	return color;

         float lowLevel = _ShineLocation - _ShineWidth;
         float highLevel = _ShineLocation + _ShineWidth;
         float currentDistanceProjection = (uv.x + uv.y) / 2;
         if (currentDistanceProjection > lowLevel && currentDistanceProjection < highLevel) 
         {
             float whitePower = 1 - (abs(currentDistanceProjection - _ShineLocation) / _ShineWidth);
             float strength = (whitePower * (1 + (currentX + currentY))) * _ColorFill;
             color.rgb += _Color.rgb * color.a * strength;
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