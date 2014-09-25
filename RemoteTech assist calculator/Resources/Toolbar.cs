#region Using Directives

using System.IO;
using System.Reflection;
using UnityEngine;

#endregion
using System.Collections.Generic;

namespace RemoteTech_assist_calculator
{
  
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
   public class Toolbar:MonoBehaviour
    {
        private static Texture2D texture;
        private readonly Settings settings = new Settings();
        private ApplicationLauncherButton buildButton;
        private ApplicationLauncherButton flightButton;

        private void Awake()
        {
            
            if (texture == null)
            {
                texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
                texture = GameDatabase.Instance.GetTexture("RemoteTech2_Assist/Textures/Toolbar", false);
            }
        }

        private void CreateButtons()
        {
            if (this.buildButton == null && (HighLogic.LoadedScene == GameScenes.EDITOR || HighLogic.LoadedScene == GameScenes.SPH || HighLogic.LoadedScene == GameScenes.FLIGHT))
            {
                this.buildButton = ApplicationLauncher.Instance.AddModApplication(
                    this.BuildOn,
                    this.BuildOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.ALWAYS,
                    texture
                    );
            }

          /*  if (this.flightButton == null && HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                this.flightButton = ApplicationLauncher.Instance.AddModApplication(
                    this.FlightOn,
                    this.FlightOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.ALWAYS,
                    texture
                    );
            }*/
        }

        private void BuildOn()
        {
            RemoteTech_assist_calculator.isVisible = true;
        }

        private void BuildOff()
        {
            RemoteTech_assist_calculator.isVisible = false;
        }
    /*
        private void FlightOn()
        {
            RemoteTech_assist_calculator.isVisibleF = true;
        }

        private void FlightOff()
        {
            RemoteTech_assist_calculator.isVisibleF = false;
        }*/
        
        private void VoidMethod() { }

        private void LateUpdate()
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR || HighLogic.LoadedScene == GameScenes.SPH || HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                if (this.buildButton != null)
                {
                    if (RemoteTech_assist_calculator.hasEngineer)
                    {
                        if (RemoteTech_assist_calculator.isVisible && this.buildButton.State != RUIToggleButton.ButtonState.TRUE)
                        {
                            this.buildButton.SetTrue();
                        }
                        else if (!RemoteTech_assist_calculator.isVisible && this.buildButton.State != RUIToggleButton.ButtonState.FALSE)
                        {
                            this.buildButton.SetFalse();
                        }
                    }
                    else
                    {
                        ApplicationLauncher.Instance.RemoveModApplication(this.buildButton);
                    }
                }
                else if (RemoteTech_assist_calculator.hasEngineer)
                {
                    this.CreateButtons();
                }

                RemoteTech_assist_calculator.hasEngineerReset = true;
            }
         /*   else if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                if (this.flightButton != null)
                {
                    if (RemoteTech_assist_calculator.hasEngineer)
                    {
                        if (RemoteTech_assist_calculator.isVisibleF && this.flightButton.State != RUIToggleButton.ButtonState.TRUE)
                        {
                            this.flightButton.SetTrue();
                        }
                        else if (!RemoteTech_assist_calculator.isVisibleF && this.flightButton.State != RUIToggleButton.ButtonState.FALSE)
                        {
                            this.flightButton.SetFalse();
                        }
                    }
                    else
                    {
                        ApplicationLauncher.Instance.RemoveModApplication(this.flightButton);
                    }
                }
                else if (RemoteTech_assist_calculator.hasEngineer)
                {
                    this.CreateButtons();
                }

                RemoteTech_assist_calculator.hasEngineerReset = true;
            }*/
        }

        private void OnDestroy()
        {
            RemoteTech_assist_calculator.hasEngineer = false;
            RemoteTech_assist_calculator.hasEngineer = false;

            if (this.buildButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(this.buildButton);
                this.buildButton = null;
            }

            if (this.flightButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(this.flightButton);
                this.flightButton = null;
            }
         
        }

        public int isToolbarIconShowed()
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR || HighLogic.LoadedScene == GameScenes.SPH)
                return 1;
            else if (HighLogic.LoadedScene == GameScenes.FLIGHT)
                return 2;
           return 0;
        }
        
 

    }
}
