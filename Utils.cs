using UnityEngine;

namespace BDMk22Plugin
{
    public static class Utils
    {
        public static Vector2 RotateAroundPoint(Vector2 input, Vector2 pivotPoint, float angle)
        {
            var relVector = input - pivotPoint;
            relVector = Quaternion.AngleAxis(angle, Vector3.forward)*relVector;
            return pivotPoint + relVector;
        }


        public static float GetRadarAltitude(Vessel vessel)
        {
            var radarAlt = Mathf.Clamp((float) (vessel.mainBody.GetAltitude(vessel.CoM) - vessel.terrainAltitude), 0,
                (float) vessel.altitude);
            return radarAlt;
        }
    }
}