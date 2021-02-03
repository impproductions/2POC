using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTC {
    public struct PolygonData
    {
        public Vector3[] vertices;
        public int[] triangles;

        public PolygonData(Vector3[] vertexPositions, int[] triangleVertices)
        {
            vertices = vertexPositions;
            triangles = triangleVertices;
        }
    }

    public static class CreateShape
    {
        public static List<GameObject> SortGameObjectsClockwise(List<GameObject> objs)
        {
            Dictionary<float, GameObject> objectAnglesRelation = new Dictionary<float, GameObject>();

            List<float> objectAngles = new List<float>();
            List<GameObject> sortedList = new List<GameObject>();

            Vector3 center = GetObjectsCentroid(objs);
            center = new Vector3(center.x, 0, center.z);

            if (objs.Count > 0)
            {
                Vector3 zeroDegrees = Vector3.ProjectOnPlane(objs[0].transform.position - center, Vector3.up).normalized;

                foreach (GameObject obj in objs)
                {
                    float projectedAngle;
                    Vector3 projectedObjDir = Vector3.ProjectOnPlane(obj.transform.position - center, Vector3.up);

                    projectedAngle = MathHelper.Get360Angle(zeroDegrees, projectedObjDir, Vector3.up);

                    objectAnglesRelation.Add(projectedAngle, obj);
                    objectAngles.Add(projectedAngle);

                    

                }


                objectAngles.Sort();

                for (int i = 0; i < objectAngles.Count; i++)
                {
                    sortedList.Add(objectAnglesRelation[objectAngles[i]]);
                }


                for (int i = 0; i < sortedList.Count; i++)
                {
                    //Debug.DrawLine(sortedList[i].transform.position, sortedList[(i+1) % sortedList.Count].transform.position, Color.blue);
                }

                //Debug.DrawRay(center, zeroDegrees * 50, Color.green);
            }

            return sortedList;
        }

        public static Vector3 GetObjectsCentroid(List<GameObject> objs)
        {
            var xValues = 0f;
            var yValues = 0f;
            var zValues = 0f;

            for (int i = 0; i < objs.Count; i++)
            {
                xValues += objs[i].transform.position.x;
                yValues += objs[i].transform.position.y;
                zValues += objs[i].transform.position.z;
            }

            return new Vector3(xValues / objs.Count, yValues / objs.Count, zValues / objs.Count);
        }

        public static PolygonData CreatePolygon(List<GameObject> objs)
        {
            PolygonData polygonData = new PolygonData();

            objs = SortGameObjectsClockwise(objs);
            Vector3 center = GetObjectsCentroid(objs);

            Vector3[] vertices = new Vector3[objs.Count+1];
            int[] triangles = new int[objs.Count*3];

            for (int i = 0; i <= objs.Count; i++)
            {
                if (i == 0)
                {
                    vertices[i] = center;
                }
                else
                {
                    vertices[i] = objs[i - 1].transform.position;
                }

                if (i < objs.Count)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = (i + 2) < objs.Count + 1 ? (i + 2) : 1;
                }

            }

            polygonData.vertices = vertices;
            polygonData.triangles = triangles;

            return polygonData;
        }

        public static bool ObjectInPolygon(GameObject obj, List<GameObject> vertexObjectList)
        {
            PolygonData polygonData = CreateShape.CreatePolygon(vertexObjectList);
            int[] triangles = polygonData.triangles;
            Vector3[] vertices = polygonData.vertices;

            bool inPolygon = false;

            for (int i = 0; i < triangles.Length / 3; i++)
            {
                var point = Vector3.ProjectOnPlane(obj.transform.position, Vector3.up);
                var t1 = Vector3.ProjectOnPlane(vertices[triangles[i * 3]], Vector3.up);
                var t2 = Vector3.ProjectOnPlane(vertices[triangles[i * 3 + 1]], Vector3.up);
                var t3 = Vector3.ProjectOnPlane(vertices[triangles[i * 3 + 2]], Vector3.up);

                bool inTriangle = MathHelper.PointInTriangle(point, t1, t2, t3);

                inPolygon = inPolygon || inTriangle;

                var color = inTriangle ? Color.red : Color.white;
                Debug.DrawLine(vertices[triangles[i * 3]], vertices[triangles[(i * 3 + 1)]], color);
                Debug.DrawLine(vertices[triangles[i * 3 + 1]], vertices[triangles[(i * 3 + 2)]], color);
                Debug.DrawLine(vertices[triangles[i * 3 + 2]], vertices[triangles[i * 3]], color);
            }

            return inPolygon;

        }
    }

    public static class ListHelper
    {
        public static Dictionary<T, int> GetHistogram<T>(List<T> list)
        {
            Dictionary<T,int> hist = new Dictionary<T, int>();

            foreach (T element in list)
            {
                if (!hist.ContainsKey(element))
                {
                    hist.Add(element, 1);
                }
                else
                {
                    hist[element] += 1;
                }
            }

            return hist; 
        }

        public static bool ContainsAllItems<T>(List<T> container, List<T> contained)
        {
            bool contains = true;
            List<T> matchedList = new List<T>(container);

            foreach (T element in contained)
            {
                // check if the element is available in the container
                if (matchedList.Contains(element))
                {
                    // remove from matched list if present
                    if (matchedList.Contains(element))
                    {
                        matchedList.RemoveAt(matchedList.IndexOf(element));
                    }
                }
                else
                {
                    return false;
                }
            }

            return contains;
        }
    }

    public static class MathHelper
    {
        public static int GetPermutations(float n)
        {
            int permutations = 0;

            for (int i = 1; i <= n; i++)
            {
                permutations += i;
            }

            return permutations;
        }

        public static bool PointInTriangle(Vector3 point, Vector3 t1, Vector3 t2, Vector3 t3)
        {
            bool inTriangle = false;

            if (SameSide(point, t1, t2, t3) && SameSide(point, t2, t3, t1) && SameSide(point, t3, t1, t2))
            {
                inTriangle = true;
            }
            else
            {
                inTriangle = false;
            }

            return inTriangle;
        }

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

    public static class GameObjectHelper
    {
        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }

    }
}

