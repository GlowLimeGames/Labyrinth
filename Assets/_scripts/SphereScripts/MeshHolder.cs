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

    public int nextTriIdx { get; private set; }

    public MeshHolder(int initialTriangleIndex)
    {
        verts = new List<Vector3>();
        tris = new List<int>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();
        nextTriIdx = initialTriangleIndex;
    }

    public void AddSquare(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, bool reverse)
    {
        verts.Add(p0);
        verts.Add(p1);
        verts.Add(p2);
        verts.Add(p3);

        int v0,v1,v2,v3;
        v0 = nextTriIdx;
        v1 = v0 + 1;
        v2 = v0 + 2;
        v3 = v0 + 3;
        nextTriIdx += 4;
        AddTriangleToList(v0, v1, v3, reverse);
        AddTriangleToList(v0, v3, v2, reverse);
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
