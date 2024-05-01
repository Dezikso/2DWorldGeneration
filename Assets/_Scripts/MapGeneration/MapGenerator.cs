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
        SpawnEnvironmentObjects();
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
    }

    public void SpawnEnvironmentObjects() //Called by MapGenerationEditor.cs
    {
        if (tileDataGrid.Count <= 1)
        {
            Debug.LogWarning("Cannot spawn objects on an empty tilemap.");
            return;
        }

        Random.InitState(seed);

        foreach (EnvironmentObjectType objectType in environmentObjectSet.environmentObjectTypes)
        {
            int objectsToSpawn = objectType.spawnAmount;
            int objectsSpawned = 0;
            int maxSpawnAttempts = objectsToSpawn * 10;

            List<Vector2Int> availableTiles = GetAvailableTiles(objectType.terrainTypeID);

            while (objectsSpawned < objectsToSpawn && maxSpawnAttempts > 0)
            {
                int randomIndex = Random.Range(0, availableTiles.Count);
                Vector2Int selectedTile = availableTiles[randomIndex];
                bool canSpawnObject = true;

                for (int y = selectedTile.y; y < selectedTile.y + objectType.height; y++)
                {
                    for (int x = selectedTile.x; x < selectedTile.x + objectType.width; x++)
                    {
                        if (!tileDataGrid.ContainsKey(new Vector2Int(x, y)) || tileDataGrid[new Vector2Int(x, y)].isOccupied)
                        {
                            canSpawnObject = false; 
                            break;
                        }
                    }

                    if (!canSpawnObject)
                        break;
                }

                if (canSpawnObject)
                {
                    for (int y = selectedTile.y; y < selectedTile.y + objectType.height; y++)
                    {
                        for (int x = selectedTile.x; x < selectedTile.x + objectType.width; x++)
                        {
                            tileDataGrid[new Vector2Int(x, y)] = new TileData(objectType.id, true);
                        }
                    }

                    Debug.Log(selectedTile);
                    objectsSpawned++;
                }
                else
                {
                    Debug.LogWarning($"Object {objectType.name} cannot be spawned at {selectedTile} because some tiles are occupied.");
                }

                maxSpawnAttempts--;
            }
        }
    }


    private void OnValidate()
    {
        if (mapWidth <1)
            mapWidth = 1;

        if (mapHeight < 1)
            mapHeight = 1;
    }


}

