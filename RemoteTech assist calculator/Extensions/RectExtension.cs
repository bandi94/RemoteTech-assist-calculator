using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;



namespace RemoteTech_assist_calculator.Extension
{
        
  public static class RectExtention
    {
       public static Rect CenterScreen(this Rect thisRect)
        {
            if (Screen.width > 0 && Screen.height > 0 && thisRect.width > 0f && thisRect.height > 0f)
            {
                thisRect.x = Screen.width / 2 - thisRect.width / 2;
                thisRect.y = Screen.height / 2 - thisRect.height / 2;
                thisRect.width =(float)(Screen.width * 0.14);
            }
            return thisRect;
        }


     }
}
