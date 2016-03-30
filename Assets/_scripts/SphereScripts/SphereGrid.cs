using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class SphereGrid : MonoBehaviour
{

    public int xSize, ySize;
    public double radius;
    public bool insideOut;
    public float wallHeight;
    public bool drawGizmos;
    public string wallPatternString;
    private Vector3[] vertices;
    private Vector3[] samples;
    private Mesh mesh;
    private bool[,] wallHere;
    private Vector3 lastClickPoint;

    private bool lastInsideOutSetting;

    private void Awake()
    {
        lastInsideOutSetting = insideOut;
        SetUpWallPlan();
        newGenerateAndExtrude();
        //SphereCollider collider = GetComponent<SphereCollider>();
        //collider.radius = (float)radius;

        if (insideOut)
        {
            //collider.radius *= -1;
            GetComponent<GravityAttractor>().gravity *= -1;
        }

        if (wallPatternString == "")
            wallPatternString = "Try setting to'Test', 'All Floors', ...";
        //StartCoroutine(GenerateAndExtrude());
    }

    #region Wall Functions
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
        mesh.RecalculateBounds();

    }

    
    void SetUpWallPlan()
    {
        wallHere = new bool[xSize, ySize];
        switch (wallPatternString)
        {
            case "All Walls":
                SetWallsToBool(true); 
                break;
            case "All Floors":
                SetWallsToBool(false);
                break;
            case "Test":
                SetTestWallPlan();
                break;
            default:
                SetWallsToBool(false); // Default to all floors
                break;
        }
    }

    private void SetWallsToBool(bool wallType)
    {
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                wallHere[x, y] = wallType;
            }
        }
    }

    void SetTestWallPlan()
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
    #endregion

    private void newGenerateAndExtrude()
    {
        // TODO Create walls, perhaps using dictionary to help

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        // TODO add multiple materials for ground/floors etc
        // GetComponent<Renderer>().materials = m;

        mesh.name = "Procedural Grid";

        vertices = new Vector3[4 * xSize * ySize]; // 4 vertices per square
        Vector3[] normals = new Vector3[vertices.Length];
        //int[] triangles = new int[xSize * ySize * 6];
        List<int> triangleGroundList = new List<int>();
        List<int> triangleWallList = new List<int>();
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
                //int ti = 6 * x + (6 * xSize) * y;
                int[] idxs = { i, i + 1, i + 2 * xSize, i + 1 + 2 * xSize };
                int TL = i;
                int TR = i + 1;
                int BL = i + 2 * xSize;
                int BR = i + 2 * xSize + 1;
                // Add both triangles, reversing order if inside out.
                AddTriangleToList(triangleGroundList, TL, TR, BR, insideOut);
                AddTriangleToList(triangleGroundList, TL, BR, BL, insideOut);

            }
        }
        for (int vIdx = 0; vIdx < vertices.Length; vIdx++)
        {
            // Find the direction from center to vertex, and if insideOut reverse it towards center
            Vector3 normal = vertices[vIdx] * (insideOut ? -1 : 1);
            normal.Normalize();
            normals[vIdx] = normal;
        }
        //for (int t = 0; t < triangleGroundList.Count; t++)
        //{
        //    Debug.Log(String.Format("{0} -> {1}", t, triangleGroundList[t]));
        //}

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.subMeshCount = 1;
        mesh.SetTriangles(triangleGroundList,0);
        //mesh.SetTriangles(triangleWallList, 1);

        ExtrudeWalls();
        var meshc = GetComponent<MeshCollider>();
        meshc.sharedMesh = mesh;
        return;
    }
    private void AddTriangleToList(List<int> triList, int tri1, int tri2, int tri3, bool reverse)
    {
        if (reverse)
        {
            triList.Add(tri3);
            triList.Add(tri2);
            triList.Add(tri1);
        }
        else
        {
            triList.Add(tri1);
            triList.Add(tri2);
            triList.Add(tri3);
        }
    }

    private void GenerateAndExtrude()
    {
        // TODO Create walls, perhaps using dictionary to help

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();

        mesh.name = "Procedural Grid";

        vertices = new Vector3[4 * xSize * ySize]; // 4 vertices per square
        Vector3[] normals = new Vector3[vertices.Length];
        int[] triangles = new int[xSize * ySize * 6];
        double deltaPhi = 2 * Math.PI / xSize;
        double deltaTheta = Math.PI / ySize;
        double r = radius;
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                int i = 2 * x + (4 * xSize) * y;
                double phi = 2 * Math.PI * x / xSize; // X angle
                double theta = Math.PI * y / ySize; // Y angle 
                // Boxes are arranged 0 1 2    Each Square idx is arranged  0 1
                //                    3 4 5                                 2 3
                // Vertex 0
                vertices[i] = new Vector3(
                    (float)(r * Math.Sin(theta) * Math.Cos(phi)),
                    (float)(r * Math.Cos(theta)),
                    (float)(r * Math.Sin(theta) * Math.Sin(phi)));
                // Vertex 1
                vertices[i + 1] = new Vector3(
                    (float)(r * Math.Sin(theta) * Math.Cos(phi + deltaPhi)),
                    (float)(r * Math.Cos(theta)),
                    (float)(r * Math.Sin(theta) * Math.Sin(phi + deltaPhi)));
                // Vertex 2
                vertices[i + 2 * xSize] = new Vector3(
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Cos(phi)),
                    (float)(r * Math.Cos(theta + deltaTheta)),
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Sin(phi)));
                // Vertex 3
                vertices[i + 1 + 2 * xSize] = new Vector3(
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
        for (int vIdx = 0; vIdx < vertices.Length; vIdx++)
        {
            // Find the direction from center to vertex, and if insideOut reverse it towards center
            Vector3 normal = vertices[vIdx] * (insideOut ? -1 : 1);
            normal.Normalize();
            normals[vIdx] = normal;
        }
        //for (int t = 0; t < triangles.Length; t++)
        //{
        //    Debug.Log(String.Format("{0} -> {1}", t, triangles[t]));
        //}
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;

        ExtrudeWalls();
        var meshc = GetComponent<MeshCollider>();
        meshc.sharedMesh = mesh;
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

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.TransformPoint(lastClickPoint), (float) radius/8f);

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

    void OnMouseDown()
    {
        

        var mousePos = Input.mousePosition;
        var hit = new RaycastHit();
        var ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit))
        {
            var clickPos = hit.point;
            var relativePos = hit.point - transform.position;
            Quaternion undoRotation = Quaternion.Inverse(transform.rotation);
            var relativeDir = lastClickPoint = undoRotation* relativePos;
            lastClickPoint = relativeDir;
            relativeDir.Normalize();
            Debug.Log(String.Format("MouseP {0} ClickP {1} RelP {2} RelDir {3}",
                mousePos, clickPos, relativePos, relativeDir));

            // TODO Convert into angles using my spherical coordinates
        }
    }
    

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected");
       
        if (lastInsideOutSetting != insideOut)
        {
            lastInsideOutSetting = insideOut;
            GenerateAndExtrude();
            Debug.Log("Changed settings");
        }
    }

    void OnDestroy()
    {
        vertices = null;
    }


}
