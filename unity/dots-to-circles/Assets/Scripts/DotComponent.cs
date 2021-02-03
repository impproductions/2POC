using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DTC.Tools;

namespace DTC {
    public class Dot
    {
        public float angle;
        public float distanceFromCenter;

        public Dot(float angle, float distanceFromCenter)
        {
            this.angle = angle;
            this.distanceFromCenter = distanceFromCenter;
        }
    }

    public class DotComponent : MonoBehaviour
    {
        private Dot dot;
        public float angle
        {
            get { return this.dot.angle; }
            set { this.dot.angle = value; }
        }
        public float distanceFromCentroid
        {
            get { return this.dot.distanceFromCenter; }
            set { this.dot.distanceFromCenter = value; }
        }
        public Vector3 projectedPosition
        {
            get { return Vector3.ProjectOnPlane(this.gameObject.transform.position, Vector3.up); }
        }

        void Start()
        {
            this.dot = new Dot(0, 0);
        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying)
                Handles.Label(this.projectedPosition + Vector3.up, this.angle.ToString() + "deg");
        }
    }
}
