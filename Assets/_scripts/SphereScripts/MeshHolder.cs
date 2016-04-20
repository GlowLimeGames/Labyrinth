using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class MeshHolder
{
    public List<Vector3> verts { get; set; }
    public List<int> tris { get; set; }
    public List<Vector3> normals { get; set; }
    public List<Vector2> uvs { get; set; }

    public int nextTriIdx { get; private set; }

    public MeshHolder(int initialTriangleIndex)
    {
        verts = new List<Vector3>();
        tris = new List<int>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();
        nextTriIdx = initialTriangleIndex;
    }

    /// <summary>
    /// Add verts/normals/tris to structure. Assumes reading order of vertices -> 0 1; 2 3
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="reverse"></param>
    public void AddSquare(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, bool reverse, bool surfacePoint)
    {
        verts.Add(p0);
        verts.Add(p1);
        verts.Add(p2);
        verts.Add(p3);
        if (surfacePoint)
        {
            Vector3 n0 = p0 * (reverse ? -1 : 1); n0.Normalize();
            Vector3 n1 = p0 * (reverse ? -1 : 1); n1.Normalize();
            Vector3 n2 = p0 * (reverse ? -1 : 1); n2.Normalize();
            Vector3 n3 = p0 * (reverse ? -1 : 1); n3.Normalize();
            normals.Add(n0); normals.Add(n1); normals.Add(n2); normals.Add(n3);
        }
        else
        {
            Vector3 norm = Vector3.Cross((p1 - p0), (p2 - p0));
            norm.Normalize();
            if (reverse)
                norm = -norm;
            normals.Add(norm); normals.Add(norm); normals.Add(norm); normals.Add(norm);
        }

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
