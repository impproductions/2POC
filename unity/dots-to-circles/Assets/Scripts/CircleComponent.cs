using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTC;
using System;

public class CircleComponent : MonoBehaviour
{
    public List<DotComponent> dots;
    public List<DotComponent> sortedDots;
    public Vector3 centroid;

    // Use this for initialization
    void Start()
    {
        this.SetVertexObjects();
    }

    private void Update()
    {
        this.SetVertexObjects();
    }
    public void SetVertexObjects()
    {
        // sort vertices to form a circle-ish shape
        this.dots = new List<DotComponent>(GetComponentsInChildren<DotComponent>()); // get array, convert to list
        this.sortedDots = new List<DotComponent>(this.dots);
        this.SortDotsClockwise();
    }

    public void SortDotsClockwise()
    {
        this.centroid = CreateShape.GetObjectsCentroid(this.dots.ConvertAll<GameObject>(dot => dot.gameObject));
        this.centroid = new Vector3(centroid.x, 0, centroid.z);

        if (this.dots.Count > 0)
        {
            Vector3 zeroDegrees = Vector3.ProjectOnPlane(dots[0].position - this.centroid, Vector3.up).normalized;

            foreach (DotComponent dot in dots)
            {
                Vector3 projectedDotDir = Vector3.ProjectOnPlane(dot.position - this.centroid, Vector3.up);
                // TODO: REVIEW
                float distanceFromCenter = Vector3.Distance(Vector3.ProjectOnPlane(dot.position, Vector3.up), Vector3.ProjectOnPlane(this.centroid, Vector3.up));
                float projectedAngle = MathHelper.Get360Angle(zeroDegrees, projectedDotDir, Vector3.up); // get 360° angle relative to the center-dot[0] line

                dot.angle = projectedAngle;
                dot.distanceFromCenter = distanceFromCenter;
            }

            // sort by angle and distance
            this.sortedDots.Sort(SortDotsClockwise);

            // draw lines and debug text in editor
            HandleDebugging();
        }
    }

    private static int SortDotsClockwise(DotComponent x, DotComponent y)
    {
        int result = x.angle.CompareTo(y.angle);

        if (result == 0) // same angles
        {
            result = x.distanceFromCenter.CompareTo(y.distanceFromCenter);
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
                dot.listIndex = i;
                Debug.DrawLine(this.sortedDots[i].position, this.sortedDots[(i + 1) % this.sortedDots.Count].position, Color.blue);
                Debug.DrawLine(this.sortedDots[i].position, this.centroid, new Color(0, 1, 0, 0.3f));
            }
        }
    }
}


//public class CircleComponent : MonoBehaviour
//{

//    public List<GameObject> vertexObjs;
//    public List<GameObject> sortedObjs;

//    public Vector3[] vertices;
//    public int[] triangles;
//    public Vector3[] normals;
//    public Vector2[] uvs;

//    private MeshFilter meshFilter;
//    private Mesh mesh;

//    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
//    private ParticleSystem.ShapeModule[] psShapes;

//    // Use this for initialization
//    void Start()
//    {
//        if (vertexObjs.Count > 0)
//        {
//            MakeMesh(vertexObjs);
//        }

//    }

//    private void InitializeMeshData()
//    {
//        meshFilter = GetComponent<MeshFilter>();
//        mesh = new Mesh();

//        this.vertices = new Vector3[sortedObjs.Count + 1];
//        this.triangles = new int[sortedObjs.Count * 3];
//        this.normals = new Vector3[sortedObjs.Count + 1];
//        this.uvs = new Vector2[sortedObjs.Count + 1];
//    }

//    public void MakeMesh(List<GameObject> objs)
//    {
//        if (objs.Count > 0)
//        {
//            SetVertexObjects(objs);
//            InitializeMeshData();
//            CreateMesh();
//            UpdateMesh();

//            InitializeParticleSystems();
//            ResetParticleSystemsShapeModule();
//            UpdateParticleSystems();
//        }

//    }

//    public void SetVertexObjects(List<GameObject> objs)
//    {
//        // sort vertices to form a circle-ish shape
//        this.vertexObjs = objs;
//        this.sortedObjs = CreateShape.SortGameObjectsClockwise(objs);
//    }

//    public void UpdateMesh()
//    {
//        this.meshFilter.mesh = mesh;
//    }

//    public void CreateMesh()
//    {
//        Vector3 center = CreateShape.GetObjectsCenter(sortedObjs);
//        transform.position = center;

//        for (int i = 0; i <= this.sortedObjs.Count; i++)
//        {
//            if (i == 0)
//            {
//                this.vertices[i] = center - transform.position;
//            }
//            else
//            {
//                this.vertices[i] = this.sortedObjs[i - 1].transform.position - transform.position;
//            }

//            if (i < sortedObjs.Count)
//            {
//                this.triangles[i * 3] = 0;
//                this.triangles[i * 3 + 1] = i + 1;
//                this.triangles[i * 3 + 2] = (i + 2) < this.sortedObjs.Count + 1 ? (i + 2) : 1;
//            }

//            this.normals[i] = Vector3.up;
//            this.uvs[i] = new Vector2(0, 0);
//        }

//        this.mesh.vertices = this.vertices;
//        this.mesh.triangles = this.triangles;
//        this.mesh.normals = this.normals;
//        this.mesh.uv = this.uvs;

//    }

//    public void UpdateParticleSystems()
//    {
//        for (int i = 0; i < this.psShapes.Length; i++)
//        {
//            this.psShapes[i].mesh = mesh;
//        }
//    }

//    private void ResetParticleSystemsShapeModule()
//    {
//        for (int i = 0; i < this.psShapes.Length; i++)
//        {
//            this.psShapes[i].scale = Vector3.one;
//            this.psShapes[i].rotation = Vector3.zero;
//        }
//    }

//    private void InitializeParticleSystems()
//    {
//        particleSystems.Clear();

//        // get particle systems
//        for (int i = 0; i < transform.childCount; i++)
//        {
//            var childObject = transform.GetChild(i);
//            var childPS = childObject.GetComponent<ParticleSystem>();

//            if (childPS != null)
//            {
//                particleSystems.Add(childPS);
//            }
//        }

//        psShapes = new ParticleSystem.ShapeModule[particleSystems.Count];

//        for (int i = 0; i < particleSystems.Count; i++)
//        {
//            psShapes[i] = particleSystems[i].shape;
//        }
//    }
//}
