using System;
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
    [Range(0, 1)][SerializeField] private float persistance = 0.5f;
    [Range(1, 15)][SerializeField] private float lacunarity = 2;

    [Header("Generation")]
    [SerializeField] private int seed;
    [SerializeField] private TerrainSetSO terrainSet;
    [SerializeField] private EnvironmentObjectSetSO environmentObjectSet;

    [Header("References")]
    [SerializeField] private Tilemap tilemap;

    private Dictionary<Vector2Int, TileData> tileDataGrid = new Dictionary<Vector2Int, TileData>();

    // Custom Editor Properties
    public bool autoUpdate;

    public void RandomizeSeed()
    {
        seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }

    public void GenerateMap()
    {
        ClearTilemap();
        ClearEnvironment();
        GenerateTilemap();
        SpawnEnvironmentObjects();
    }

    public void ClearTilemap() // Called by MapGenerationEditor.cs
    {
        tileDataGrid.Clear();
        tilemap.ClearAllTiles();
    }

    public void ClearEnvironment()
    {
        tileDataGrid.Clear();
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in tilemap.transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }
    }

    public void GenerateTilemap() // Called by MapGenerationEditor.cs
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
                        tileDataGrid[tilePosition2D] = new TileData(Array.IndexOf(terrainSet.terrainTypes, terrainType), false);

                        Vector3Int tilePosition3D = new Vector3Int(x, y, 0);
                        tilemap.SetTile(tilePosition3D, terrainType.tile);
                        break;
                    }
                }
            }
        }
    }

    public void SpawnEnvironmentObjects() // Called by MapGenerationEditor.cs
    {
        if (tileDataGrid.Count <= 1)
        {
            Debug.LogWarning("Cannot spawn objects on an empty tilemap.");
            return;
        }

        UnityEngine.Random.InitState(seed);

        foreach (EnvironmentObjectType objectType in environmentObjectSet.environmentObjectTypes)
        {
            int objectsToSpawn = objectType.spawnAmount;
            int objectsSpawned = 0;
            int maxSpawnAttempts = objectsToSpawn * 10;

            List<Vector2Int> availableTiles = GetAvailableTilesForTerrain(objectType.terrainTypeID);

            while (objectsSpawned < objectsToSpawn && maxSpawnAttempts > 0)
            {
                if (availableTiles.Count == 0)
                {
                    Debug.LogWarning($"No available tiles for spawning {objectType.name}.");
                    break;
                }

                Vector2Int selectedTile = availableTiles[UnityEngine.Random.Range(0, availableTiles.Count)];

                if (CanSpawnObjectAtTile(selectedTile, objectType.width, objectType.height))
                {
                    SpawnObjectAtTile(selectedTile, objectType);
                    objectsSpawned++;
                }

                maxSpawnAttempts--;
            }
        }
    }

    private bool CanSpawnObjectAtTile(Vector2Int selectedTile, int width, int height)
    {
        for (int y = selectedTile.y; y < selectedTile.y + height; y++)
        {
            for (int x = selectedTile.x; x < selectedTile.x + width; x++)
            {
                Vector2Int tilePos = new Vector2Int(x, y);

                if (!tileDataGrid.ContainsKey(tilePos) || tileDataGrid[tilePos].isOccupied)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void SpawnObjectAtTile(Vector2Int tilePosition, EnvironmentObjectType objectType)
    {
        if (objectType.objectPrefab != null)
        {
            GameObject spawnedObject = Instantiate(objectType.objectPrefab, new Vector3(tilePosition.x, tilePosition.y, 0), Quaternion.identity);
            spawnedObject.transform.SetParent(tilemap.transform);

            for (int y = tilePosition.y; y <= tilePosition.y + objectType.height - 1; y++)
            {
                for (int x = tilePosition.x; x <= tilePosition.x + objectType.width - 1; x++)
                {
                    tileDataGrid[new Vector2Int(x, y)].isOccupied = true;
                }
            }
        }
    }

    private List<Vector2Int> GetAvailableTilesForTerrain(int terrainTypeID)
    {
        List<Vector2Int> availableTiles = new List<Vector2Int>();

        foreach (var tileData in tileDataGrid)
        {
            if (!tileData.Value.isOccupied && tileData.Value.terrainTypeID == terrainTypeID)
            {
                availableTiles.Add(tileData.Key);
            }
        }

        return availableTiles;
    }
}


