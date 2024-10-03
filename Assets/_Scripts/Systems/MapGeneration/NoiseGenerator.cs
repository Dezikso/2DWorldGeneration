using UnityEngine;

public class NoiseGenerator
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, Vector2 offset, int octaves, float persistance, float lacunarity, int seed)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        Vector2[] octaveOffsets = GenerateOctaveOffsets(octaves, offset, seed);

        if (scale <= 0)
            scale = 0.0001f;


        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++) 
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity; 
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x,y] = noiseHeight;
            }
        }

        noiseMap = NormalizeNoiseMap(noiseMap, mapHeight, mapWidth, minNoiseHeight, maxNoiseHeight);

        return noiseMap;
    }


    private static Vector2[] GenerateOctaveOffsets(int octaves, Vector2 offset, int seed)
    {
        Random.InitState(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = Random.Range(-100000, 100000) + offset.x;
            float offsetY = Random.Range(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        return octaveOffsets;
    }


    private static float[,] NormalizeNoiseMap(float[,] noiseMap, int mapHeight, int mapWidth, float minNoiseHeight, float maxNoiseHeight)
    {
        float[,] normalizedNoiseMap = noiseMap;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                normalizedNoiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, normalizedNoiseMap[x, y]);
            }
        }

        return normalizedNoiseMap;
    }
}
