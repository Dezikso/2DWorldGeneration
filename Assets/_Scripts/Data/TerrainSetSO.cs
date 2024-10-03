using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTerrainSet", menuName = "MapGeneration/TerrainSet")]
public class TerrainSetSO : ScriptableObject
{
    public List<TerrainType> terrainTypes;
}

[System.Serializable]
public class TerrainType
{
    public int id;
    public string name;
    public float height;
    public TileBase tile;

    public TerrainType(int id, string name, float height, TileBase tile)
    {
        this.id = id;
        this.name = name;
        this.height = height;
        this.tile = tile;
    }
}