using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RemoteTech_assist_calculator.Extension
{
    public static class PartExtentions
    {
        public static bool IsPrimary(this Part thisPart, List<Part> partlist, int moduleCalssId)
        {
            foreach (Part part in partlist)
            {
                if(part.Modules.Contains(moduleCalssId))
                {
                    if (part == thisPart)
                         return true;
                    else
                        break;
                    
                }

            }

            return false;
        }

        public static bool IsPrimaryEditor(this Part thisPart, int moduleCalssId)
        {
            if (EditorLogic.fetch != null)
                {
                    foreach (Part part in EditorLogic.fetch.ship.Parts)
                    {
                        if (part.Modules.Contains(moduleCalssId))
                        {
                            if (thisPart == part)
                            {
                                return true;
                            }
                            break;
                        }
                    }
                
                return false;
            }
            return false;
        }



        public static double TotalElectricConsumption()
        {
            int charge = 0;

            var parts = new List<Part>(EditorLogic.fetch.ship.parts);
            foreach (Part part in parts)
            {
            //  var info = new List<IModuleInfo>(part.partInfo.moduleInfos); 
                
                
                //charge+=part.partInfo.moduleInfos
            }


            return charge;
        }

        public static double TotalElectricChargeAvalible()
        {
            double charge = 0;

            var parts = new List<Part>(EditorLogic.fetch.ship.parts);
            foreach (Part part in parts)
              foreach (PartResource resource in part.Resources)
                 if(resource.resourceName == "ElectricCharge" )
                       charge+= resource.amount;
                       
           return charge;
        }
    }
}
