using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public bool AutoUpdate { get => _autoUpdate; }

    [Header("Dimensions")]
    [SerializeField] private int _mapWidth = 100;
    [SerializeField] private int _mapHeight = 100;
    [SerializeField] private float _noiseScale = 20;
    [SerializeField] private Vector2 _offset = Vector2.zero;

    [Header("Noise Modifiers")]
    [Range(0, 24)][SerializeField] private int _octaves = 4;
    [Range(0, 1)][SerializeField] private float _persistance = 0.5f;
    [Range(1, 15)][SerializeField] private float _lacunarity = 2;

    [Header("Generation")]
    [SerializeField] private int _seed;
    [SerializeField] private TerrainSetSO _terrainSet;
    [SerializeField] private EnvironmentObjectSetSO _environmentObjectSet;

    [Header("References")]
    [SerializeField] private Tilemap _tilemap;

    [Header("Editor")]
    [SerializeField] private bool _autoUpdate;

    private Dictionary<Vector2Int, TileData> _tileDataGrid = new Dictionary<Vector2Int, TileData>();


    public void RandomizeSeed()
    {
        _seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }

    public void GenerateMap()
    {
        ClearTilemap();
        ClearEnvironment();
        GenerateTilemap();
        SpawnEnvironmentObjects();
    }

    private void ClearTilemap()
    {
        _tileDataGrid.Clear();
        _tilemap.ClearAllTiles();
    }

    private void ClearEnvironment()
    {
        _tileDataGrid.Clear();
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in _tilemap.transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }
    }

    private void GenerateTilemap()
    {
        TileBase[] tiles = new TileBase[_mapWidth * _mapHeight];
        Vector3Int[] tilePositions = new Vector3Int[_mapWidth * _mapHeight];

        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(_mapWidth, _mapHeight, _noiseScale, _offset, _octaves, _persistance, _lacunarity, _seed);

        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];

                foreach (TerrainType terrainType in _terrainSet.terrainTypes)
                {
                    if (currentHeight <= terrainType.height)
                    {
                        Vector2Int tilePosition2D = new Vector2Int(x, y);
                        _tileDataGrid[tilePosition2D] = new TileData(Array.IndexOf(_terrainSet.terrainTypes, terrainType), false);

                        tilePositions[y * _mapWidth + x] = new Vector3Int(x, y, 0);
                        tiles[y * _mapWidth + x] = terrainType.tile;
                        break;
                    }
                }
            }
        }

        _tilemap.SetTiles(tilePositions, tiles);
    }

    private void SpawnEnvironmentObjects()
    {
        if (_tileDataGrid.Count <= 1)
        {
            Debug.LogError("Cannot spawn objects on an empty tilemap.");
            return;
        }

        UnityEngine.Random.InitState(_seed);

        foreach (EnvironmentObjectType objectType in _environmentObjectSet.environmentObjectTypes)
        {
            int objectsToSpawn = objectType.spawnAmount;
            int objectsSpawned = 0;
            int maxSpawnAttempts = objectsToSpawn * 10;

            List<Vector2Int> availableTiles = GetAvailableTilesForTerrain(objectType.terrainTypeID);

            while (objectsSpawned < objectsToSpawn && maxSpawnAttempts > 0)
            {
                if (availableTiles.Count == 0)
                {
                    Debug.LogError($"No available tiles for spawning {objectType.name}.");
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

                if (!_tileDataGrid.ContainsKey(tilePos) || _tileDataGrid[tilePos].isOccupied)
                    return false;
            }
        }
        return true;
    }

    private void SpawnObjectAtTile(Vector2Int tilePosition, EnvironmentObjectType objectType)
    {
        if (objectType.objectPrefab != null)
        {
            GameObject spawnedObject = Instantiate(objectType.objectPrefab, new Vector3(tilePosition.x, tilePosition.y, 0), Quaternion.identity);
            spawnedObject.transform.SetParent(_tilemap.transform);

            for (int y = tilePosition.y; y <= tilePosition.y + objectType.height - 1; y++)
            {
                for (int x = tilePosition.x; x <= tilePosition.x + objectType.width - 1; x++)
                {
                    _tileDataGrid[new Vector2Int(x, y)].isOccupied = true;
                }
            }
        }
    }

    private List<Vector2Int> GetAvailableTilesForTerrain(int terrainTypeID)
    {
        List<Vector2Int> availableTiles = new List<Vector2Int>();

        foreach (var tileData in _tileDataGrid)
        {
            if (!tileData.Value.isOccupied && tileData.Value.terrainTypeID == terrainTypeID)
                availableTiles.Add(tileData.Key);
        }

        return availableTiles;
    }
}


