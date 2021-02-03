using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTC.Tools
{
    public static class VectorHelper
    {
        // check if two points are on the same side of a line
        public static bool SameSide(Vector3 p1, Vector3 p2, Vector3 lineStart, Vector3 lineEnd)
        {
            bool sameSide = false;

            Vector3 cp1 = Vector3.Cross((lineEnd - lineStart), (p1 - lineStart));
            Vector3 cp2 = Vector3.Cross((lineEnd - lineStart), (p2 - lineStart));

            if (Vector3.Dot(cp1, cp2) >= 0)
            {
                sameSide = true;
            }
            else
            {
                sameSide = false;
            }

            return sameSide;
        }

        // get angles bigger than 180deg
        public static float Get360Angle(Vector3 from, Vector3 to, Vector3 planeNormal)
        {
            from.Normalize();
            to.Normalize();

            float angle = Vector3.Angle(from, to);
            Vector3 referencePoint = Vector3.Normalize(Quaternion.AngleAxis(90, planeNormal) * from);
            float newAngle = 0;

            //Debug.DrawLine(Vector3.zero, from, Color.blue);
            //Debug.DrawLine(Vector3.zero, -from, Color.blue);
            //Debug.DrawLine(Vector3.zero, referencePoint, Color.blue);

            if (SameSide(referencePoint, to, Vector3.zero, from))
            {
                //Debug.DrawLine(Vector3.zero, to, Color.green);
                newAngle = angle;
            }
            else 
            {
                //Debug.DrawLine(Vector3.zero, to, Color.red);
                newAngle = 360 - angle;
            }

            return newAngle;
        }
    }
}

