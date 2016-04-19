using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class SphereSerializable
{

    public bool InsideOut { get; set; }
    public double Radius { get; set; }
    public bool[][] WallHereJagged { get; set; }
    //public float WallHeight { get; set; }
    //public Vector3 LocalPos { get; set; }
    //public Vector3 LocalEuler { get; set; }
    //public Vector3 LocalScale { get; set; }

    private SphereSerializable()
    {

    }

    public SphereSerializable(bool isInverse, double radius, bool[][] wallHere)
        //float wallHeight, Vector3 localPos, Vector3 localEuler, Vector3 localScale)
    {
        InsideOut = isInverse;
        Radius = radius;
        WallHereJagged = wallHere;
        //WallHeight = wallHeight;
        //LocalPos = localPos;
        //LocalEuler = localEuler;
        //LocalScale = localScale;
    }


    //public Material[] materials;
    //private MeshHolder ground;
    //private MeshHolder vertWalls;
    //private MeshHolder horzWalls;

    //private Vector3[] vertices;
    //private Mesh mesh;

}
