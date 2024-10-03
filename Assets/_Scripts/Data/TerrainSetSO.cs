using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTerrainSet", menuName = "MapGeneration/TerrainSet")]
public class TerrainSetSO : ScriptableObject
{
    public TerrainType[] terrainTypes;
}

[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public TileBase tile;
}