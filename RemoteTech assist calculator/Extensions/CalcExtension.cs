using System;
using UnityEngine;


namespace RemoteTech_assist_calculator.Extension
{
    public static class CalcExtention
    {
        public static double length(Vector2d P1, Vector2d P2)
        {
          return Math.Sqrt(Math.Pow(P2.x - P1.x, 2) + Math.Pow(P2.y - P1.y, 2));
        }
        
        public static double distanceBetweenPointAndLine(Vector2d Point, Vector2d onLine1, Vector2d onLine2)
        {
           Vector2d h = new Vector2d(onLine1.x + (onLine2.x - onLine1.x) / 2, onLine1.y + (onLine2.y - onLine1.y) / 2);
           return length(Point, h);
        }

        public static Vector2d SatPosition(int index,int count,CelestialBody Body,int altitude)
        {
           double radius = Body.Radius/1000 + altitude;
           return new Vector2d(radius * Math.Cos(2 * Math.PI / count * index), + radius * Math.Sin(2 * Math.PI / count * index));
        }

        public static bool isS2SCommunicationPossible(CelestialBody Body, int alt,int index,int count)
        {
            if (count < 2) return false;

            if (distanceBetweenPointAndLine(new Vector2d(0, 0), SatPosition(index,count,Body,alt), SatPosition(index+1,count,Body,alt)) > Body.Radius/1000)
                return true;

            return false;
        }

        
        public static double circleCrossPoint(Vector2d origin,Vector2d center1,Vector2d center2,int radius,bool mode)
        {   
            double dist = length(center1, center2);
            double rad1 = Math.Atan2(center2.y - center1.y, center2.x - center1.x);
            double rad2 = Math.Acos(dist / (2 * radius));
            Vector2d cross1 = new Vector2d(center1.x + radius * Math.Cos(rad1 + rad2), center1.y + radius * Math.Sin(rad1 + rad2));
            Vector2d cross2= new Vector2d(center1.x + radius * Math.Cos(rad1 - rad2), center1.y + radius * Math.Sin(rad1 - rad2));
                 if (mode) 
                      return length(origin, cross1) > length(origin, cross2) ? length(origin, cross1) : length(origin, cross2);
                 else 
                      return length(origin, cross1) < length(origin, cross2) ? length(origin, cross1) : length(origin, cross2);
        }


        public static double StableLimitAltitude(int range, int index, int count, CelestialBody Body, int altitude)
        {
            return circleCrossPoint(new Vector2d(0, 0), SatPosition(index, count, Body, altitude), SatPosition(index+1, count, Body, altitude), range, true)- Body.Radius/1000;
        }

        public static double MinAltitudeForS2SCommunication(CelestialBody Body, int alt, int index, int count)
        {
            if (count < 3)
                return 0;

            while(!isS2SCommunicationPossible(Body,alt,index,count))
                {
                    alt += 10;
                }
            return alt;
        }

        public static double DistanceBetweenS2S(CelestialBody Body, int alt, int index, int count)
        {
            return length(SatPosition(index, count, Body, alt), SatPosition(index + 1, count, Body, alt));
        }

        public static double orbitalPeriod(double bodyRadius,int altitude,double stdGravParam) 
        {

          return 2 * Math.PI * Math.Sqrt(Math.Pow(bodyRadius + altitude, 3) / stdGravParam);
        }

       public static double orbitalNightTime(double bodyRadius,int altitude,double stdGravParam) 
       {
     
        double ra = bodyRadius + altitude;
        return 2 * Math.Pow(ra, 2) / Math.Sqrt(ra * stdGravParam) * Math.Asin(bodyRadius / ra);
       }

       public static double hohmannStartDeltaV(double bodyRadius,double altitude1,int altitude2,double stdGravParam) 
       {

        double r1 = bodyRadius + altitude1;
        double r2 = bodyRadius + altitude2;
        return Math.Sqrt(stdGravParam / r1) * (Math.Sqrt((2 * r2) / (r1 + r2)) - 1);
       }

       public static double hohmannFinishDeltaV(double bodyRadius,double altitude1,int altitude2,double stdGravParam) 
       {
       
        double r1 = bodyRadius + altitude1;
        double r2 = bodyRadius + altitude2;
        return Math.Sqrt(stdGravParam / r2) * (1 - Math.Sqrt((2 * r1) / (r1 + r2)));
       }

        public static double OrbitalSlidePhaseAngle(int slideDeg,double periodLow,double periodHigh) 
        {
        return slideDeg / (1 - periodLow / periodHigh);
       }

       public static double slidePhaseAngle(double radius,int count,double parkingAltitude,int altitude,double stdGravParam) 
       {

        double periodLow = orbitalPeriod(radius, (int)parkingAltitude,stdGravParam);
        double periodHigh = orbitalPeriod(radius, altitude,stdGravParam);

        return OrbitalSlidePhaseAngle(360 / count, periodLow, periodHigh);
       }

       public static double slidePhaseTime(double slidePhaseAngle,double radius,double parkingAltitude,double stdGravParam)
       {
        return slidePhaseAngle / 360 * orbitalPeriod(radius, (int)parkingAltitude,stdGravParam);
       }




       public static double FindDriftOrbitalPeriod(this Vessel vessel)
       {
          Orbit vO = vessel.orbit;
          if (vO.radius<=0) return 0;

          double Operiod = vO.orbitPercent;

          return Operiod;
       }


       public static int GetScaleFactorForGraphic(int BodyRadius, int altitude, int maxRad)
       {
           int factor = 0;
           int standardSizeFactor = BodyRadius * 100 / 600;
           if(standardSizeFactor < 25) 
            standardSizeFactor = 25;

           int startFactor = standardSizeFactor * 100 / BodyRadius;
           factor = (BodyRadius*startFactor/100 + altitude * startFactor / 100) / 2;

           if (factor < maxRad)
               return startFactor;
           else
               return maxRad*2 * 100 / (BodyRadius + altitude);
        }




   }
}
