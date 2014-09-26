using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RemoteTech_assist_calculator.Extension;

namespace RemoteTech_assist_calculator
{
    public class RemoteTech_assist_calculator : PartModule
    {

        private ApplicationLauncherButton _appButton;
        public static Rect _WindowPosition = new Rect();
        private static Rect _PlanetsWindow = new Rect(0, 0,(float)(Screen.width * 0.07),0);
        private static Rect _GraphicWindow = new Rect(0,0,(int)(Screen.width*0.22),(int)(Screen.height*0.39));
        private static Rect _CenterImage = new Rect();
        private GUIStyle WindowStyle, LabelStyle, LabelStyleG, LabelStyleR, TextFStyle, ButtonStyle, AreaStyle, DataStyle;

        public static bool isVisible = true;
        private bool isChosing = false;
        private bool isCalcReady = false;

        private bool isInEditor = false;

        public static string[][] PlanetList;
        public static string SelectedItem = "";

        private string cmm, startDV;
        private double altlim, dist, orbp, nightt, slidea, slidet,endDV;
        private bool inEditor = false;
        private bool inOrbital = true;
        private bool inResync = false;
        private bool showGraphic = true;
        public static Texture2D PlanetT;
        public static Texture2D SatT;
        public static Texture2D PlanetT_Exterior = new Texture2D((int)(_GraphicWindow.width),(int)(_GraphicWindow.height));
        private static Material LineMat;
        private int Ialt, Icount,IBodyRadius,altRadius;

        private string count = "0", alt = "0";


        public override void OnStart(PartModule.StartState state)
        {
            ExecuteOnStartOperations();
            InitStyles();

            if (state == StartState.Editor)
            {
                isInEditor = true;
                this.part.OnEditorAttach += OnEditorAttach;
                this.part.OnEditorDetach += OnEditorDetach;
                this.part.OnEditorDestroy += OnEditorDestroy;
                OnEditorAttach();
            }
            else
            {
                isInEditor = false;
                RenderingManager.AddToPostDrawQueue(0, onDraw);
            }

            isVisible = false;
            _appButton = ApplicationLauncher.Instance.AddModApplication(this.ButtonOn, this.ButtonOff, null, null, null,null, ApplicationLauncher.AppScenes.ALWAYS, GameDatabase.Instance.GetTexture("RemoteTech2_Assist/Textures/Toolbar", false) );

        }




