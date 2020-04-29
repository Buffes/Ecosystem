using UnityEngine;

namespace Ecosystem.Grid
{
    public static class Noise 
    {
        /// <summary>
        /// Generates a noise map based on Perlin noise and the specified parameters.
        /// </summary>
        /// <param name="width">The width of the map.</param>
        /// <param name="height">The height of the map.</param>
        /// <param name="scale">The scale of the map. At 1 this will return a map with the same value everywhere.</param>
        /// <param name="octaves"></param>
        /// <param name="persistence"></param>
        /// <param name="lacunarity"></param>
        /// <returns></returns>
        public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity)
        {
            
            System.Random random = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = random.Next(-100000, 100000);
                float offsetY = random.Next(-100000, 100000);
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if (scale <= 0f)
            {
                scale = 0.0001f; // Avoid division by 0.
            }

            float[,] noiseMap = new float[width, height]; 
            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {        
                        float xInput = x / scale * frequency + octaveOffsets[i].x;
                        float yInput = y / scale * frequency + octaveOffsets[i].y;

                        float perlinValue = 2f * Mathf.PerlinNoise(xInput, yInput) - 1f;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                    if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
                    noiseMap[x, y] = noiseHeight;
                }            
            }
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }
            
            return noiseMap;
        }
    }
}