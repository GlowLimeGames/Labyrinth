using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereGrid : MonoBehaviour
{

    public int xSize, ySize;
    public double radius;
    public bool insideOut;
    public float wallHeight;
    public bool drawGizmos;
    private Vector3[] vertices;
    private Vector3[] samples;
    private Mesh mesh;
    private bool[,] wallHere;

    private void Awake()
    {
        setDefaultWallPlan();
        Generate();
        ExtrudeWalls();
        //StartCoroutine(Generate());
    }

    private void ExtrudeWalls()
    {
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if (wallHere[x, y])
                {
                    int i = 2 * x + (4 * xSize) * y;
                    int[] idxs = { i, i + 1, i + 2 * xSize, i + 1 + 2 * xSize };
                    foreach (int idx in idxs)
                    {
                        Vector3 extrudeDir = vertices[idx].normalized;
                        extrudeDir *= wallHeight;
                        if (insideOut)
                            extrudeDir *= -1;
                        vertices[idx] = vertices[idx] + extrudeDir;
                    }
                }
            }
        }
        mesh.vertices = vertices;
    }


    void setDefaultWallPlan()
    {
        wallHere = new bool[xSize,ySize];
        //for (int i = 0; i < xSize; i++)
        //    wallPlan[i, 0] = wallPlan[i, ySize - 1] = true;
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if (y < 3 || y > (ySize - 3) || x < 3)
                    wallHere[x, y] = false;
                else if(y % 2 != 0)
                    wallHere[x, y] = true;
                //Debug.Log(String.Format("{0}", wallHere[x, y].ToString()));
            }
        }
    }

    private void Generate()
    {
        //WaitForSeconds wait = new WaitForSeconds(0.05f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[4 * xSize * ySize]; // 4 vertices per square
        int[] triangles = new int[xSize * ySize * 6];
        double deltaPhi = 2*Math.PI/xSize;
        double deltaTheta = Math.PI/ySize;
        double r = radius;
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                int i = 2*x + (4*xSize)*y;
                double phi = 2*Math.PI*x/xSize; // X angle
                double theta = Math.PI*y/ySize; // Y angle 
                // Boxes are arranged 0 1 2    Each Square idx is arranged  0 1
                //                    3 4 5                                 2 3
                // Vertex 0
                vertices[i] = new Vector3(               
                    (float) (r*Math.Sin(theta)*Math.Cos(phi)),
                    (float) (r*Math.Cos(theta)),                
                    (float) (r*Math.Sin(theta)*Math.Sin(phi)));
                // Vertex 1
                vertices[i + 1] = new Vector3(
                    (float) (r*Math.Sin(theta)*Math.Cos(phi + deltaPhi)),
                    (float) (r*Math.Cos(theta)),
                    (float) (r*Math.Sin(theta)*Math.Sin(phi + deltaPhi)));
                // Vertex 2
                vertices[i + 2*xSize] = new Vector3(
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Cos(phi)),
                    (float)(r * Math.Cos(theta + deltaTheta)),
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Sin(phi)));
                // Vertex 3
                vertices[i + 1 + 2*xSize] = new Vector3(
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Cos(phi + deltaPhi)),
                    (float)(r * Math.Cos(theta + deltaTheta)),
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Sin(phi + deltaPhi)));

                // Triangle Idx of top left corner of current square
                int ti = 6 * x + (6 * xSize) * y;
                if (insideOut)
                {
                    int[] idxs = { i, i + 1, i + 2 * xSize, i + 1 + 2 * xSize };
                    triangles[ti] = triangles[ti + 3] = idxs[0];
                    triangles[ti + 1] = triangles[ti + 5] = idxs[3];
                    triangles[ti + 2] = idxs[1];
                    triangles[ti + 4] = idxs[2];
                }
                else
                {
                    int[] idxs = { i, i + 1, i + 2 * xSize, i + 1 + 2 * xSize };
                    triangles[ti] = triangles[ti + 3] = idxs[0];
                    triangles[ti + 1] = idxs[1];
                    triangles[ti + 2] = triangles[ti + 4] = idxs[3];
                    triangles[ti + 5] = idxs[2];
                }
                
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        //yield return null;
        return;

        #region Scrap Code

        //Vector2[] uv = new Vector2[vertices.Length];
        //Vector4[] tangents = new Vector4[vertices.Length];
        //Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);


        //for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        //{
        //    for (int x = 0; x < xSize; x++, ti += 6, vi++)
        //    {
        //        // Add 
        //        triangles[ti] = vi;
        //        triangles[ti + 3] = triangles[ti + 2] = vi + 1;
        //        triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
        //        triangles[ti + 5] = vi + xSize + 2;


        //        //yield return wait;
        //    }
        //}
        //mesh.triangles = triangles;

        #endregion

    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        if (drawGizmos)
        {
            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);
            }
            Gizmos.color = Color.magenta;
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    if (wallHere[x, y])
                    {
                        int i = 2*x + (4*xSize)*y;
                        int[] idxs = {i, i + 1, i + 2*xSize, i + 1 + 2*xSize};
                        foreach (int idx in idxs)
                        {
                            Vector3 extrudeDir = vertices[idx].normalized;
                            extrudeDir *= wallHeight;
                            if (insideOut)
                                extrudeDir *= -1;
                            Gizmos.DrawRay(transform.TransformPoint(vertices[idx]), extrudeDir);
                         }
                    }
                }
            }
        }
    }


}
