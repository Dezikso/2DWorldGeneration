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
    [SerializeField] private EnvironmentObjectSetSO environmentObjectSet;

    [Header("References")]
    [SerializeField] private Tilemap tilemap;

    private Dictionary<Vector2Int, TileData> tileDataGrid = new Dictionary<Vector2Int, TileData>();

    //Custom Editor Properties
    public bool autoUpdate;


    private void Start()
    {
        ClearTilemap();
        GenerateMap();   
    }

    private void SpawnEnvironmentObjects()
    {
        Random.InitState(seed);

        foreach (EnvironmentObjectType objectType in environmentObjectSet.environmentObjectTypes)
        {
            int density = objectType.density;
            List<Vector2Int> availableTiles = GetAvailableTiles(objectType.terrainTypeID);

            if (availableTiles.Count < density)
            {
                Debug.LogWarning($"The density of {objectType.name} is higher than avidable tiles!");
                density = availableTiles.Count - 1;
            }

            for (int i = 0; i < density; i++)
            {
                int randomIndex = Random.Range(0, availableTiles.Count);
                Vector2Int selectedTile = availableTiles[randomIndex];
                availableTiles.RemoveAt(randomIndex);

                tileDataGrid[selectedTile] = new TileData(objectType.id, true);

                //DEBUG ONLY, DELETE LATER
                Debug.Log(selectedTile.ToString());
            }
        }
    }

    private List<Vector2Int> GetAvailableTiles(int terrainTypeID)
    {
        List<Vector2Int> availableTiles = new List<Vector2Int>();

        foreach (var tileData in tileDataGrid)
        {
            if (tileData.Value.terrainTypeID == terrainTypeID && !tileData.Value.isOccupied)
            {
                availableTiles.Add(tileData.Key);
            }
        }

        return availableTiles;
    }


    public void ClearTilemap() //Called by MapGenerationEditor.cs
    {
        tileDataGrid.Clear();
        tilemap.ClearAllTiles();
    }

    public void GenerateMap() //Called by MapGenerationEditor.cs
    {
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

        SpawnEnvironmentObjects();
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

