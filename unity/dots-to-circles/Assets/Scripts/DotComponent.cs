using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        public float distanceFromCenter
        {
            get { return this.dot.distanceFromCenter; }
            set { this.dot.distanceFromCenter = value; }
        }
        public Vector3 position
        {
            get { return this.gameObject.transform.position; }
        }

        public int listIndex;

        // Use this for initialization
        void Start()
        {
            this.dot = new Dot(0, 0);
            this.listIndex = 0;
        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying)
                Handles.Label(this.position + Vector3.up, this.listIndex.ToString() + '-' + this.angle.ToString());
        }
    }
}
