using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereGrid : MonoBehaviour
{

    public int xSize, ySize;
    public double radius;
    public bool insideOut;
    private Vector3[] vertices;
    private Vector3[] samples;
    private Mesh mesh;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

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
        yield return null;

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
            
    }
    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);
            //Debug.Log(vertices[i]);
            
        }
    }


}
