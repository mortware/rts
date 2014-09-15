//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(MeshCollider))]
//[RequireComponent(typeof(MeshFilter))]
//public class Chunk : MonoBehaviour
//{
//
//    public byte[, ,] map;
//    public Mesh visualMesh;
//    protected MeshRenderer meshRenderer;
//    protected MeshCollider meshCollider;
//    protected MeshFilter meshFilter;
//
//    // Use this for initialization
//    void Start()
//    {
//
//        meshRenderer = GetComponent<MeshRenderer>();
//        meshCollider = GetComponent<MeshCollider>();
//        meshFilter = GetComponent<MeshFilter>();
//
//
//        map = new byte[World.currentWorld.chunkWidth, World.currentWorld.chunkHeight, World.currentWorld.chunkWidth];
//
//        for (int x = 0; x < World.currentWorld.chunkWidth; x++)
//        {
//            for (int z = 0; z < World.currentWorld.chunkWidth; z++)
//            {
//                map[x, 0, z] = 1;
//                map[x, 1, z] = (byte)Random.Range(0, 1);
//            }
//        }
//
//        CreateVisualMesh();
//
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//
//    }
//
//    public virtual void CreateVisualMesh()
//    {
//        visualMesh = new Mesh();
//
//        List<Vector3> verts = new List<Vector3>();
//        List<Vector2> uvs = new List<Vector2>();
//        List<int> tris = new List<int>();
//
//
//        for (int x = 0; x < World.currentWorld.chunkWidth; x++)
//        {
//            for (int y = 0; y < World.currentWorld.chunkHeight; y++)
//            {
//                for (int z = 0; z < World.currentWorld.chunkWidth; z++)
//                {
//                    if (map[x, y, z] == 0) continue;
//
//                    byte brick = map[x, y, z];
//                    BuildFace(brick, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
//                    BuildFace(brick, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);
//
//                    BuildFace(brick, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
//                    BuildFace(brick, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);
//
//                    BuildFace(brick, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
//                    BuildFace(brick, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
//
//
//                }
//            }
//        }
//
//        visualMesh.vertices = verts.ToArray();
//        visualMesh.uv = uvs.ToArray();
//        visualMesh.triangles = tris.ToArray();
//        visualMesh.RecalculateBounds();
//        visualMesh.RecalculateNormals();
//
//        meshFilter.mesh = visualMesh;
//        meshCollider.sharedMesh = visualMesh;
//
//    }
//    public virtual void BuildFace(byte brick, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
//    {
//        int index = verts.Count;
//
//        verts.Add(corner);
//        verts.Add(corner + up);
//        verts.Add(corner + up + right);
//        verts.Add(corner + right);
//
//        uvs.Add(new Vector2(0, 0));
//        uvs.Add(new Vector2(0, 1));
//        uvs.Add(new Vector2(1, 1));
//        uvs.Add(new Vector2(1, 0));
//
//        if (reversed)
//        {
//            tris.Add(index + 0);
//            tris.Add(index + 1);
//            tris.Add(index + 2);
//            tris.Add(index + 2);
//            tris.Add(index + 3);
//            tris.Add(index + 0);
//        }
//        else
//        {
//            tris.Add(index + 1);
//            tris.Add(index + 0);
//            tris.Add(index + 2);
//            tris.Add(index + 3);
//            tris.Add(index + 2);
//            tris.Add(index + 0);
//        }
//
//    }
//}
//
//
