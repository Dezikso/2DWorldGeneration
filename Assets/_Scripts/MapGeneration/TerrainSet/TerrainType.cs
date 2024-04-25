using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public TileBase tile;
}
