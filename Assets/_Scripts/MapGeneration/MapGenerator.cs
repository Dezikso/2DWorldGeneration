using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{   
    [Header("Dimensions")]
    [SerializeField] private int mapWidth = 100;
    [SerializeField] private int mapHeight = 100;
    [SerializeField] private float noiseScale = 20;
    [SerializeField] private Vector2 offset = Vector2.zero;

    [Header("Noise Modifiers")]
    [Range(0, 24)][SerializeField] private int octaves = 4;
    [Range(0,1)][SerializeField] private float persistance = 0.5f;
    [Range(1, 15)][SerializeField] private float lacunarity = 2;

    [Header("Generation")]
    [SerializeField] private int seed;
    [SerializeField] private TerrainSetSO terrainSet;

    [Header("References")]
    [SerializeField] private Tilemap tilemap;

    private Dictionary<Vector2Int, TileData> tileDataGrid = new Dictionary<Vector2Int, TileData>();

    //Custom Editor Properties
    public bool autoUpdate;


    private void Start()
    {
        GenerateMap();   
    }


    public void ClearTilemap() //Called by MapGenerationEditor.cs
    {
        tileDataGrid.Clear();
        tilemap.ClearAllTiles();
    }

    public void GenerateMap() //Called by MapGenerationEditor.cs
    {
        ClearTilemap();

        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, offset, octaves, persistance, lacunarity, seed);

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];

                foreach (TerrainType terrainType in terrainSet.terrainTypes)
                {
                    if (currentHeight <= terrainType.height)
                    {
                        Vector2Int tilePosition2D = new Vector2Int(x, y);
                        tileDataGrid[tilePosition2D] = new TileData(terrainType.id, false);

                        Vector3Int tilePosition3D = new Vector3Int(x, y, 0);
                        tilemap.SetTile(tilePosition3D, terrainType.tile);
                        break; 
                    }
                }
            }
        }
    }

    public bool GetTileData(Vector2Int position, out TileData tileData)
    {
        return tileDataGrid.TryGetValue(position, out tileData);
    }


    private void OnValidate()
    {
        if (mapWidth <1)
            mapWidth = 1;

        if (mapHeight < 1)
            mapHeight = 1;
    }

}

