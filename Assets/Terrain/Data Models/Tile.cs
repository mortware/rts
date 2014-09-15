using UnityEngine;
using System.Collections;

public class Tile
{
    public TileType Type { set; get; }

    public Vector3 Position { get; set; }
    public float Height { get; set; }

    public Tile()
    {

    }
}

public enum TileType
{
    Grass,
    Rock,
    Water
}
