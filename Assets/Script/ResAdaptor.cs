using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResAdaptor{
     static readonly float  originWidth = 30;
     static readonly float  originHeight = 20;
     static float xRate = 0;
     static float yRate = 0;
 
     public static float XRate
     {
        get
        {
           if(xRate == 0)
              xRate = Screen.width / originWidth;
           return xRate;
        }
     }
 
     public static float YRate
     {
        get
        {
           if(yRate == 0)
              yRate = Screen.height/ originHeight;
           return yRate;
        }
     }
 
     public static Vector2 AVector2(float x, float y)
     {
         return new Vector2(x * XRate, y * YRate);
     }
  
     public static Rect ARect(float x, float y, float width, float height)
     {
         return new Rect(x * XRate, y * YRate, width * XRate, height * YRate);
     }
}

