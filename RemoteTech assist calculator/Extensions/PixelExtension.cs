using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RemoteTech_assist_calculator.Extension
{
    public class PixelExtention
    {

       public static void ClearTexture(out Texture2D texture,Texture2D textureO)
       {
           texture = textureO;

         for (int x = 0; x < texture.width; x++)
                for (int y = 0; y < texture.height; y++)
                    texture.SetPixel(x, y, Color.clear);

           texture.Apply();
       }

       public static void DrawCircle(out Texture2D texture, Texture2D textureO, int CenterX, int CenterY, int radius, Color color)
        {
            texture = textureO;

                for (int i = 0; i <= 360; i++)
                {
                    float degInRad = i * Mathf.Deg2Rad;
                    texture.SetPixel((int)(CenterX + Math.Cos(degInRad) * radius), (int)(CenterY  + Math.Sin(degInRad) * radius), color);
                    texture.SetPixel((int)(CenterX + Math.Cos(degInRad) * (radius))+1, (int)(CenterY  + Math.Sin(degInRad) * radius), color);
                    texture.SetPixel((int)(CenterX + Math.Cos(degInRad) * (radius)), (int)(CenterY + Math.Sin(degInRad) * radius)+1, color);
                }

             texture.Apply();
        }

       public static void DrawLine(out Texture2D texture, Texture2D textureO, int startX, int startY, int endX, int endY,Color color)
       {
           texture = textureO;

           float x,y;
           float dy = endY-startY;    
           float dx = endX-startY;    
           float m = dy/dx; 
           float dy_inc = -1;    
 
             if(  dy < 0 ) 
               dy = 1;    
 
         float dx_inc = 1;    
             if(  dx < 0 ) 
                  dx = -1; 
 
             if( Mathf.Abs(dy) > Mathf.Abs(dx) )
             {
                  for( y = endY; y < startY; y += dy_inc )
                  {                
                   x = startX + ( y - startY ) * m;
                   texture.SetPixel((int)(x), (int)(y), color);       
                   }
            }
            else
             {
                  for( x = startX; x < endX; x +=  dx_inc  ) 
                  {
                     y = startY + ( x - startX ) * m;
                     texture.SetPixel((int)(x), (int)(y), color);           
                   }   
            }
            texture.Apply();
        }

         public static void LoadImage(out Texture2D texture, String fileName,int size)
       {
           fileName = fileName.Split('.')[0];
           String path = "RemoteTech2_Assist/Textures/" + fileName;
           
           texture = GameDatabase.Instance.GetTexture(path, false);
           if (texture == null)
           {
               texture = new Texture2D(size, size);
               texture.SetPixels32(Enumerable.Repeat((Color32)Color.white, size * size).ToArray());
               texture.Apply();
           }
       }

         public static void BlendTexures(out Texture2D onTexture, Texture2D onTextureO, Texture2D addTexture,int cx, int cy)
         {
             onTexture = onTextureO;

             for(int x=1;x<=addTexture.width;x++)
                 for (int y = 1; y <= addTexture.height; y++)
                    onTexture.SetPixel(cx + x, cy + y, addTexture.GetPixel(x, y));
         }

    }
}
