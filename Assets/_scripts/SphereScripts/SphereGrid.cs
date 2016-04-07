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
    public Material[] materials;
    private MeshHolder ground;
    private MeshHolder vertWalls;
    private MeshHolder horzWalls;

    private Vector3[] vertices;
    private Mesh mesh;
    private bool[,] wallHere;
    private Vector3 lastClickPoint;
    private int N;

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

    private bool isWall(int x, int y)
    {
        x = (x + xSize)%xSize;
        return wallHere[x, y];
    }

    private void ExtrudePoint(ref Vector3 point)
    {
        Vector3 extrudeDir = point.normalized;
        extrudeDir *= wallHeight;
        if (insideOut)
            extrudeDir *= -1;
        point += extrudeDir;
    }
    
    void SetUpWallPlan()
    {
        wallHere = new bool[xSize, ySize];
        string wallPattern = wallPatternString.ToLower();
        switch (wallPattern)
        {
            case "all walls":
                SetWallsToBool(true); 
                break;
            case "all floors":
                SetWallsToBool(false);
                break;
            case "test":
                SetTestWallPlan();
                break;
            case "half":
                SetTopHalfToWalls();
                break;
            default:
                SetWallsToBool(false); // Default to all floors
                break;
        }
    }

    private void SetTopHalfToWalls()
    {
        for (int y = 0; y < ySize/2; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                wallHere[x, y] = true;
            }
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
        mesh.Clear();
        // TODO add multiple materials for ground/floors etc
        GetComponent<Renderer>().materials = materials;
        mesh.name = "Procedural Grid";

        N = 4*xSize*ySize;
        vertices = new Vector3[4 * xSize * ySize]; // 4 vertices per square
        Vector3[] normals = new Vector3[vertices.Length];
        //int[] triangles = new int[xSize * ySize * 6];
        List<int> triangleGroundList = new List<int>();
        List<int> triangleWallList = new List<int>();
        double deltaPhi = 2*Math.PI/xSize;
        double deltaTheta = Math.PI/ySize;
        double r = radius;

        // Set vertices
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
                // int ti = 6 * x + (6 * xSize) * y;
                int[] idxs = { i, i + 1, i + 2 * xSize, i + 1 + 2 * xSize };
                int TL = i;
                int TR = i + 1;
                int BL = i + 2 * xSize;
                int BR = i + 2 * xSize + 1;
                // Add both triangles, reversing order if inside out. 
                // TODO add normals to this
                AddTriangleToList(triangleGroundList, TL, TR, BR, insideOut);
                AddTriangleToList(triangleGroundList, TL, BR, BL, insideOut);

                // Add walls between this square and square to left
                AddLatitudeWall(triangleWallList, (x-1), y, x, y);
                // Add walls between this square and square above (towards north pole)
                if (y > 0) // Don't add if at north pole
                    AddLongitudeWall(triangleWallList, x, (y - 1), x, y);

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
        int subMeshCount = 0;
        mesh.subMeshCount = 2;
        if (triangleGroundList.Count > 0)
        {
            mesh.SetTriangles(triangleGroundList, 0);
        }
        if (triangleWallList.Count > 0)
        {
            mesh.SetTriangles(triangleWallList, 2);
            Debug.Log(String.Format("wall count {0}", triangleWallList.Count/6));
        }
        else
        {
            Debug.Log(String.Format("f-wall count {0}", triangleWallList.Count / 6));
        }
        //mesh.subMeshCount = subMeshCount;

        ExtrudeWalls();
        var meshc = GetComponent<MeshCollider>();
        meshc.sharedMesh = mesh;
        return;
    }
    
    // Given the x,y coords of two patches, add vertical wall if one is wall, one is floor
    private void AddLatitudeWall(List<int> triList, int leftSquareX, int leftSquareY, int rightSquareX, int rightSquareY)
    {

        // TODO Verify
        leftSquareX = (leftSquareX + xSize)%xSize;
        //Debug.Log(String.Format("Lx {0} Ly {1} Rx {2} Ry{3}", leftSquareX, leftSquareY, rightSquareX, rightSquareY));
        bool leftType = wallHere[leftSquareX, leftSquareY];
        bool rightType = wallHere[rightSquareX, rightSquareY];
        // Don't add wall between if they're both walls/floor
        if (leftType == rightType) 
            return;
        // Idx of top left vertex for left Square
        int leftVertIdx = 2 * leftSquareX + (4 * xSize) * leftSquareY;
        int left_TR_vert = leftVertIdx + 1;
        int left_BR_vert = leftVertIdx + 1 + 2*xSize;
        int rightVertIdx = 2 * rightSquareX + (4 * xSize) * rightSquareY;
        int right_TL_vert = rightVertIdx;
        int right_BL_vert = rightVertIdx + 2 * xSize;
        //int[] idxs = { i, i + 1, i + 2 * xSize, i + 1 + 2 * xSize };
        AddTriangleToList(triList, left_BR_vert, left_TR_vert, right_TL_vert, insideOut);
        AddTriangleToList(triList, left_BR_vert, right_TL_vert, right_BL_vert, insideOut);
    }
    // Given the x,y coords of two patches, add vertical wall if one is wall, one is floor
    private void AddLongitudeWall(List<int> triList, int topSquareX, int topSquareY, int bottomSquareX, int bottomSquareY)
    {
        // TODO Verify
        
        bool topType = wallHere[topSquareX, topSquareY];
        bool botType = wallHere[bottomSquareX, bottomSquareY];
        // Don't add wall between if they're both walls/floor
        if (topType == botType)
            return;
        // Idx of top left vertex for left Square
        int topVertIdx = 2 * topSquareX + (4 * xSize) * topSquareY;
        int top_BL_vert = topVertIdx + 2 * xSize;
        int top_BR_vert = topVertIdx + 2*xSize + 1;
        int botVertIdx = 2 * bottomSquareX + (4 * xSize) * bottomSquareY;
        int bot_TL_vert = botVertIdx;
        int bot_TR_vert = botVertIdx + 1;
        //int[] idxs = { i, i + 1, i + 2 * xSize, i + 1 + 2 * xSize };
        AddTriangleToList(triList, top_BL_vert, top_BR_vert, bot_TR_vert, insideOut);
        AddTriangleToList(triList, top_BL_vert, bot_TR_vert, bot_TL_vert, insideOut);
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

    #region Old GenerateExtrude


    private void GenerateAndExtrude()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.Clear();
        // TODO add multiple materials for ground/floors etc
        GetComponent<Renderer>().materials = materials;
        mesh.name = "Procedural Sphere";

        N = 4 * xSize * ySize;

        // Used to calculate angle of verts relative to top-left vert
        double deltaPhi = 2 * Math.PI / xSize;
        double deltaTheta = Math.PI / ySize;
        double r = radius;

        ground = new MeshHolder(0);
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                double phi = 2 * Math.PI * x / xSize; // X angle
                double theta = Math.PI * y / ySize; // Y angle 

                var p0 = new Vector3( // Add TL vert
                    (float) (r*Math.Sin(theta)*Math.Cos(phi)),
                    (float) (r*Math.Cos(theta)),
                    (float) (r*Math.Sin(theta)*Math.Sin(phi)));
                var p1 = new Vector3( // Add TR vert
                    (float) (r*Math.Sin(theta)*Math.Cos(phi + deltaPhi)),
                    (float) (r*Math.Cos(theta)),
                    (float) (r*Math.Sin(theta)*Math.Sin(phi + deltaPhi)));
                var p2 = new Vector3( // Add BL vert
                    (float) (r*Math.Sin(theta + deltaTheta)*Math.Cos(phi)),
                    (float) (r*Math.Cos(theta + deltaTheta)),
                    (float) (r*Math.Sin(theta + deltaTheta)*Math.Sin(phi)));
                var p3 = new Vector3( // Add BR vert
                    (float) (r*Math.Sin(theta + deltaTheta)*Math.Cos(phi + deltaPhi)),
                    (float) (r*Math.Cos(theta + deltaTheta)),
                    (float) (r*Math.Sin(theta + deltaTheta)*Math.Sin(phi + deltaPhi)));
                ground.AddSquare(p0, p1, p2, p3, insideOut);
            }
        }
        vertWalls = new MeshHolder(ground.nextTriIdx);
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                double phi = 2 * Math.PI * x / xSize; // X angle
                double theta = Math.PI * y / ySize; // Y angle 
                
                var left_TR = new Vector3( // Add TL vert
                    (float)(r * Math.Sin(theta) * Math.Cos(phi)),
                    (float)(r * Math.Cos(theta)),
                    (float)(r * Math.Sin(theta) * Math.Sin(phi)));
                var left_BR = new Vector3( // Add BL vert
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Cos(phi)),
                    (float)(r * Math.Cos(theta + deltaTheta)),
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Sin(phi)));
                if (isWall(x - 1, y))
                {
                    ExtrudePoint(ref left_TR);
                    ExtrudePoint(ref left_BR);
                }
                var right_TL = new Vector3( // Add TL vert
                    (float)(r * Math.Sin(theta) * Math.Cos(phi)),
                    (float)(r * Math.Cos(theta)),
                    (float)(r * Math.Sin(theta) * Math.Sin(phi)));
                var right_BL = new Vector3( // Add BL vert
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Cos(phi)),
                    (float)(r * Math.Cos(theta + deltaTheta)),
                    (float)(r * Math.Sin(theta + deltaTheta) * Math.Sin(phi)));
                if (isWall(x, y))
                {
                    ExtrudePoint(ref right_TL);
                    ExtrudePoint(ref right_BL);
                }
                vertWalls.AddSquare(left_BR, left_TR, right_BL, right_TL, insideOut);
            }
        }

        horzWalls = new MeshHolder(vertWalls.nextTriIdx);
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
            }
        }





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

    private void AddVerticalWall(List<int> triList,
        int leftSquareX, int leftSquareY,
        int rightSquareX, int rightSquareY)
    {
        leftSquareX = (leftSquareX + xSize) % xSize; // Wrap the index around
        bool leftType = wallHere[leftSquareX, leftSquareY];
        bool rightType = wallHere[rightSquareX, rightSquareY];
    }
    #endregion


    #region Special Functions "On_blah"
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
    #endregion

}
