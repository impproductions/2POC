using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DTC.Tools;


namespace DTC
{
    public class CircleComponent : MonoBehaviour
    {
        public List<DotComponent> dots;
        public List<DotComponent> sortedDots;
        public Vector3 centroid;

        void Start()
        {
            this.CreatePath();
        }

        private void Update()
        {
            this.CreatePath();
        }

        private void CreatePath()
        {
            this.SetDots();

            if (this.dots.Count > 0)
            {
                this.SetCentroid();
                this.SetSortedDots();

                // do rendering stuff now
            }
        }

        // grab children dot components
        public void SetDots()
        {
            this.dots = new List<DotComponent>(GetComponentsInChildren<DotComponent>()); // returns array, convert to list
            this.sortedDots = new List<DotComponent>(this.dots);
        }

        // sort dots to form a circle-ish path
        public void SetSortedDots()
        {
            // reference 0deg angle
            Vector3 zeroDegrees = (this.dots[0].projectedPosition - this.centroid).normalized;

            foreach (DotComponent dot in this.dots)
            {
                Vector3 centroidToDotDirection = (dot.projectedPosition - this.centroid).normalized;
                float distanceFromCentroid = Vector3.Distance(dot.projectedPosition, this.centroid);
                float dotAngle = VectorHelper.Get360Angle(zeroDegrees, centroidToDotDirection, Vector3.up); // get 360° angle relative to the center-dot[0] line

                dot.angle = dotAngle;
                dot.distanceFromCentroid = distanceFromCentroid;
            }

            // sort by angle and distance
            this.sortedDots.Sort(ClockwiseSortComparer);

            // draw lines and debug text in editor
            HandleDebugging();
        }
        private void SetCentroid()
        {
            var xValues = 0f;
            var yValues = 0f;
            var zValues = 0f;

            for (int i = 0; i < dots.Count; i++)
            {
                xValues += this.dots[i].projectedPosition.x;
                yValues += this.dots[i].projectedPosition.y;
                zValues += this.dots[i].projectedPosition.z;
            }

            this.centroid = new Vector3(xValues / this.dots.Count, yValues / this.dots.Count, zValues / this.dots.Count);
        }

        private static int ClockwiseSortComparer(DotComponent x, DotComponent y)
        {
            int result = x.angle.CompareTo(y.angle);

            if (result == 0) // same angles
            {
                // sort based on distance from centroid, to avoid line spiking out from the shape
                result = x.distanceFromCentroid.CompareTo(y.distanceFromCentroid);
            }

            return result;
        }

        private void HandleDebugging()
        {
            if (Application.isPlaying)
            {
                for (int i = 0; i < this.sortedDots.Count; i++)
                {
                    DotComponent dot = this.sortedDots[i];
                    Debug.DrawLine(this.sortedDots[i].projectedPosition, this.sortedDots[(i + 1) % this.sortedDots.Count].projectedPosition, Color.blue);
                    Debug.DrawLine(this.sortedDots[i].projectedPosition, this.centroid, new Color(0, 1, 0, 0.3f));
                }
            }
        }
    }
}