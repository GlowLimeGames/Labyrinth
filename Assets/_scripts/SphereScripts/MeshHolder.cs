using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class MeshHolder
{

    public List<Vector3> verts;
    public List<int> tris;
    public List<Vector3> normals;
    public List<Vector2> uvs;

    public MeshHolder()
    {
        verts = new List<Vector3>();
        tris = new List<int>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();
    }

    public void AddTriangleToList( int tri1, int tri2, int tri3, bool reverse)
    {
        if (reverse)
        {
            tris.Add(tri3);
            tris.Add(tri2);
            tris.Add(tri1);
        }
        else
        {
            tris.Add(tri1);
            tris.Add(tri2);
            tris.Add(tri3);
        }
    }


}
