using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{
    private const int TrianglesPerTile = 4;
    private const int VerticesPerTile = 5;
    private const float uvu = 0.083333f; // UV unit


    protected List<Tile> Tiles = new List<Tile>();

    public int SizeX = 2;
    public int SizeZ = 2;
    public float TileSize = 1.0f;
    public Texture2D TileTextures;

    protected Vector3 Offset
    {
        get { return ((new Vector3(SizeX, 0, SizeZ) * TileSize) / 2) + new Vector3(TileSize / 2, 0, TileSize / 2); }
    }

    private void Start()
    {
        BuildMesh();
    }

    private void Update()
    {

    }

    public void BuildMesh()
    {
        var filter = GetComponent<MeshFilter>();
        var mesh = filter.sharedMesh;
        mesh.Clear();

        int numTiles = SizeX * SizeZ;
        int numTris = numTiles * TrianglesPerTile;

        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();

        for (int x = 0; x < SizeX; x++)
        {
            for (int z = 0; z < SizeZ; z++)
            {
                var tile = new Tile
                {
                    Position = (new Vector3(x, 0, z) * TileSize) + (Vector3.forward * TileSize / 2) + (Vector3.right * TileSize / 2) - Offset,
                    Height = ((float)Math.Round(Random.Range(0.0f, 0.0f) * 4, MidpointRounding.ToEven) / 4) + (TileSize / 4)
                };
                this.Tiles.Add(tile);
                BuildTileMesh(tile, vertices, uv, triangles);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.RecalculateBounds();

        var meshRenderer = GetComponent<MeshRenderer>();
        var meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        filter.mesh = mesh;
    }

    private void BuildTileMesh(Tile tile, List<Vector3> verts, List<Vector2> uvs, List<int> triangles)
    {
        int index = verts.Count;

        // Relative Measurements
        var corner = tile.Position + (Vector3.up * tile.Height);
        var forward = (Vector3.forward * TileSize) / 4;
        var right = (Vector3.right * TileSize) / 4;
        var ground = new Vector3(0, -corner.y, 0);

        var length = TileSize;
        var width = TileSize;
        var height = tile.Height;

        var center = tile.Position + new Vector3(TileSize /2, 0, TileSize/2);

//        var p0 = new Vector3()


        // Top
        var bottomLeft = corner + (forward * 0) + (right * 0);
        verts.Add(bottomLeft);
        verts.Add(corner + (forward * 2) + (right * 0));
        var topLeft = corner + (forward * 4) + (right * 0);
        verts.Add(topLeft);
        verts.Add(corner + (forward * 1) + (right * 1));
        verts.Add(corner + (forward * 3) + (right * 1));
        verts.Add(corner + (forward * 0) + (right * 2));
        verts.Add(corner + (forward * 2) + (right * 2));
        verts.Add(corner + (forward * 4) + (right * 2));
        verts.Add(corner + (forward * 1) + (right * 3));
        verts.Add(corner + (forward * 3) + (right * 3));
        var bottomRight = corner + (forward * 0) + (right * 4);
        verts.Add(bottomRight);
        verts.Add(corner + (forward * 2) + (right * 4));
        var topRight = corner + (forward * 4) + (right * 4);
        verts.Add(topRight);

        var uvStart = new Vector2((float)TileTextures.width / 3, (float)TileTextures.height / 3);

        uvs.Add(new Vector2(uvStart.x + uvu * 0, uvStart.y + uvu * 0));
        uvs.Add(new Vector2(uvStart.x + uvu * 0, uvStart.y + uvu * 2));
        uvs.Add(new Vector2(uvStart.x + uvu * 0, uvStart.y + uvu * 4));

        uvs.Add(new Vector2(uvStart.x + uvu * 1, uvStart.y + uvu * 1));
        uvs.Add(new Vector2(uvStart.x + uvu * 1, uvStart.y + uvu * 3));

        uvs.Add(new Vector2(uvStart.x + uvu * 2, uvStart.y + uvu * 0));
        uvs.Add(new Vector2(uvStart.x + uvu * 2, uvStart.y + uvu * 2));
        uvs.Add(new Vector2(uvStart.x + uvu * 2, uvStart.y + uvu * 4));

        uvs.Add(new Vector2(uvStart.x + uvu * 3, uvStart.y + uvu * 1));
        uvs.Add(new Vector2(uvStart.x + uvu * 3, uvStart.y + uvu * 3));

        uvs.Add(new Vector2(uvStart.x + uvu * 4, uvStart.y + uvu * 0));
        uvs.Add(new Vector2(uvStart.x + uvu * 4, uvStart.y + uvu * 2));
        uvs.Add(new Vector2(uvStart.x + uvu * 4, uvStart.y + uvu * 4));
        //        uvs.Add(new Vector2(uvStart.x + uvu * 8, uvStart.y + uvu * 4));
        //        uvs.Add(new Vector2(uvStart.x + uvu * 8, uvStart.y + uvu * 6));
        //        uvs.Add(new Vector2(uvStart.x + uvu * 8, uvStart.y + uvu * 8));

        // A
        triangles.Add(index + 0);
        triangles.Add(index + 1);
        triangles.Add(index + 3);

        triangles.Add(index + 0);
        triangles.Add(index + 3);
        triangles.Add(index + 5);

        triangles.Add(index + 5);
        triangles.Add(index + 3);
        triangles.Add(index + 6);

        triangles.Add(index + 1);
        triangles.Add(index + 6);
        triangles.Add(index + 3);

        // B
        triangles.Add(index + 5);
        triangles.Add(index + 6);
        triangles.Add(index + 8);

        triangles.Add(index + 5);
        triangles.Add(index + 8);
        triangles.Add(index + 10);

        triangles.Add(index + 10);
        triangles.Add(index + 8);
        triangles.Add(index + 11);

        triangles.Add(index + 6);
        triangles.Add(index + 11);
        triangles.Add(index + 8);

        // C
        triangles.Add(index + 1);
        triangles.Add(index + 2);
        triangles.Add(index + 4);

        triangles.Add(index + 1);
        triangles.Add(index + 4);
        triangles.Add(index + 6);

        triangles.Add(index + 6);
        triangles.Add(index + 4);
        triangles.Add(index + 7);

        triangles.Add(index + 2);
        triangles.Add(index + 7);
        triangles.Add(index + 4);

        // D
        triangles.Add(index + 6);
        triangles.Add(index + 7);
        triangles.Add(index + 9);

        triangles.Add(index + 6);
        triangles.Add(index + 9);
        triangles.Add(index + 11);

        triangles.Add(index + 11);
        triangles.Add(index + 9);
        triangles.Add(index + 12);

        triangles.Add(index + 7);
        triangles.Add(index + 12);
        triangles.Add(index + 9);

        //        uvStart = new Vector2((float)TileTextures.width / 3, 0);
        //        BuildSide(bottomLeft, Vector3.right, uvStart, verts, uvs, triangles);
        //
        //        uvStart = new Vector2(0, (float)TileTextures.height / 3);
        //        BuildSide(topLeft, Vector3.back, uvStart, verts, uvs, triangles);
        //
        //        uvStart = new Vector2((float)TileTextures.width / 3, ((float)TileTextures.height / 3) * 2);
        //        BuildSide(topRight, Vector3.left, uvStart, verts, uvs, triangles);
        //
        //        uvStart = new Vector2(((float)TileTextures.width / 3) * 2, (float)TileTextures.height / 3);
        //        BuildSide(bottomRight, Vector3.forward, uvStart, verts, uvs, triangles);
    }
    private void BuildSide(Vector3 corner, Vector3 direction, Vector2 uvStart, List<Vector3> verts, List<Vector2> uvs, List<int> triangles)
    {
        Debug.Log(uvStart);
        int index = verts.Count;

        var across = (direction * TileSize) / 2;
        var down = (Vector3.down * TileSize);
        var ground = new Vector3(0, -corner.y, 0);

        verts.Add(corner + (across * 0) + down * 0);
        verts.Add(corner + (across * 1) + down * 0);
        verts.Add(corner + (across * 2) + down * 0);
        verts.Add(corner + (across * 0) + ground);
        verts.Add(corner + (across * 1) + ground);
        verts.Add(corner + (across * 2) + ground);


        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.zero);

        //        uvs.Add(new Vector2(uvStart.x + uvu * 0, uvStart.y + uvu * 0));
        //        uvs.Add(new Vector2(uvStart.x + uvu * 2, uvStart.y + uvu * 0));
        //        uvs.Add(new Vector2(uvStart.x + uvu * 4, uvStart.y + uvu * 0));
        //        uvs.Add(new Vector2(uvStart.x + uvu * 0, uvStart.y + uvu * 4));
        //        uvs.Add(new Vector2(uvStart.x + uvu * 2, uvStart.y + uvu * 4));
        //        uvs.Add(new Vector2(uvStart.x + uvu * 4, uvStart.y + uvu * 4));

        triangles.Add(index + 3);
        triangles.Add(index + 0);
        triangles.Add(index + 1);

        triangles.Add(index + 3);
        triangles.Add(index + 1);
        triangles.Add(index + 4);

        triangles.Add(index + 4);
        triangles.Add(index + 1);
        triangles.Add(index + 5);

        triangles.Add(index + 1);
        triangles.Add(index + 2);
        triangles.Add(index + 5);
    }

    private Tile GetTileAtMousePos()
    {
        return null;
    }

    public Vector3 GetTileCenter(Vector3 point)
    {
        point = new Vector3(
            (float)Math.Floor(point.x / TileSize) * TileSize,
            point.y,
            (float)Math.Floor(point.z / TileSize) * TileSize);

        return point + new Vector3(TileSize / 2, 0, TileSize / 2);
    }
}