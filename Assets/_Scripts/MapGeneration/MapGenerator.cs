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

    //Custom Editor Properties
    public bool autoUpdate;


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
                        Vector3Int tilePosition = new Vector3Int(x, y, 0);
                        tilemap.SetTile(tilePosition, terrainType.tile);
                        break; 
                    }
                }
            }
        }
    }

    public void ClearTilemap() //Called by MapGenerationEditor.cs
    {
        tilemap.ClearAllTiles();
    }


    private void OnValidate()
    {
        if (mapWidth <1)
            mapWidth = 1;

        if (mapHeight < 1)
            mapHeight = 1;
    }

}