        private void onDraw()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && isVisible)
                if((!isInEditor && this.part.IsPrimary(this.vessel.parts, this.ClassID)) || this.part.IsPrimaryEditor(this.ClassID))
            {
                _WindowPosition = GUILayout.Window(10, _WindowPosition, onWindow, "RemoteTech Assist Calculator", WindowStyle);

                if (_WindowPosition.x == 0f && _WindowPosition.y == 0f)
                    _WindowPosition = _WindowPosition.CenterScreen();



                if (showGraphic && SelectedItem!="")
                {
                    _GraphicWindow.x = _WindowPosition.x + _WindowPosition.width;
                    _GraphicWindow.y = _WindowPosition.y;
                    _GraphicWindow = GUI.Window(12, _GraphicWindow, OnGraphic, "Graphic of satellite positioning", WindowStyle);
                }

            }

         
        }

        private void onWindow(int windowId)
        {

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal(GUILayout.Width((int)(Screen.width*0.22)));
            GUILayout.Label("Chose your Target:", LabelStyle);
            if (GUILayout.Button(SelectedItem) && !isChosing)
            {
                RenderingManager.AddToPostDrawQueue(1, DrawTargetSelection);
                isChosing = true;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Satellites count:", LabelStyle);
            count = GUILayout.TextField(count, TextFStyle, GUILayout.Width(50));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Altitude(km):", LabelStyle);
            alt = GUILayout.TextField(alt, TextFStyle, GUILayout.Width(100));
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
           if (GUILayout.Button("Update", ButtonStyle, GUILayout.Width(100)))
              OnInputDataChange();
          

            GUILayout.FlexibleSpace();
          //  inEditor = GUILayout.Toggle(inEditor, "SDA", ButtonStyle);
            inOrbital =  GUILayout.Toggle(inOrbital, "SOL", ButtonStyle);
           // inResync = GUILayout.Toggle(inResync, "SLA", ButtonStyle);
            showGraphic = GUILayout.Toggle(showGraphic, ">>", ButtonStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

                if (inOrbital)
                {
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Orbital Information Display", LabelStyle);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(AreaStyle);
                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal(); GUILayout.Label("Communication possibility: ", LabelStyle); GUILayout.FlexibleSpace(); GUILayout.Label(cmm, (String.Compare(cmm, "Yes") == 0) ? LabelStyleG : LabelStyleR); GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(); GUILayout.Label("Distance between 2 satellite: ", LabelStyle); GUILayout.FlexibleSpace(); GUILayout.Label(Convert.ToString(dist) + " km", LabelStyleG); GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(); GUILayout.Label("Orbital Period: ", LabelStyle); GUILayout.FlexibleSpace(); GUILayout.Label(Convert.ToString(orbp) + " s", LabelStyleG); GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(); GUILayout.Label("Night Time: ", LabelStyle); GUILayout.FlexibleSpace(); GUILayout.Label(Convert.ToString(nightt) + " s", LabelStyleG); GUILayout.EndHorizontal();

                    if (HighLogic.LoadedScene == GameScenes.FLIGHT)
                    {
                        GUILayout.BeginHorizontal(); GUILayout.Label("Start dV: ", LabelStyle); GUILayout.FlexibleSpace(); GUILayout.Label(Convert.ToString(startDV), LabelStyleG); GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(); GUILayout.Label("End dV: ", LabelStyle); GUILayout.FlexibleSpace(); GUILayout.Label(Convert.ToString(endDV) + " m/s", LabelStyleG); GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(); GUILayout.Label("Slide angle: ", LabelStyle); GUILayout.FlexibleSpace(); GUILayout.Label(Convert.ToString(slidea) + " deg", LabelStyleG); GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(); GUILayout.Label("Slide time: ", LabelStyle); GUILayout.FlexibleSpace(); GUILayout.Label(Convert.ToString(slidet) + " s", LabelStyleG); GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }

             /*   if (inResync)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Re-Sync Display", LabelStyle);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(AreaStyle);
                    GUILayout.BeginVertical();

                  
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }*/

                
          
            GUI.DragWindow();
         

        }






        public void DrawTargetSelection()
        {
            if (isVisible)
            {
                _PlanetsWindow = GUILayout.Window(11, _PlanetsWindow, onTargetSelectionWindow, "Chose Target", WindowStyle);
                _PlanetsWindow.x = _WindowPosition.x + _WindowPosition.width + 5;
                _PlanetsWindow.y = _WindowPosition.y;
            }
        }


        public void onTargetSelectionWindow(int windowId)
        {
            GUILayout.BeginVertical();

            for (int x = 0; x < PlanetList.Length; x++)
            {
                if (GUILayout.Button(PlanetList[x][0]))
                {
                    SelectedItem = PlanetList[x][0];
                    RenderingManager.RemoveFromPostDrawQueue(1, DrawTargetSelection);
                    isChosing = false;
                }

                for (int x1 = 1; x1 < PlanetList[x].Length; x1++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(30);
                    if (GUILayout.Button(PlanetList[x][x1]))
                    {
                        SelectedItem = PlanetList[x][x1];
                        RenderingManager.RemoveFromPostDrawQueue(1, DrawTargetSelection);
                        isChosing = false;
                    }
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndVertical();

        }






     


        private void OnInputDataChange()
        {
             Ialt = Convert.ToInt32(alt);
             Icount = Convert.ToInt32(count);
             CelestialBody SelectedBody = PlanetsExtention.GetCelestialBodyFromName(SelectedItem);
             double gravParameter = SelectedBody.gravParameter / 1000000000;
             IBodyRadius = (int)(SelectedBody.Radius / 1000);


            altlim = CalcExtention.MinAltitudeForS2SCommunication(SelectedBody, Ialt, 1, Icount);
            cmm = (string)((altlim == Ialt) ? "Yes" : "No(" + altlim + " km)");
            dist = Math.Round(CalcExtention.DistanceBetweenS2S(SelectedBody, Ialt, 1, Icount), 3);
            orbp = Math.Round(CalcExtention.orbitalPeriod(SelectedBody.Radius / 1000, Ialt, gravParameter), 3);
            nightt = Math.Round(CalcExtention.orbitalNightTime(SelectedBody.Radius / 1000, Ialt, gravParameter), 3);
         
           if( HighLogic.LoadedScene == GameScenes.FLIGHT )
            {
            startDV = Convert.ToString(Math.Round(CalcExtention.hohmannStartDeltaV(SelectedBody.Radius / 1000, this.vessel.altitude / 1000, Ialt, gravParameter) * 1000, 3)) + "m/s ( " + Math.Round(this.vessel.altitude / 1000) + "km) ";
            endDV = Math.Round(CalcExtention.hohmannFinishDeltaV(SelectedBody.Radius / 1000, this.vessel.altitude / 1000, Ialt, gravParameter) * 1000, 3);
            slidea = Math.Round(CalcExtention.slidePhaseAngle(SelectedBody.Radius / 1000, Icount, this.vessel.altitude / 1000, Ialt, gravParameter), 3);
            slidet = Math.Round(CalcExtention.slidePhaseTime(slidea, SelectedBody.Radius / 1000, this.vessel.altitude / 1000, gravParameter), 3);
            }


          //////////////////////////////////////////////// Graphics //////////////////////////////////////////////

            int scaleFactor = CalcExtention.GetScaleFactorForGraphic(IBodyRadius, Ialt, (int)(_GraphicWindow.width / 2 - 32));
            IBodyRadius = IBodyRadius * scaleFactor / 100;
            altRadius = IBodyRadius / 2 + (Ialt * scaleFactor / 100) / 2;

            PixelExtention.ClearTexture(out PlanetT_Exterior, PlanetT_Exterior);
            PixelExtention.DrawCircle(out PlanetT_Exterior, PlanetT_Exterior, PlanetT_Exterior.width / 2, PlanetT_Exterior.height / 2, altRadius, Color.red);

       }




      private void OnGraphic(int windowId)
        {
          
            GUI.DrawTexture(new Rect(_GraphicWindow.width / 2 - IBodyRadius / 2, _GraphicWindow.height / 2 - IBodyRadius / 2, IBodyRadius, IBodyRadius), PlanetT);
            GUI.DrawTexture(new Rect(0, 0, _GraphicWindow.width, _GraphicWindow.height), PlanetT_Exterior);
           
             double slice = 2 * Math.PI / Icount;
             for (int i = 0; i < Icount; i++)
             {
                 double angle = slice * i;
                 int newX = (int)(_GraphicWindow.width / 2 + altRadius * Math.Cos(angle));
                 int newY = (int)(_GraphicWindow.height / 2 + altRadius * Math.Sin(angle));
               
             
                 GUI.DrawTexture(new Rect(newX - 16, newY - 16,32,32),SatT);
                 
             }

             
         }





      private void OnEditorAttach()
      {
          if (this.part.IsPrimaryEditor(this.ClassID))
              RenderingManager.AddToPostDrawQueue(0, onDraw);
      }

      private void OnEditorDetach()
      {
          if (this.part.IsPrimaryEditor(this.ClassID))
             RenderingManager.RemoveFromPostDrawQueue(0, onDraw);
      }

      private void OnEditorDestroy()
      {
          if (this.part.IsPrimaryEditor(this.ClassID))
             RenderingManager.RemoveFromPostDrawQueue(0, onDraw);
      }

      private void OnDestroy()
      {
          if (_appButton != null)
              ApplicationLauncher.Instance.RemoveModApplication(_appButton);
      }


      private void ButtonOn()
      {
          RemoteTech_assist_calculator.isVisible = true;
      }

      private void ButtonOff()
      {
          RemoteTech_assist_calculator.isVisible = false;
      }



      private void ExecuteOnStartOperations()
      {
          PlanetList = PlanetsExtention.GetPlanetNames();
          PixelExtention.LoadImage(out PlanetT, "Planet.png", 100);
          PixelExtention.LoadImage(out SatT, "Satellite.png", 32);
          PixelExtention.ClearTexture(out PlanetT_Exterior, PlanetT_Exterior);

      }


      private void InitStyles()
      {
          WindowStyle = new GUIStyle(HighLogic.Skin.window);

          LabelStyle = new GUIStyle(HighLogic.Skin.label);
          LabelStyle.normal.textColor = Color.white;
          LabelStyle.fontStyle = FontStyle.Normal;
          LabelStyle.alignment = TextAnchor.MiddleLeft;
          LabelStyle.stretchWidth = true;

          LabelStyleG = new GUIStyle(HighLogic.Skin.label);
          LabelStyleG.normal.textColor = Color.green;
          LabelStyleG.fontStyle = FontStyle.Normal;
          LabelStyleG.alignment = TextAnchor.MiddleLeft;
          LabelStyleG.stretchWidth = true;

          LabelStyleR = new GUIStyle(HighLogic.Skin.label);
          LabelStyleR.normal.textColor = Color.red;
          LabelStyleR.fontStyle = FontStyle.Normal;
          LabelStyleR.alignment = TextAnchor.MiddleLeft;
          LabelStyleR.stretchWidth = true;

          TextFStyle = new GUIStyle(HighLogic.Skin.textField);
          TextFStyle.normal.textColor = Color.green;
          TextFStyle.fontStyle = FontStyle.Normal;

          ButtonStyle = new GUIStyle(HighLogic.Skin.button);
          ButtonStyle.normal.textColor = Color.white;
          ButtonStyle.fontStyle = FontStyle.Normal;


          AreaStyle = new GUIStyle(HighLogic.Skin.textArea);
          AreaStyle.active = AreaStyle.hover = AreaStyle.normal;

          DataStyle = new GUIStyle(HighLogic.Skin.label);
          DataStyle.fontStyle = FontStyle.Normal;
          DataStyle.alignment = TextAnchor.MiddleRight;
          DataStyle.stretchWidth = true;

      }






    }
    }