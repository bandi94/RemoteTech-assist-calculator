using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RemoteTech_assist_calculator.Extension
{
    public class PlanetsExtention 
    {
       public static string[][] GetPlanetNames()
        {
            CelestialBody Sun = Planetarium.fetch.Sun;
            List<CelestialBody> Planets = Sun.orbitingBodies;

            string[][] PlanetNames = new string[Planets.Count][];
            int i = 0,j=0;
 
            foreach (CelestialBody Body in Planets)
            {
                PlanetNames[i] = new string[Body.orbitingBodies.Count+1];
                PlanetNames[i][0] = Body.name;

                 j=1;

                 foreach(CelestialBody OrbBody in Body.orbitingBodies)
                     {
                          PlanetNames[i][j] = OrbBody.name;
                          j++;
                     }
                 i++;
            }

            return PlanetNames;
        }


       public static CelestialBody GetCelestialBodyFromName(string name)
       {
           CelestialBody Sun = Planetarium.fetch.Sun;
           List<CelestialBody> Planets = Sun.orbitingBodies;

           foreach (CelestialBody Body in Planets)
           {
               if (String.Compare(Body.name, name) == 0)
                   return Body;

               foreach (CelestialBody OrbBody in Body.orbitingBodies)
               {
                   if (String.Compare(OrbBody.name, name) == 0)
                       return OrbBody;
               }
           }

           return null;
       }

 
    }
}
